using UnityEngine;

namespace ExtensionMethods
{
    /// <summary>
    /// Static class for GameObject extention methods.
    /// </summary>
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Tries to get a component in this gameobject or all its parents until the parent is null.
        /// </summary>
        /// <typeparam name="T">Component to get.</typeparam>
        /// <param name="thisGameObject">This gameObject.</param>
        /// <param name="component">Out component variable.</param>
        /// <returns>True if component was found.</returns>
        public static bool TryGetComponentInParent<T>(this GameObject thisGameObject, out T component)
        {
            if (thisGameObject.TryGetComponent(out T outComponent))
            {
                component = outComponent;
                return true;
            }

            else
            {
                if (thisGameObject.transform.parent != null)
                {
                    return thisGameObject.transform.parent.gameObject.TryGetComponentInParent(out component);
                }

                else
                {
                    component = default;
                    return false;
                }
            }
        }

        /// <summary>
        /// NOT 100% TESTED.
        /// Tries to get a component in this gameobject or all its childs until the parent is null.
        /// </summary>
        /// <typeparam name="T">Component to get.</typeparam>
        /// <param name="thisGameObject">This gameObject.</param>
        /// <param name="component">Out component variable.</param>
        /// <returns>True if component was found.</returns>
        public static bool TryGetComponentInChildren<T>(this GameObject thisGameObject, out T component)
        {
            if (thisGameObject.TryGetComponent(out T outComponent))
            {
                component = outComponent;
                return true;
            }

            else
            {
                if (thisGameObject.transform.childCount > 0)
                {
                    for (int i = 0; i < thisGameObject.transform.childCount; i++)
                    {
                        return thisGameObject.transform.GetChild(i).gameObject.TryGetComponentInParent(out component);
                    }
                    component = default;
                    return false;
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
