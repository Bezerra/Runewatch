using UnityEngine;

/// <summary>
/// Class responsible for containg chest spawn information.
/// </summary>
public class ChestGizmosMesh : MonoBehaviour
{
    [SerializeField] private Mesh chestMesh;

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.6f, 0.4f, 0.23f);
        Gizmos.DrawMesh(chestMesh, transform.position, transform.rotation, new Vector3(2, 2, 2));
    }
}
