using UnityEngine;

namespace ExtensionMethods
{
    /// <summary>
    /// Class responsible for adding extension methods to Color.
    /// </summary>
    public static class ColorExtensions 
    {
        /// <summary>
        /// Lerps an output value depending on the input values received.
        /// For ex: Remap(1, 10, Color.Blue, Coor.Yellow, someValue) =
        /// Below 1 = blue, lerps blue to yellow from 1 to 10, Above 10 = yellow.
        /// </summary>
        /// <param name="thisColor">This float.</param>
        /// <param name="inputMin">Minimum input value.</param>
        /// <param name="inputMax">Maximum input value.</param>
        /// <param name="outputMin">Minimum output value to minimum input value.</param>
        /// <param name="outputMax">Maximum output value to maximum input value.</param>
        /// <param name="value">Value to measure.</param>
        /// <returns>Returns a color between outputMin and outputMax</returns>
        public static Color Remap(this Color thisColor, float inputMin,
            float inputMax, Color outputMin, Color outputMax, float value)
        {
            float t = Mathf.InverseLerp(inputMin, inputMax, value);
            return Color.Lerp(outputMin, outputMax, t);
        }
    }
}
