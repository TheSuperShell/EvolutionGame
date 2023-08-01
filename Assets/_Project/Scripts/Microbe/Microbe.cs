using System;
using System.Collections;
using TheSuperShell.Brains;
using TheSuperShell.Microbes.Params;
using UnityEngine;
using static UnityEngine.Random;
using static UnityEngine.Time;

namespace TheSuperShell.Microbes
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(FieldOfView))]
    public sealed class Microbe : MonoBehaviour
    {
        public event EventHandler DeathEvent;
        public event EventHandler<BirthEventArgs> BirthEvent;
        public class BirthEventArgs : EventArgs
        {
            public BrainStructure brainGenome;
            public MicrobeGenome genome;
            public Vector3 position;
            public float energyToPass;
            public float eggHatchTime;
        }

        [SerializeField] private Transform birthPosition;
        [SerializeField] private Mouth mouth;
        [SerializeField] private Transform rightMandibulaJoint;
        [SerializeField] private Transform leftMandibulaJoint;
        [SerializeField] private SpriteRenderer[] bodyPartsSprites;
        [SerializeField] private string[] inputNames;
        [SerializeField] private string[] outputNames;
        [Header("Debugging")]
        [SerializeField] private bool godMode = false;
        [SerializeField] private bool playerControlled = false;

        private float forward = 0f;
        private float backward = 0f;
        private float right = 0f;
        private float left = 0f;
        private float want_to_eat = 0f;
        private float want_to_give_birth = 0f;
        private float want_to_grab = 0f;
        private float tikTakInput = 1;
        private float restart_chronometer = 0;
        private float regen_health = 0f;
        private float chronometer = 0;
        private float want_to_attack = 0;
        private float grow = 0;

        private Animator animator;
        private Rigidbody2D rb;
        private FixedJoint2D fj;
        private FieldOfView fov;
        private Brain brain;
        private MicrobeParameters parameters;
        private MicrobeGenome genome;

        private float age;
        private float maturity;
        private float growthRate;
        private float movementCost;
        private float rotationCost;
        private float metabolismCost;
        private float agePenalty;
        private float growthCost;
        private float healthRegenerationAmount;
        private float sizeRadius;
        private float baselineHealth;
        private Resource energy;
        private Resource health;
        private bool grabed;
        private float biteCooldown;
        private float lifetime;

        public float Age { get => age; }
        public Resource Energy { get => energy; }
        public Resource Health { get => health; }
        public Color MicrobeColor { get => genome.color; }
        public Brain ShowBrain() => brain;
        public MicrobeGenome Genome { get => genome; }
        public string[] InputNames { get => inputNames; }
        public string[] OutputNames { get => outputNames; }
        public float MovementCost { get => movementCost; }
        public float RotationCost { get => rotationCost; }
        public float MovementWant 
        {
            get => (Mathf.Abs(forward - backward) * parameters.MovingCost +
                Mathf.Abs(right - left) * parameters.RotationCost) /
                (parameters.MovingCost + parameters.RotationCost);
        }
        public float MetabolismCost { get => metabolismCost; }
        public float AgePenalty { get => agePenalty; }
        public float GrowthCost { get => growthCost; }
        public float GrowthWant { get => grow; }
        public float HealthRegenerationAmount { get => healthRegenerationAmount; }
        public float EggCost { get => baselineHealth * parameters.EnergyToHealthRatio * genome.birthCost + parameters.BaselineEggCost; }
        public float Maturity { get => maturity; }
        public float GrowthRate { get => growthRate; }
        public float Speed { get => rb.velocity.magnitude; }
        public float Size { get => sizeRadius * sizeRadius; }
        public float Mass { get => rb.mass; }
        public float Lifetime { get => lifetime; }

        public float GrowthFunction(float maturity) => genome.growthScale / (1 + genome.growthFactor * Mathf.Pow(maturity, genome.growthExponent));

        public void GenerateNew()
        {
            brain = new(inputNames.Length, outputNames.Length);
            genome = Parameters.CreateDefaultGenome(parameters);
            genome = Parameters.RandomParameter.Mutate(genome, parameters.BasetineMutationIntensity + genome.mutationIntensity);
            genome = Parameters.COLOR.Mutate(genome, parameters.BasetineMutationIntensity + genome.mutationIntensity);
            brain.Mutate(genome.mutationIntensity);
            baselineHealth = parameters.BaselineMaxHealth * genome.linearSize * genome.linearSize;
            SetEnergyToNewborn(50f);
        }

        public void Inherit(BrainStructure brainGenome, MicrobeGenome genome, float energyToPass)
        {
            brain = new(brainGenome);
            if (value < parameters.BaselineMutationProbability + genome.mutationProbability)
            {
                genome = Parameters.RandomParameter.Mutate(genome, parameters.BasetineMutationIntensity + genome.mutationIntensity);
                genome = Parameters.COLOR.Mutate(genome, parameters.BasetineMutationIntensity + genome.mutationIntensity);
            }
            if (value < genome.mutationProbability)
            {
                brain.Mutate(parameters.BasetineMutationIntensity + genome.mutationIntensity);
                genome = Parameters.COLOR.Mutate(genome, parameters.BasetineMutationIntensity + genome.mutationIntensity);
            }
            this.genome = genome;
            this.genome.generation += 1;
            SetEnergyToNewborn(energyToPass);
        }

        private void SetEnergyToNewborn(float energyToPass)
        {
            baselineHealth = parameters.BaselineMaxHealth * genome.linearSize * genome.linearSize;

            SetMaxHealth(Mathf.Clamp(energyToPass * baselineHealth / (genome.linearSize * genome.linearSize * parameters.EnergyDensity), 5, Mathf.Infinity));
            Mature();
            energy.SetResource(energy.MaxValue);
            health.SetResource(health.MaxValue);
            //Debug.Log("New microbe " + this + " was born", this);
        }

        private void Awake()
        {
            parameters = Resources.Load<MicrobeParameters>("Microbe Global Parameters");
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            fj = GetComponent<FixedJoint2D>();
            fj.enabled = false;
            fov = GetComponent<FieldOfView>();
            rb.drag = parameters.LinearDrag;
            rb.angularDrag = parameters.AngularDrag;
            health = new();
            energy = new();
        }

        private void Start()
        {
            if (playerControlled)
                GenerateNew();
            fov.StartSensing();
            transform.Find("body").GetComponent<SpriteRenderer>().color = genome.color;
            foreach (SpriteRenderer sprite in bodyPartsSprites)
                sprite.color = genome.color;
            StartCoroutine(TikTakTimer());
            lifetime = parameters.Lifetime * 60 * genome.linearSize;
        }

        private void Update()
        {
            if (playerControlled)
            {
                right = Input.GetKey(KeyCode.D) ? 1 : 0;
                left = Input.GetKey(KeyCode.A) ? 1 : 0;
                forward = Input.GetKey(KeyCode.W) ? 1 : 0;
                backward = Input.GetKey(KeyCode.S) ? 1 : 0;

                want_to_eat = Input.GetKey(KeyCode.Space) ? 1 : 0;
                want_to_give_birth = Input.GetKey(KeyCode.F) ? 1 : 0;
                want_to_grab = Input.GetKey(KeyCode.E) ? 1 : 0;

                sizeRadius += Input.GetKeyDown(KeyCode.U) ? 0.1f : 0;
                sizeRadius -= Input.GetKeyDown(KeyCode.J) ? 0.1f : 0;
                Mature();
            }
            else if (brain != null)
            {
                float[] inputs = new float[inputNames.Length];
                inputs[0] = 1f; // Constant
                inputs[1] = health.GetShare(); // Health
                inputs[2] = energy.GetShare(); // Energy
                inputs[3] = Mathf.Clamp01(rb.velocity.magnitude / 5f); // Speed
                inputs[4] = fov.ClosestFoodDistance / fov.Radius; // Distance to closest food in FOV
                inputs[5] = fov.ClosestFoodAngle / 180f; // Angle to closest food in FOV
                inputs[6] = Mathf.Clamp01(fov.AmountOfFoodSeen / 10f); // Amount of food in FOV
                inputs[7] = fov.ClosestMicrobeDistance / fov.Radius; // Distance to closest food in FOV
                inputs[8] = fov.ClosestMicrobeAngle / 180f; // Angle to closest food in FOV
                inputs[9] = Mathf.Clamp01(fov.AmountOfMicrobesSeen / 5f); // Amount of food in FOV
                inputs[10] = fov.ClosestMicrobeColor.r;
                inputs[11] = fov.ClosestMicrobeColor.g;
                inputs[12] = fov.ClosestMicrobeColor.b;
                inputs[13] = tikTakInput;
                inputs[14] = Mathf.Clamp01(maturity);
                inputs[15] = Mathf.Clamp01(age / (parameters.Lifetime * 60 * genome.linearSize));
                inputs[16] = chronometer;

                float[] nnOutputs = brain.MakeDecision(inputs);
                forward = nnOutputs[0]; // forward movement
                backward = nnOutputs[1]; // backward movement
                right = nnOutputs[2]; // right
                left = nnOutputs[3]; // left
                want_to_eat = nnOutputs[4]; // outputs[2]; // try eating
                want_to_give_birth = nnOutputs[5]; // try giving birth
                want_to_grab = nnOutputs[6]; // grab
                regen_health = nnOutputs[7]; // regen health
                restart_chronometer = nnOutputs[8];
                want_to_attack = nnOutputs[9];
                grow = nnOutputs[10];
            }

        }

        private void FixedUpdate()
        {
            UpdateParameters();

            float linearMovement = (forward - backward);
            animator.SetFloat("Speed", Mathf.Abs(linearMovement));
            if (linearMovement < 0) linearMovement *= parameters.BackwardsForcePenalty;
            rb.AddForce(linearMovement * parameters.MovementForce * genome.movementForcePercent * sizeRadius * health.GetShare() * fixedDeltaTime * transform.right, ForceMode2D.Force);
            rb.AddTorque((left - right) * parameters.RotationForce * genome.rotationForcePercent * sizeRadius * sizeRadius * health.GetShare() * fixedDeltaTime, ForceMode2D.Force);

            if (want_to_eat >= 0.5f && biteCooldown <= 0)
                TryEating();
            if (want_to_attack >= 0.5f && biteCooldown <= 0)
                TryAttacking();
            if (restart_chronometer >= 0.75f)
                chronometer = 0;
            if (want_to_give_birth >= 0.5f)
                TryGivingBirth();
            if (want_to_grab >= 0.5f)
                TryGrabbing();
            else
                TryDropping();

            if (!godMode)
            {
                ConsumeEnergy();
                if (health.Value <= 0)
                    Die();
                age += fixedDeltaTime;
            }
        }

        private void Mature()
        {
            maturity = health.MaxValue / baselineHealth;
            sizeRadius = Mathf.Sqrt(maturity) * genome.linearSize;
            transform.localScale = Vector3.one * sizeRadius;
            rb.mass = sizeRadius * sizeRadius * parameters.MassDensity + genome.defence * 100 * parameters.MassPerDefence;
        }

        private void SetMaxHealth(float value)
        {
            health.SetMaxAmount(value);
            energy.SetMaxAmountWithoutValueChange(health.MaxValue * parameters.EnergyToHealthRatio);
        }

        private void UpdateParameters()
        {
            Mature();
            growthRate = GrowthFunction(maturity);
            growthCost = GrowthRate * grow * parameters.EnergyDensity * genome.linearSize * genome.linearSize / baselineHealth / 4;
            if (energy.Value >= growthCost * fixedDeltaTime)
            {
                SetMaxHealth(health.MaxValue + growthRate * grow * fixedDeltaTime);
            }
            else
                growthCost = 0;
            movementCost = genome.movementForcePercent * sizeRadius * parameters.MovingCost * Mathf.Abs(forward - backward);
            rotationCost = genome.rotationForcePercent * sizeRadius * parameters.RotationCost * Mathf.Abs(right - left);
            agePenalty = (age >= lifetime) ? Mathf.Exp(0.1f / 10f * (age - lifetime)) : 1;
            metabolismCost = sizeRadius * parameters.Metabolism * agePenalty;
            healthRegenerationAmount = genome.regenerationRate * regen_health * sizeRadius * sizeRadius;
            if (chronometer < 1)
                chronometer += fixedDeltaTime;
            if (biteCooldown > 0)
                biteCooldown -= fixedDeltaTime;
        }

        private void Die()
        {
            DeathEvent?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
        }

        private void ConsumeEnergy()
        {
            float energyBelowZero = energy.ConsumeResource(fixedDeltaTime * (movementCost + rotationCost + metabolismCost + growthCost));
            health.ConsumeResource(energyBelowZero);
            if (energyBelowZero > 0)
                energy.SetResource(0);

            if (health.Value < health.MaxValue)
            {
                float healthToRegen = fixedDeltaTime * HealthRegenerationAmount;
                if (healthToRegen <= energy.Value)
                {
                    energy.ConsumeResource(healthToRegen);
                    health.AddToResource(healthToRegen);
                }
            }
        }

        private void TryEating()
        {
            food.GenericFood food = mouth.GetFoodNearMouth();
            float sizeToEat = sizeRadius * parameters.BaselineMouthSize * genome.linearSize;
            if (food != null && energy.MaxValue - energy.Value >= food.SizeToEnergy(sizeToEat))
            {
                animator.Play("Biting");
                biteCooldown = parameters.BiteCooldown;
                float foodEnergy = food.Bite(food.SizeToEnergy(sizeToEat));
                energy.AddToResource(foodEnergy);
            }
        }

        public void TakeDamage(float amount)
        {
            float defence = genome.defence * parameters.MaxDamageReduction;
            amount -= amount * defence;
            if (health.ConsumeResource(amount) > 0)
                Die();
        }

        private void TryAttacking()
        {
            Microbe microbe = mouth.GetMicrobeNearMouth();
            float attackCost = parameters.AttackCost * sizeRadius;
            if (microbe != null && energy.Value >= attackCost)
            {
                animator.Play("Biting");
                energy.ConsumeResource(attackCost);
                biteCooldown = parameters.BiteCooldown;
                float damage = parameters.AttackDamage * sizeRadius * health.GetShare();
                microbe.TakeDamage(damage);
            }
        }

        private void TryGrabbing()
        {
            food.GenericFood food = mouth.GetFoodNearMouth();
            if (!grabed)
            {
                if (food != null)
                {
                    fj.enabled = true;
                    fj.connectedBody = food.FoodRigidBody;
                    grabed = true;
                }
            }
            else if (food == null)
                TryDropping();
        }

        private void TryDropping()
        {
            if (grabed)
            {
                fj.enabled = false;
                fj.connectedBody = null;
                grabed = false;
            }
        }

        private void TryGivingBirth()
        {
            if (Maturity < 1) return;
            float energyToGiveBirth = baselineHealth * parameters.EnergyToHealthRatio * genome.birthCost;
            if (energy.Value >= energyToGiveBirth)
            {
                energy.ConsumeResource(energyToGiveBirth);
                BirthEvent?.Invoke(this, new BirthEventArgs() 
                {
                    brainGenome = brain.GetBrainGenome(),
                    genome = genome,
                    energyToPass = energyToGiveBirth - parameters.BaselineEggCost * genome.linearSize,
                    position = birthPosition.position,
                    eggHatchTime = genome.hatchTime
                });
            }
        }

        private IEnumerator TikTakTimer()
        {
            while (true)
            {
                tikTakInput = (tikTakInput == 0) ? 1 : 0;
                yield return new WaitForSeconds(genome.tikTak);
            }
        }
    }
}
