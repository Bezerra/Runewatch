using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class RandomExtensions
{
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
