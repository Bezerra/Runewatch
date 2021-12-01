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

    /// <summary>
    /// Applies weight probabilities in a list of weights.
    /// </summary>
    /// <param name="random">This random.</param>
    /// <param name="weights">List with weights.</param>
    /// <returns>Returns a random number.</returns>
    public static float RandomWeight(this Random random, IList<float> weights)
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

    /// <summary>
    /// Shuffles a list with a random instance.
    /// </summary>
    /// <typeparam name="T">Type of the list to shuffle.</typeparam>
    /// <param name="list">IList to shuffle.</param>
    /// <param name="random">Random instance.</param>
    public static void Shuffle<T>(this IList<T> list, Random random)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
