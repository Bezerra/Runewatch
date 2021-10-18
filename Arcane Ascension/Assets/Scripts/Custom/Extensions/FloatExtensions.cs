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
    }
}
