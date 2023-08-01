using TheSuperShell.Brains;
using TheSuperShell.Microbes.Params;
using UnityEngine;
using System;

namespace TheSuperShell.Eggs
{
    public class Egg : MonoBehaviour
    {
        public event EventHandler<HatchEventArgs> HatchEvent;
        public class HatchEventArgs : EventArgs
        {
            public float energy;
            public BrainStructure brain;
            public MicrobeGenome genes;
            public Vector3 position;
        }

        [SerializeField] private float energyPassRate;
        private float hatchTime;
        private float energy;
        private float energyToPass;
        private BrainStructure brainGenome;
        private MicrobeGenome genome;
        private float size;

        public void SetUp(BrainStructure brain, MicrobeGenome genes, float energyFromParent, float hatchTime)
        {
            energy = energyFromParent;
            brainGenome = brain;
            genome = genes;
            this.hatchTime = hatchTime;
            size = genes.linearSize * .4f;
            transform.localScale = Vector3.one * size;
        }

        private void FixedUpdate()
        {
            hatchTime -= Time.fixedDeltaTime;
            if (hatchTime <= 0)
            {
                HatchEvent?.Invoke(this, new HatchEventArgs() { energy = energyToPass, brain = brainGenome, genes = genome, position = transform.position });
                Destroy(gameObject);
            }
            if (energy > 0) 
            {
                energyToPass += energyPassRate * size * Time.fixedDeltaTime;
                energy -= energyPassRate * size * Time.fixedDeltaTime;
            }
        }
    }
}
