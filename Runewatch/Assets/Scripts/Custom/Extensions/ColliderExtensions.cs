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

        /// <summary>
        /// Tries to get a component in this gameobject or all of its direct childs.
        /// </summary>
        /// <typeparam name="T">Component to get.</typeparam>
        /// <param name="thisGameObject">This gameObject.</param>
        /// <param name="component">Out component variable.</param>
        /// <returns>True if component was found.</returns>
        public static bool TryGetComponentInChildrenFirstGen<T>(this Collider thisGameObject, out T component)
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
                        if (thisGameObject.transform.GetChild(i).TryGetComponent(out outComponent))
                        {
                            component = outComponent;
                            return true;
                        }
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
