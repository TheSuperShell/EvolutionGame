using System.Collections.Generic;
using static UnityEngine.Mathf;
using static UnityEngine.Random;

namespace TheSuperShell.Brains
{
    public interface IActivatable
    {
        public abstract float Activate(float value);
    }

    public class BinaryActivation : IActivatable
    {
        public float Activate(float value) => (value < 0) ? -1 : 1;
    }

    public class LinearActivation : IActivatable
    {
        public float Activate(float value) => value;
    }

    public class SigmoidActivation : IActivatable
    {
        public float Activate(float value) => 1f / (Exp(-value) + 1f);
    }

    public class HyperbolicActivation : IActivatable
    {
        public float Activate(float value) => (Exp(value) - Exp(-value)) / (Exp(value) + Exp(-value));
    }

    public class ReLUActivation : IActivatable
    {
        public float Activate(float value) => Max(0, value);
    }

    public static class ActivationFunctions
    {
        private static readonly List<IActivatable> activationFunctionList = new();

        public static IActivatable Binary = RegisterActivationFunction(new BinaryActivation());
        public static IActivatable Linear = RegisterActivationFunction(new LinearActivation());
        public static IActivatable Sigmoid = RegisterActivationFunction(new SigmoidActivation());
        public static IActivatable Hyperbolic = RegisterActivationFunction(new HyperbolicActivation());
        public static IActivatable ReLU = RegisterActivationFunction(new ReLUActivation());

        public static IActivatable GetUniform()
        {
            int index =  Range(0, activationFunctionList.Count - 1);
            return activationFunctionList[index];
        }

        private static IActivatable RegisterActivationFunction(IActivatable function)
        {
            activationFunctionList.Add(function);
            return function;
        }
    }
}
