using UnityEngine;

namespace ExtensionMethods
{
    /// <summary>
    /// Static class for GameObject extention methods.
    /// </summary>
    public static class ColliderExtensions
    {
        /// <summary>
        /// Tries to get a component in this gameobject or all its parents until the parent is null.
        /// </summary>
        /// <typeparam name="T">Component to get.</typeparam>
        /// <param name="thisGameObject">This gameObject.</param>
        /// <param name="component">Out component variable.</param>
        /// <returns>True if component was found.</returns>
        public static bool TryGetComponentInParent<T>(this Collider thisCollider, out T component)
        {
            if (thisCollider.TryGetComponent(out T outComponent))
            {
                component = outComponent;
                return true;
            }

            else
            {
                if (thisCollider.transform.parent != null)
                {
                    return thisCollider.transform.parent.gameObject.TryGetComponentInParent(out component);
                }

                else
                {
                    component = default;
                    return false;
                }
            }
        }
    }
}
