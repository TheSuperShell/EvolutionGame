using UnityEngine;
using TheSuperShell.Utils;

namespace TheSuperShell.Microbes.Params
{
    public class Speed : MutatableParameter
    {
        public Speed(MutatableParameterSettings settings) : base(settings) { }

        public override string AsString(MicrobeGenome genome)
        {
            return genome.movementForcePercent.ToString("P0");
        }

        public override MicrobeGenome Mutate(MicrobeGenome genome, float intensity)
        {
            genome.movementForcePercent = Mathf.Clamp(genome.movementForcePercent + Functions.NormalDistribution(0, 0.1f * intensity), 0.1f, 10f);
            return genome;
        }

        public override MicrobeGenome SetDefaultValue(MicrobeGenome genome, MicrobeParameters parameters)
        {
            genome.movementForcePercent = parameters.Speed;
            return genome;
        }
    }

    public class Size : MutatableParameter
    {
        public Size(MutatableParameterSettings settings) : base(settings) { }

        public override string AsString(MicrobeGenome genome)
        {
            return genome.linearSize.ToString("P0");
        }

        public override MicrobeGenome Mutate(MicrobeGenome genome, float intensity)
        {
            genome.linearSize = Mathf.Clamp(genome.linearSize + Functions.NormalDistribution(0, intensity * 0.2f), 0.6f, 5f);
            return genome;
        }

        public override MicrobeGenome SetDefaultValue(MicrobeGenome genome, MicrobeParameters parameters)
        {
            genome.linearSize = parameters.Size;
            return genome;
        }
    }

    public class RotationSpeed : MutatableParameter
    {
        public RotationSpeed(MutatableParameterSettings settings) : base(settings) { }

        public override string AsString(MicrobeGenome genome)
        {
            return genome.rotationForcePercent.ToString("P0");
        }

        public override MicrobeGenome Mutate(MicrobeGenome genome, float intensity)
        {
            genome.rotationForcePercent = Mathf.Clamp(genome.rotationForcePercent + Functions.NormalDistribution(0, intensity * 0.1f), 0.1f, 10f);
            return genome;
        }

        public override MicrobeGenome SetDefaultValue(MicrobeGenome genome, MicrobeParameters parameters)
        {
            genome.rotationForcePercent = parameters.RotationSpeed;
            return genome;
        }
    }

    public class HatchTime : MutatableParameter
    {
        public HatchTime(MutatableParameterSettings settings) : base(settings) { }

        public override string AsString(MicrobeGenome genome)
        {
            return genome.hatchTime.ToString("F2") + " s";
        }

        public override MicrobeGenome Mutate(MicrobeGenome genome, float intensity)
        {
            genome.hatchTime = Mathf.Clamp(genome.hatchTime + Functions.NormalDistribution(0, 3 * intensity), 1f, 50f);
            return genome;
        }

        public override MicrobeGenome SetDefaultValue(MicrobeGenome genome, MicrobeParameters parameters)
        {
            genome.hatchTime = parameters.HatchTime;
            return genome;
        }
    }
    public class BirthCost : MutatableParameter
    {
        public BirthCost(MutatableParameterSettings settings) : base(settings) { }

        public override string AsString(MicrobeGenome genome)
        {
            return genome.birthCost.ToString("P0");
        }

        public override MicrobeGenome Mutate(MicrobeGenome genome, float intensity)
        {
            genome.birthCost = Mathf.Clamp(genome.birthCost + Functions.NormalDistribution(0, 0.1f * intensity), 0.05f, 1f);
            return genome;
        }

        public override MicrobeGenome SetDefaultValue(MicrobeGenome genome, MicrobeParameters parameters)
        {
            genome.birthCost = parameters.BirthCost;
            return genome;
        }
    }

    public class MutationProbability : MutatableParameter
    {
        public MutationProbability(MutatableParameterSettings settings) : base(settings) { }

        public override string AsString(MicrobeGenome genome)
        {
            return genome.mutationProbability.ToString("P1");
        }

        public override MicrobeGenome Mutate(MicrobeGenome genome, float intensity)
        {
            genome.mutationProbability = Mathf.Clamp(genome.mutationProbability + Functions.NormalDistribution(0, 0.1f * intensity), 0f, 0.75f);
            return genome;
        }

        public override MicrobeGenome SetDefaultValue(MicrobeGenome genome, MicrobeParameters parameters)
        {
            genome.mutationProbability = parameters.MutationProbability;
            return genome;
        }
    }

    public class MutationIntensity : MutatableParameter
    {
        public MutationIntensity(MutatableParameterSettings settings) : base(settings) { }

        public override string AsString(MicrobeGenome genome)
        {
            return genome.mutationIntensity.ToString("P1");
        }

        public override MicrobeGenome Mutate(MicrobeGenome genome, float intensity)
        {
            genome.mutationIntensity = Mathf.Clamp(genome.mutationIntensity + Functions.NormalDistribution(0, 0.1f * intensity), 0.05f, 5f);
            return genome;
        }

        public override MicrobeGenome SetDefaultValue(MicrobeGenome genome, MicrobeParameters parameters)
        {
            genome.mutationIntensity = parameters.MutationIntensity;
            return genome;
        }
    }

    public class RegenerationRate : MutatableParameter
    {
        public RegenerationRate(MutatableParameterSettings settings) : base(settings) { }

        public override string AsString(MicrobeGenome genome)
        {
            return genome.regenerationRate.ToString("F1") + " hp/s";
        }

        public override MicrobeGenome Mutate(MicrobeGenome genome, float intensity)
        {
            genome.regenerationRate = Mathf.Clamp(genome.regenerationRate + Functions.NormalDistribution(0, 1f * intensity), 0f, 50f);
            return genome;
        }

        public override MicrobeGenome SetDefaultValue(MicrobeGenome genome, MicrobeParameters parameters)
        {
            genome.regenerationRate = parameters.RegenerationRate;
            return genome;
        }
    }

    public class TikTak : MutatableParameter
    {
        public TikTak(MutatableParameterSettings settings) : base(settings) { }

        public override string AsString(MicrobeGenome genome)
        {
            return genome.tikTak.ToString("F1") + " s";
        }

        public override MicrobeGenome Mutate(MicrobeGenome genome, float intensity)
        {
            genome.tikTak = Mathf.Clamp(genome.tikTak + Functions.NormalDistribution(0, 0.3f * intensity), 0f, 10f);
            return genome;
        }

        public override MicrobeGenome SetDefaultValue(MicrobeGenome genome, MicrobeParameters parameters)
        {
            genome.tikTak = parameters.TikTak;
            return genome;
        }
    }

    public class MutateColor : MutatableParameter
    {
        public MutateColor(MutatableParameterSettings settings) : base(settings) { }

        public override string AsString(MicrobeGenome genome)
        {
            return "[" + genome.color.r.ToString("F2") + ", " + genome.color.g.ToString("F2") + ", " + genome.color.b.ToString("F2") + "]";
        }

        public override MicrobeGenome Mutate(MicrobeGenome genome, float intensity)
        {
            float random = Random.value;
            if (random < 0.33f)
                genome.color.r = ChangeBasicColor(genome.color.r, intensity);
            else if (random < 0.66f)
                genome.color.g = ChangeBasicColor(genome.color.g, intensity);
            else
                genome.color.b = ChangeBasicColor(genome.color.b, intensity);
            return genome;
        }

        public override MicrobeGenome SetDefaultValue(MicrobeGenome genome, MicrobeParameters parameters)
        {
            genome.color = parameters.Color;
            return genome;
        }

        private float ChangeBasicColor(float oldRGB, float intensity) => Mathf.Clamp(oldRGB + Functions.NormalDistribution(0, 0.25f * intensity), 0.3f, 1);
    }

    public class GrowthScale : MutatableParameter
    {
        public GrowthScale(MutatableParameterSettings settings) : base(settings) { }

        public override string AsString(MicrobeGenome genome)
        {
            return genome.growthScale.ToString("F2");
        }

        public override MicrobeGenome Mutate(MicrobeGenome genome, float intensity)
        {
            genome.growthScale = ChangeValueWithNoramlDistribution(intensity, 0.2f, genome.growthScale);
            return genome;
        }

        public override MicrobeGenome SetDefaultValue(MicrobeGenome genome, MicrobeParameters parameters)
        {
            genome.growthScale = parameters.GrowthScale;
            return genome;
        }
    }
    public class GrowthFactor : MutatableParameter
    {
        public GrowthFactor(MutatableParameterSettings settings) : base(settings) { }

        public override string AsString(MicrobeGenome genome)
        {
            return genome.growthFactor.ToString("F2");
        }

        public override MicrobeGenome Mutate(MicrobeGenome genome, float intensity)
        {
            genome.growthFactor = ChangeValueWithNoramlDistribution(intensity, 0.5f, genome.growthFactor);
            return genome;
        }

        public override MicrobeGenome SetDefaultValue(MicrobeGenome genome, MicrobeParameters parameters)
        {
            genome.growthFactor = parameters.GrowthFactor;
            return genome;
        }
    }
    public class GrowthExponent : MutatableParameter
    {
        public override string AsString(MicrobeGenome genome)
        {
            return genome.growthExponent.ToString("F2");
        }

        public GrowthExponent(MutatableParameterSettings settings) : base(settings) { }

        public override MicrobeGenome Mutate(MicrobeGenome genome, float intensity)
        {
            genome.growthExponent = ChangeValueWithNoramlDistribution(intensity, 0.5f, genome.growthExponent);
            return genome;
        }

        public override MicrobeGenome SetDefaultValue(MicrobeGenome genome, MicrobeParameters parameters)
        {
            genome.growthExponent = parameters.GrowthExponent;
            return genome;
        }
    }

    public class Defence : MutatableParameter
    {

        public override string AsString(MicrobeGenome genome)
        {
            return (genome.defence * 100).ToString("F1");
        }
        public Defence(MutatableParameterSettings settings) : base(settings) { }
        public override MicrobeGenome Mutate(MicrobeGenome genome, float intensity)
        {
            genome.defence = ChangeValueWithNoramlDistribution(intensity, 0.1f, genome.defence);
            return genome;
        }

        public override MicrobeGenome SetDefaultValue(MicrobeGenome genome, MicrobeParameters parameters)
        {
            genome.defence = parameters.Defence;
            return genome;
        }
    }
}
