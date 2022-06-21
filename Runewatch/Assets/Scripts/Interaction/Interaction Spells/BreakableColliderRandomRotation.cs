using UnityEngine;

/// <summary>
/// Gives a collider a random Y rotation.
/// </summary>
public class BreakableColliderRandomRotation : MonoBehaviour
{
    [SerializeField] private SphereCollider sphereCollider;

    private void Awake()
    {
        sphereCollider.radius += Random.Range(0, 0.3f);
    }
}
