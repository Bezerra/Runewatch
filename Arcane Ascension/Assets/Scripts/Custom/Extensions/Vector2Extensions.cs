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
    }
}
