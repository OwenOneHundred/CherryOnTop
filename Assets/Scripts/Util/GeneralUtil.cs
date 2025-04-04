using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class GeneralUtil
{
    /// <summary>
    /// Returns index of random float from given list, but with proportionally greater likelihood of returning bigger floats.
    /// </summary>
    /// <returns>Index of chosen float.</returns>
    public static int RandomWeighted(List<float> values)
    {
        float total = values.Sum(x => x);
        float randomValue = UnityEngine.Random.Range(0f, total);
        float tally = 0;
        for (int i = 0; i < values.Count; i++)
        {
            tally += values[i];
            if (tally > randomValue)
            {
                return i;
            }
        }
        return values.Count - 1;
    }

    public static float RandomAccordingToNormalDistribution(float mean, float standard_deviation)
    {
        return mean + NextGaussian() * standard_deviation;

        static float NextGaussian()
        {
            float v1, v2, s;
            do
            {
                v1 = 2.0f * Random.Range(0f, 1f) - 1.0f;
                v2 = 2.0f * Random.Range(0f, 1f) - 1.0f;
                s = v1 * v1 + v2 * v2;
            } while (s >= 1.0f || s == 0f);
            s = Mathf.Sqrt((-2.0f * Mathf.Log(s)) / s);

            return v1 * s;
        }
    }
}
