using UnityEngine;

/// <summary>
/// Class responsible for containg shopkeeper spawn information.
/// </summary>
public class ShopkeeperGizmosMesh : MonoBehaviour
{
    [SerializeField] private Mesh shopkeeperMesh;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawMesh(shopkeeperMesh, transform.position, transform.rotation);
    }
}
