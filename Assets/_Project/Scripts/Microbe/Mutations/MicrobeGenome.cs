using UnityEngine;

namespace TheSuperShell.Microbes.Params
{
    public struct MicrobeGenome
    {
        public int generation;
        public float linearSize;
        public float movementForcePercent;
        public float rotationForcePercent;
        public float hatchTime;
        public float birthCost;
        public float tikTak;
        public float mutationProbability;
        public float mutationIntensity;
        public float regenerationRate;
        public Color color;
        public float growthScale;
        public float growthFactor;
        public float growthExponent;
        public float defence;
    }
}
