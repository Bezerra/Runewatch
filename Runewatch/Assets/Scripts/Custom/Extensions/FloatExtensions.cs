using UnityEngine;
using System;

namespace ExtensionMethods
{
    /// <summary>
    /// Class responsible for adding extension methods to float.
    /// </summary>
    public static class FloatExtensions
    {
        /// <summary>
        /// Checks if a float is similiar to another with a determined
        /// compensation.
        /// </summary>
        /// <param name="thisFloat">This float.</param>
        /// <param name="otherFloat">Float to compare.</param>
        /// <param name="compensation">Compensation.</param>
        /// <returns>True if it is similiar.</returns>
        public static bool Similiar(this float thisFloat,
            float otherFloat, float compensation = 0.01f)
        {
            float finalPositive = otherFloat + compensation;
            float finalNegative = otherFloat - compensation;

            if (thisFloat < finalPositive &&
                thisFloat > finalNegative)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Lerps an output value depending on the input values received.
        /// For ex: Remap(1, 10, Color.Blue, Coor.Yellow, someValue) =
        /// Below 1 = blue, lerps blue to yellow from 1 to 10, Above 10 = yellow.
        /// </summary>
        /// <param name="thisFloat">This float.</param>
        /// <param name="inputMin">Minimum input value.</param>
        /// <param name="inputMax">Maximum input value.</param>
        /// <param name="outputMin">Minimum output value to minimum input value.</param>
        /// <param name="outputMax">Maximum output value to maximum input value.</param>
        /// <param name="value">Value to measure.</param>
        /// <returns>Returns a float value between outputMin and outputMax</returns>
        public static float Remap(this float thisFloat, float inputMin,
            float inputMax, float outputMin, float outputMax, float value)
        {
            float t = Mathf.InverseLerp(inputMin, inputMax, value);
            return Mathf.Lerp(outputMin, outputMax, t);
        }

        /// <summary>
        /// Checks a percentage against 100% percentage.
        /// Example: Enemy drops an item with a 10% chance; itemDrop.PercentageCheck(random),
        /// will execute a probability to know if that 10% chance was met.
        /// </summary>
        /// <param name="desiredPercentage">Percentage that's being analysed.</param>
        /// <param name="random">Random instance.</param>
        /// <returns>True if desiredPercentage was met.</returns>
        public static bool PercentageCheck(this float desiredPercentage, System.Random random)
        {
            float randomChance = random.Next(0, 101);
            if (randomChance < desiredPercentage)
                return true;
            return false;
        }
    }
}
