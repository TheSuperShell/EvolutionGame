using System;
using UnityEngine;

namespace TheSuperShell.food
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class GenericFood : MonoBehaviour
    {
        public event EventHandler Destroyed;

        [Range(0.01f, 0.5f)]
        [SerializeField] private float emptySize;

        private float energy;
        private float size;
        private float energyDensity;
        private Rigidbody2D rb;

        public Rigidbody2D FoodRigidBody { get => rb; }

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public float EnergyToSize(float energy) => Mathf.Pow(energy / energyDensity, 0.5f) + emptySize;
        public float SizeToEnergy(float size) => size * size * energyDensity;

        public float Eat()
        {
            Destroyed?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
            return energy;
        }

        public float Bite(float biteEnergy)
        {
            if (biteEnergy > energy)
                return Eat();

            energy -= biteEnergy;
            size = EnergyToSize(energy);
            transform.localScale = Vector3.one * size;
            return biteEnergy;
        }

        public void Spawn(float energy, float density)
        {
            energyDensity = density;
            size = EnergyToSize(energy);
            this.energy = energy;
            transform.localScale = Vector3.one * size;
        }
    }
}
