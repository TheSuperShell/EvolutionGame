using System.Collections.Generic;
using UnityEngine;
using TheSuperShell.Utils;

namespace TheSuperShell.Microbes.Params
{

    public abstract class MutatableParameter
    {
        public readonly string Name;
        public readonly bool Show;

        private readonly float maxValue;
        private readonly float minValue;

        public MutatableParameter(MutatableParameterSettings settings)
        {
            Name = settings.Name;
            Show = settings.Show;
            maxValue = settings.MaxValue;
            minValue = settings.MinValue;
        }

        protected float ChangeValueWithNoramlDistribution(float intensity, float regularChange, float value)
        {
            return Mathf.Clamp(value + Functions.NormalDistribution(0, regularChange * intensity), minValue, maxValue);
        }
        public virtual string AsString(MicrobeGenome genome) => "";
        public abstract MicrobeGenome Mutate(MicrobeGenome genome, float intensity);
        public abstract MicrobeGenome SetDefaultValue(MicrobeGenome genome, MicrobeParameters parameters);
    }

    public class MutatableParameterSettings
    {
        public string Name { get; private set; } = "";
        public bool Show { get; private set; } = true;
        public float MaxValue { get; private set; } = 100;
        public float MinValue { get; private set; } = 0;

        public MutatableParameterSettings DoNotShow()
        {
            Show = false;
            return this;
        }

        public MutatableParameterSettings SetName(string name)
        {
            Name = name;
            return this;
        }

        public MutatableParameterSettings SetRange(float min, float max)
        {
            MinValue = min;
            MaxValue = max;
            return this;
        }
    }

    public static class Parameters
    {
        public readonly static List<MutatableParameter> ParameterList = new();

        public static MutatableParameter SPEED = RegisterParameter(new Speed(new MutatableParameterSettings().SetName("Force")));
        public static MutatableParameter ROTATION_SPEED = RegisterParameter(new RotationSpeed(new MutatableParameterSettings().SetName("Rotation Force")));
        public static MutatableParameter SIZE = RegisterParameter(new Size(new MutatableParameterSettings().SetName("Size")));
        public static MutatableParameter HATCH_TIME = RegisterParameter(new HatchTime(new MutatableParameterSettings().SetName("Hatch Time")));
        public static MutatableParameter BIDTH_COST = RegisterParameter(new BirthCost(new MutatableParameterSettings().SetName("Birth Cost")));
        public static MutatableParameter MUTATION_PROBABILITY = RegisterParameter(new MutationProbability(new MutatableParameterSettings().SetName("Mutation P.")));
        public static MutatableParameter MUTAION_INTENSITY = RegisterParameter(new MutationIntensity(new MutatableParameterSettings().SetName("Mutation I.")));
        public static MutatableParameter REGENERATION_RATE = RegisterParameter(new RegenerationRate(new MutatableParameterSettings().SetName("Regen. Rate")));
        public static MutatableParameter TIK_TAK = RegisterParameter(new TikTak(new MutatableParameterSettings().SetName("TikTak")));
        public static MutatableParameter COLOR = RegisterParameter(new MutateColor(new MutatableParameterSettings().SetName("Color")));
        public static MutatableParameter GROWTH_SCALE = RegisterParameter(new GrowthScale(new MutatableParameterSettings().SetRange(0, 20f).SetName("Growth Scale")));
        public static MutatableParameter GROWTH_FACTOR = RegisterParameter(new GrowthFactor(new MutatableParameterSettings().SetRange(0, 10).SetName("Growth Factor")));
        public static MutatableParameter GROWTH_EXPONENT = RegisterParameter(new GrowthExponent(new MutatableParameterSettings().SetRange(1, 10).SetName("Growth Exponent")));
        public static MutatableParameter DEFENCE = RegisterParameter(new Defence(new MutatableParameterSettings().SetRange(0, 1).SetName("Defence")));

        public static MutatableParameter RegisterParameter(MutatableParameter mutation)
        {
            ParameterList.Add(mutation);
            return mutation;
        }

        public static MutatableParameter RandomParameter 
        {
            get
            {
                while (true)
                {
                    int i = Random.Range(0, ParameterList.Count - 1);
                    if (ParameterList[i] != COLOR)
                        return ParameterList[i];
                }
            }
        }

        public static MicrobeGenome CreateDefaultGenome(MicrobeParameters microbeParameters)
        {
            MicrobeGenome newGenome = new();
            foreach (MutatableParameter parameter in ParameterList)
            {
                newGenome = parameter.SetDefaultValue(newGenome, microbeParameters);
            }
            return newGenome;
        }
    }
}
