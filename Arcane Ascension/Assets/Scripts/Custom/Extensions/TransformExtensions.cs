﻿using UnityEngine;

namespace ExtensionMethods
{
    /// <summary>
    /// Class responsible for add Transform extensions.
    /// </summary>
    public static class TransformExtentions
    {
        /// <summary>
        /// Checks if a transform forward is looking to a position. 
        /// Has a maximum angle to look.
        /// </summary>
        /// <param name="from">This transform.</param>
        /// <param name="finalPosition">Final position to check. 
        /// Direction is not needed, only the vector3 with position.</param>
        /// <returns>Returns true if this transform is looking towards that 
        /// position.</returns>
        /// <param name="ignoreHeightDistance">False if height matters. True if
        /// this check is considered to be on the same Y (ignoring Y distance).</param>
        /// <param name="maximumAngle">Maximum angle to check.</param>
        /// <returns>Returns true if a transform is looking towards another transform.</returns>
        public static bool IsLookingTowards(this Transform from,
            Vector3 finalPosition, bool ignoreHeightDistance = false, float maximumAngle = 10)
        {
            if (ignoreHeightDistance == false)
            {
                Vector3 dir = from.position.Direction(finalPosition);
                if (Vector3.Angle(dir, from.forward) < maximumAngle) return true;
            }
            else
            {
                Vector2 fromPosition = new Vector2(from.position.x, from.position.z);
                Vector2 targetPosition = new Vector2(finalPosition.x, finalPosition.z);
                Vector2 dir = fromPosition.Direction(targetPosition);
                Debug.DrawRay(new Vector3(fromPosition.x, 1, fromPosition.y), from.forward);
                if (Vector2.Angle(dir, from.forward) < maximumAngle) return true;
            }
            
            return false;
        }

        /// <summary>
        /// Checks if a transform forward is looking to a position. 
        /// Has a maximum angle to look.
        /// </summary>
        /// <param name="from">This transform.</param>
        /// <param name="finalPosition">Final position to check. 
        /// Direction is not needed, only the vector3 with position.</param>
        /// <returns>Returns true if this transform is looking towards that 
        /// position.</returns>
        /// <param name="ignoreHeightDistance">False if height matters. True if
        /// this check is considered to be on the same Y (ignoring Y distance).</param>
        /// <param name="maximumAngle">Maximum angle to check.</param>
        /// <returns>Returns true if a transform is looking towards another transform.</returns>
        public static bool IsLookingTowards(this Transform from,
            Transform finalPosition, bool ignoreHeightDistance = false, float maximumAngle = 10)
        {
            if (ignoreHeightDistance == false)
            {
                Vector3 dir = from.position.Direction(finalPosition.position);
                if (Vector3.Angle(dir, from.forward) < maximumAngle) return true;
            }
            else
            {
                Vector2 fromPosition = new Vector2(from.position.x, from.position.z);
                Vector2 targetPosition = 
                    new Vector2(finalPosition.transform.position.x, finalPosition.transform.position.z);
                Vector2 dir = fromPosition.Direction(targetPosition);
                if (Vector2.Angle(dir, from.forward) < maximumAngle) return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if a transform can see another transform in any direction. 
        /// Parameter "to" must have the desired layer to check
        /// (this layer must be on layers parameter as well).
        /// </summary>
        /// <param name="from">From this transform.</param>
        /// <param name="to">Final transform. Must have the desired layer
        /// to check.</param>
        /// <param name="layers">Layers to check.</param>
        /// <returns>Returns true if source transform can see the final 
        /// transform.</returns>
        public static bool CanSee(this Transform from, Transform to,
            LayerMask layers)
        {
            Ray rayTo = new Ray(from.position, from.position.Direction(to.position));
            float distance = Vector3.Distance(from.position, to.position);
            if (Physics.Raycast(rayTo, out RaycastHit hit, distance, layers))
            {
                if (hit.collider.gameObject.layer == to.gameObject.layer)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Rotates this transform towards another transform on Y only.
        /// </summary>
        /// <param name="from">This transform.</param>
        /// <param name="to">Other transform.</param>
        public static void LookAtY(this Transform from, Transform to)
        {
            from.LookAt(to);
            from.transform.eulerAngles = new Vector3(0, from.transform.eulerAngles.y, 0);
        }

        /// <summary>
        /// Rotates this transform towards another transform on Y only.
        /// </summary>
        /// <param name="from">This transform.</param>
        /// <param name="to">Other transform.</param>
        /// <param name="rotationSpeed">Speed of the rotation.</param>
        public static void LookAtYLerp(this Transform from, Transform to, float rotationSpeed = 1)
        {
            Quaternion finalRotation = Quaternion.LookRotation(from.Direction(to), Vector3.up);
            from.transform.rotation = Quaternion.Lerp(from.transform.rotation, finalRotation, Time.deltaTime * rotationSpeed);
            from.transform.eulerAngles = new Vector3(0, from.transform.eulerAngles.y, 0);
        }

        /// <summary>
        /// Rotates this transform towards another transform on Y only.
        /// </summary>
        /// <param name="from">This transform.</param>
        /// <param name="to">Other vector3.</param>
        /// <param name="rotationSpeed">Speed of the rotation.</param>
        public static void LookAtYLerp(this Transform from, Vector3 to, float rotationSpeed = 1)
        {
            Quaternion finalRotation = Quaternion.LookRotation(from.Direction(to), Vector3.up);
            from.transform.rotation = Quaternion.Lerp(from.transform.rotation, finalRotation, Time.deltaTime * rotationSpeed);
            from.transform.eulerAngles = new Vector3(0, from.transform.eulerAngles.y, 0);
        }

        /// <summary>
        /// Returns direction to some position.
        /// </summary>
        /// <param name="from">Initial transform position.</param>
        /// <param name="to">Final transform position.</param>
        /// <returns>Returns a normalized vector3 with direction.</returns>
        public static Vector3 Direction(this Transform from, Transform to)
        {
            return (to.position - from.position).normalized;
        }

        /// <summary>
        /// Returns direction to some position.
        /// </summary>
        /// <param name="from">Initial transform position.</param>
        /// <param name="to">Final transform position.</param>
        /// <returns>Returns a normalized vector3 with direction.</returns>
        public static Vector3 Direction(this Transform from, Vector3 to)
        {
            return (to - from.position).normalized;
        }

        /// <summary>
        /// Checks if a transform is looking at the same direction as another transform.
        /// </summary>
        /// <param name="from">From this transform.</param>
        /// <param name="target">Target transform.</param>
        /// <param name="value">Float value between 0 and 1. 0 means the transforms
        /// are perpendicular, 1 means the transforms are exactly in the same
        /// direction.</param>
        /// <returns>True if directions are the same.</returns>
        public static bool SameDirectionAs(this Transform from, Transform target,
            float value = 0.9f)
        {
            if (value < 0) value = 0;
            if (value > 1) value = 1;
            if (Vector3.Dot(from.forward, target.forward) >= value) return true;
            return false;
        }

        /// <summary>
        /// Checks if a transform is looking on the opposite direction of another transform.
        /// </summary>
        /// <param name="from">From this transform.</param>
        /// <param name="target">Target transform.</param>
        /// <param name="value">Float value between -1 and 0.</param>
        /// <returns>True if the directions are inverse.</returns>
        public static bool InverseDirectionAs(this Transform from, Transform target,
            float value = -0.9f)
        {
            if (value < -1) value = -1;
            if (value > 0) value = 0;
            if (Vector3.Dot(from.forward, target.forward) <= value) return true;
            return false;
        }
    }
}
