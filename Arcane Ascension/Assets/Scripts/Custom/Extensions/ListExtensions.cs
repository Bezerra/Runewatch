using System;
using System.Collections.Generic;

/// <summary>
/// Class with extension methods for List.
/// </summary>
public static class ListExtensions
{
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
