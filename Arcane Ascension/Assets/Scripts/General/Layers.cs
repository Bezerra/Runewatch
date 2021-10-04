using UnityEngine;

/// <summary>
/// Class with layers combinations.
/// </summary>
public class Layers : MonoBehaviour
{
    private static Layers instance;

    [SerializeField] private LayerMask allExceptPlayerAndEnemy;
    [SerializeField] private LayerMask enemyWithWalls;

    public static LayerMask AllExceptPlayerAndEnemy => instance.allExceptPlayerAndEnemy;
    public static LayerMask EnemyWithWalls => instance.enemyWithWalls;

    private void Awake() =>
        instance = this;
}
