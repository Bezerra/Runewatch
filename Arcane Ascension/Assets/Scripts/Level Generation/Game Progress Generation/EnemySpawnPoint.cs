using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Class responsible for holding a list with information about an enemy spawn point.
/// </summary>
public class EnemySpawnPoint : MonoBehaviour
{
    [SerializeField] private Mesh meshForGizmos;

    [TableList]
    [SerializeField]
    private List<EnemySpawnPointInformation> pointInformation;

    private void OnDrawGizmos()
    {
        Gizmos.DrawMesh(meshForGizmos, transform.position, transform.rotation, new Vector3(0.2f, 0.2f, 0.2f));
    }
}
