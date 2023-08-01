using TheSuperShell.Brains;
using UnityEngine;

namespace TheSuperShell
{
    public class Brain
    {
        private int amountOfInputs;
        private int amountOfOutputs;
        private StandardNeuralNetwork nn;
        public Brain(int inputs, int outputs)
        {
            amountOfInputs = inputs;
            amountOfOutputs = outputs;
            int[] nnShape = new int[2] { inputs, outputs };
            nn = new(nnShape, ActivationFunctions.Sigmoid);
        }
        
        public Brain(BrainStructure genome)
        {
            amountOfInputs = genome.shape[0];
            amountOfOutputs = genome.shape[^1];
            nn = new(genome);
        }

        public float[] MakeDecision(float[] inputs)
        {
            if (inputs.Length != amountOfInputs)
                throw new System.InvalidOperationException("Wrong number of inputs!");
            float[] outputs = nn.FeedForward(inputs);
            if (outputs.Length != amountOfOutputs)
                throw new System.InvalidOperationException("Wrong number of outputs!");
            return outputs;
        }

        public void Mutate(float intensity) 
        {
            nn.RandomMutation(intensity);
        }

        public float[][] NNValues { get => nn.Values; }
        public float[][,] NNWeights { get => nn.Weights; }

        public BrainStructure GetBrainGenome() => nn.GetBrainGenome();
    }
}
