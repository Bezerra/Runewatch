using UnityEngine;

namespace ExtensionMethods
{
    /// <summary>
    /// Class responsible for adding extension methods to vector2.
    /// </summary>
    public static class Vector2Extensions
    {
        public static float Minimum(this Vector2 thisVector2)
        {
            return thisVector2.x;
        }
        public static float Maximum(this Vector2 thisVector2)
        {
            return thisVector2.y;
        }

        /// <summary>
        /// Returns direction to some position.
        /// </summary>
        /// <param name="from">Initial position.</param>
        /// <param name="to">Final position.</param>
        /// <returns>Returns a normalized vector2 with direction.</returns>
        public static Vector2 Direction(this Vector2 from, Vector2 to)
        {
            return (to - from).normalized;
        }
    }
}
