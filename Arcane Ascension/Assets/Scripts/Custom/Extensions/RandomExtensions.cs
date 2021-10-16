using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Class with extension methods for System Random.
/// </summary>
public static class RandomExtensions
{
    /// <summary>
    /// Applies weight probabilities in a list of weights.
    /// </summary>
    /// <param name="random">This random.</param>
    /// <param name="weights">List with weights.</param>
    /// <returns>Returns a random number.</returns>
    public static int RandomWeight(this Random random, IList<int> weights)
    {
        float rnd = (float)(random.NextDouble() * weights.Sum());
        int randomNum = 0;

        for (int i = 0; i < weights.Count(); i++)
        {
            rnd -= weights[i];
            if (rnd < 0)
            {
                randomNum = i;
                break;
            }
        }
        return randomNum;
    }
}
