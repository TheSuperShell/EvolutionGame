using UnityEngine;

namespace TheSuperShell.Microbes
{
    [CreateAssetMenu(fileName = "Microbe Global Parameters", menuName = "ScriptableObjects/MicrobeGlobalParametersScriptableObject", order = 1)]
    public class MicrobeParameters : ScriptableObject
    {
        [Header("Global Parameters")]
        [Range(1f, 1000f)]
        public float BaselineMaxHealth;
        [Range(1f, 10f)]
        public float EnergyToHealthRatio;
        [Range(0f, 3000f)]
        public float MovementForce;
        [Range(0f, 3000f)]
        public float RotationForce;
        [Range(0, 1)]
        public float BackwardsForcePenalty;
        [Range(0f, 200f)]
        public float BaselineEggCost;
        [Range(0f, 100f)]
        public float MovingCost;
        [Range(0f, 100f)]
        public float RotationCost;
        [Range(0f, 100f)]
        public float Metabolism;
        [Range(0f, 5f)]
        public float DamageFromOvereating;
        [Range(0f, 1f)]
        public float BaselineMouthSize;
        [Range(0, 0.5f)]
        public float BaselineMutationProbability;
        [Range(0, 0.5f)]
        public float BasetineMutationIntensity;
        [Range(1, 5)]
        public float Lifetime;
        [Range(0, 1)]
        public float BaseMeatSize;
        [Range(0, 10)]
        public float BiteCooldown;
        [Range(0, 100)]
        public float AttackDamage;
        [Range(0, 100)]
        public float AttackCost;
        [Range(0, 1)]
        public float MaxDamageReduction;
        [Range(0, 10)]
        public float MassPerDefence;

        [Header("Physics params")]
        [Range(0f, 10f)]
        public float LinearDrag;
        [Range(0f, 10f)]
        public float AngularDrag;
        [Range(0.01f, 10f)]
        public float MassDensity;
        [Range(0.01f, 1000f)]
        public float EnergyDensity;

        [Header("Parameters might change through mutations")]
        [Range(0.2f, 1f)]
        public float MutationProbability;
        [Range(0.05f, 5f)]
        public float MutationIntensity;
        [Range(0f, 1f)]
        [Tooltip("Share of energy that can pass to a newborn")]
        public float BirthCost;
        [Range(0f, 10f)]
        public float Speed;
        [Range(0f, 10f)]
        public float RotationSpeed;
        [Range(0.01f, 10f)]
        public float Size;
        [Range(0f, 100f)]
        public float HatchTime;
        [Range(0f, 100f)]
        public float TikTak;
        [Range(0f, 10f)]
        public float RegenerationRate;
        public Color Color;
        [Range(0f, 10f)]
        public float GrowthScale;
        [Range(0f, 10f)]
        public float GrowthFactor;
        [Range(1f, 10f)]
        public float GrowthExponent;
        [Range(0, 1)]
        public float Defence;
    }
}
