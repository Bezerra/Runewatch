using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Class responsible for holding a list with information about an enemy spawn point.
/// </summary>
public class EnemySpawnPoint : MonoBehaviour
{
    [SerializeField] private Mesh meshForGizmos;

    // Serialized list with enemy spawn for all difficulties
    [TableList]
    [SerializeField]
    private List<EnemySpawnPointInformation> pointInformation;

    [TableList]
    [SerializeField]
    private EnemyWave wave;

    public EnemyWave Wave => wave;

    /// <summary>
    /// Getter for a dictionary with this point information.
    /// </summary>
    public Dictionary<DifficultyType, EnemySpawnType> PointInformation { get; private set; }

    private void Awake()
    {
        PointInformation = new Dictionary<DifficultyType, EnemySpawnType>();
        for (int i = 0; i < pointInformation.Count; i++)
        {
            PointInformation.Add(
                pointInformation[i].Difficulty, pointInformation[i].EnemySpawnType);
        }
    }

    private void OnDrawGizmos()
    {
        if (Wave == EnemyWave.FirstWave)
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.green;

        Gizmos.DrawMesh(meshForGizmos, transform.position, transform.rotation, new Vector3(0.2f, 0.2f, 0.2f));
    }
}

