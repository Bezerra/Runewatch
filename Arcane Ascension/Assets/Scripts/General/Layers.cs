using UnityEngine;

/// <summary>
/// Class with layers combinations.
/// </summary>
public class Layers : MonoBehaviour
{
    private static Layers instance;

    [SerializeField] private LayerMask allExceptPlayerAndEnemy;
    [SerializeField] private LayerMask enemyWithWalls;
    [SerializeField] private LayerMask enemySensiblePoint;

    public static LayerMask AllExceptPlayerAndEnemy => instance.allExceptPlayerAndEnemy;
    public static LayerMask EnemyWithWalls => instance.enemyWithWalls;

    public static LayerMask EnemySensiblePoint => instance.enemySensiblePoint;

    private void Awake() =>
        instance = this;
}
