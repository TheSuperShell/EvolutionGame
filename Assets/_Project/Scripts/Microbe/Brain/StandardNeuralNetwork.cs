using System;
using TheSuperShell.Utils;
using UnityEngine;
using static UnityEngine.Random;

namespace TheSuperShell.Brains
{
    public class StandardNeuralNetwork
    {
        private int[] shape;
        private float[][,] weights;
        private float[][] biases;
        private IActivatable activationFunction;
        private float[][] values;

        public float[][] Values { get => values; }
        public float[][,] Weights { get => weights; }
        
        public StandardNeuralNetwork(int[] shape, IActivatable activation)
        {
            this.shape = shape;
            activationFunction = activation;
            weights = new float[shape.Length - 1][,];
            biases = new float[shape.Length - 1][];
            values = new float[shape.Length][];
            for (int i = 0; i < shape.Length - 1; i++)
            {
                weights[i] = new float[shape[i+1], shape[i]];
                for (int x = 0; x < shape[i+1]; x++)
                    for (int y = 0; y < shape[i]; y++) 
                        weights[i][x, y] = Functions.NormalDistribution();
                if (i != shape.Length - 1)
                {
                    biases[i] = new float[shape[i + 1]];
                    for (int j = 0; j < biases[i].Length; j++)
                        biases[i][j] = Functions.NormalDistribution();
                }  
            }
        }

        public StandardNeuralNetwork(BrainStructure weightsAndBiases)
        {
            shape = weightsAndBiases.shape;
            activationFunction = weightsAndBiases.activationFunction;
            weights = new float[shape.Length - 1][,];
            biases = new float[shape.Length - 1][];
            values = new float[shape.Length][];
            for (int i = 0; i < shape.Length - 1; i++)
            {
                weights[i] = new float[shape[i + 1], shape[i]];
                for (int x = 0; x < shape[i + 1]; x++)
                    for (int y = 0; y < shape[i]; y++)
                        weights[i][x, y] = weightsAndBiases.weights[i][x, y];
                if (i != shape.Length - 1)
                {
                    biases[i] = new float[shape[i + 1]];
                    for (int j = 0; j < biases[i].Length; j++)
                        biases[i][j] = weightsAndBiases.biases[i][j];
                }
            }
        }

        private float[] Multiply(float[] a, float[,] b)
        {
            if (a.Length != b.GetLength(1))
                throw new InvalidOperationException("Length of a does not equal Length of b");
            float[] result = new float[b.GetLength(0)];
            for (int i = 0; i < b.GetLength(0); i++) {
                result[i] = 0;
                for (int j = 0; j < a.Length; j++)
                    result[i] += a[j] * b[i, j];
            }
            return result;
        }

        private float[] Add(float[] a, float[] b)
        {
            if (a.Length != b.Length)
                throw new InvalidOperationException("Length of a does not equal Length of b");

            float[] result = new float[a.Length];
            for (int i = 0; i < a.Length; i++)
                result[i] = a[i] + b[i];
            return result;
        }

        private float[] Activate(float[] a)
        {
            for (int i = 0; i < a.Length; i++)
                a[i] = activationFunction.Activate(a[i]);
            return a;
        }

        public void RandomMutation(float randomness)
        {
            int layer = Range(0, shape.Length - 2);
            int weight = Range(0, 1);
            if (true)
            {
                int x = Range(0, weights[layer].GetLength(0));
                int y = Range(0, weights[layer].GetLength(1));
                weights[layer][x, y] += Functions.NormalDistribution(0, randomness);
            }
            else
            {
                int x = Range(0, biases[layer].Length);
                biases[layer][x] += Functions.NormalDistribution(0, randomness);
            }
        }

        public float[] FeedForward(float[] inputs)
        {
            if (inputs.Length != shape[0])
                throw new InvalidOperationException("Invalid number of inputs");

            values[0] = inputs;
            for (int i = 0; i < shape.Length - 1; i++)
            {
                inputs = Activate(Multiply(inputs, weights[i]));
                values[i + 1] = inputs;
            }
            return inputs;
        }

        public BrainStructure GetBrainGenome() => new(shape, weights, biases, activationFunction);
    }

    public struct BrainStructure
    {
        public int[] shape;
        public float[][,] weights;
        public float[][] biases;
        public IActivatable activationFunction;

        public BrainStructure(int[] _shape, float[][,] _weights, float[][] _biases, IActivatable _activationFunction)
        {
            shape = _shape;
            weights = _weights;
            biases = _biases;
            activationFunction = _activationFunction;
        }
    }
}
