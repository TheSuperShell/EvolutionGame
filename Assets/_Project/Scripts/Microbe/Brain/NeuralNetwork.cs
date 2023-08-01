using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheSuperShell.Brains
{
    public class NeuralNetwork
    {
        private uint[] input_nodes;
        private uint[] output_nodes;
        private Node[] hidden_nodes;
        private float[] values;

        private NeuralNetwork(uint[] inputs, uint[] outputs, Node[] nodes)
        {
            input_nodes = inputs;
            output_nodes = outputs;
            hidden_nodes = nodes;
            values = new float[inputs.Length + nodes.Length];
            for (int i = 0; i < values.Length; i++)
                values[i] = 0;
        }
        

        private struct Node
        {
            public uint Id { get; private set; }
            public IActivatable ActivationFunction { get; private set; }
            public float Bias { get; private set; }
            public Link[] Inputs { get; private set; }

            public Node(uint id, IActivatable activationFunction, float bias, Link[] links)
            {
                Id = id;
                ActivationFunction = activationFunction;
                Bias = bias;
                Inputs = links;
            }
        }

        private struct Link
        {
            public uint InputId { get; private set; }
            public float Weight { get; private set; }

            public Link(uint input, float weight)
            {
                InputId = input;
                Weight = weight;
            }
        }

        public float[] FeedForward(float[] inputs)
        {
            if (inputs.Length != input_nodes.Length)
                throw new System.InvalidOperationException("Expected " + input_nodes.Length.ToString() + " inputs, got " + inputs.Length.ToString());

            foreach (uint id in input_nodes)
                values[id] = inputs[id];

            foreach (Node node in hidden_nodes)
            {
                float value = 0;
                foreach (Link link in node.Inputs)
                    value += link.Weight * values[link.InputId];
                values[node.Id] = node.ActivationFunction.Activate(value + node.Bias);
            }

            float[] output = new float[output_nodes.Length];
            for (int i = 0; i < output_nodes.Length; i++)
                output[i] = values[output_nodes[i]];

            return output;
        }

        public static NeuralNetwork Create(GenomeConfig config, Genome genome)
        {
            Node[] nodes = new Node[10];
            return new NeuralNetwork(config.InputIds, config.OutputIds, nodes);
        }
    }
}
