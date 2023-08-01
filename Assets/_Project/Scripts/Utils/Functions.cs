using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheSuperShell.Utils
{
    public static class Functions
    {
        public static float NormalDistribution()
        {
            float u, v, S;
            do
            {
                u = 2f * Random.value - 1f;
                v = 2f * Random.value - 1f;
                S = u * u + v * v;
            }
            while (S >= 1f);
            float fac = Mathf.Sqrt(-2f * Mathf.Log(S) / S);
            return fac * u;
        }

        public static float NormalDistribution(float mean, float std)
        {
            return mean + NormalDistribution() * std;
        }
    }
}
