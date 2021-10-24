using UnityEngine;

/// <summary>
/// Class with layers combinations.
/// </summary>
public class Layers : MonoBehaviour
{
    private static Layers instance;

    [SerializeField] private LayerMask allExceptPlayerAndEnemy;
    [SerializeField] private LayerMask enemyWithWalls;
    [SerializeField] private LayerMask enemyWithWallsFloor;
    [SerializeField] private LayerMask walls;
    [SerializeField] private int wallsNum;
    [SerializeField] private LayerMask floor;
    [SerializeField] private int floorNum;
    [SerializeField] private LayerMask enemySensiblePoint;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private int enemyLayerNum;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private int playerLayerNum;
    [SerializeField] private LayerMask ignoreLayer;
    [SerializeField] private int ignoreLayerNum;

    public static LayerMask AllExceptPlayerAndEnemy => instance.allExceptPlayerAndEnemy;
    public static LayerMask EnemyWithWalls => instance.enemyWithWalls;
    public static LayerMask EnemyWithWallsFloor => instance.enemyWithWallsFloor;
    public static LayerMask Floor => instance.floor;
    public static int FloorNum => instance.floorNum;
    public static LayerMask Walls => instance.walls;
    public static int WallsNum => instance.wallsNum;
    public static LayerMask EnemySensiblePoint => instance.enemySensiblePoint;
    public static LayerMask EnemyLayer => instance.enemyLayer;
    public static int EnemyLayerNum => instance.enemyLayerNum;
    public static LayerMask PlayerLayer => instance.playerLayer;
    public static int PlayerLayerNum => instance.playerLayerNum;
    public static LayerMask IgnoreLayer => instance.ignoreLayer;
    public static int IgnoreLayerNum => instance.ignoreLayerNum;

    private void Awake() =>
        instance = this;
}
