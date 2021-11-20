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
    [SerializeField] private LayerMask playerEnemy;
    [SerializeField] private LayerMask playerEnemyWithWalls;
    [SerializeField] private LayerMask wallsFloor;
    [SerializeField] private LayerMask walls;
    [SerializeField] private int wallsNum;
    [SerializeField] private int ceilingNum;
    [SerializeField] private LayerMask floor;
    [SerializeField] private int floorNum;
    [SerializeField] private LayerMask enemySensiblePoint;
    [SerializeField] private int enemySensiblePointNum;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private int enemyLayerNum;
    [SerializeField] private LayerMask enemyImmuneLayer;
    [SerializeField] private int enemyImmuneLayerNum;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private int playerLayerNum;
    [SerializeField] private LayerMask ignoreLayer;
    [SerializeField] private int ignoreLayerNum;
    [SerializeField] private LayerMask interectable;

    public static LayerMask AllExceptPlayerAndEnemy => instance.allExceptPlayerAndEnemy;
    public static LayerMask EnemyWithWalls => instance.enemyWithWalls;
    public static LayerMask EnemyWithWallsFloor => instance.enemyWithWallsFloor;
    public static LayerMask PlayerEnemy => instance.playerEnemy;
    public static LayerMask PlayerEnemyWithWalls => instance.playerEnemyWithWalls;
    public static LayerMask WallsFloor => instance.wallsFloor;
    public static LayerMask Floor => instance.floor;
    public static int FloorNum => instance.floorNum;
    public static LayerMask Walls => instance.walls;
    public static int WallsNum => instance.wallsNum;
    public static int CeilingNum => instance.ceilingNum;
    public static LayerMask EnemySensiblePoint => instance.enemySensiblePoint;
    public static int EnemySensiblePointNum => instance.enemySensiblePointNum;
    public static LayerMask EnemyLayer => instance.enemyLayer;
    public static int EnemyLayerNum => instance.enemyLayerNum;
    public static LayerMask EnemyImmuneLayer => instance.enemyImmuneLayer;
    public static int EnemyImmuneLayerNum => instance.enemyImmuneLayerNum;
    public static LayerMask PlayerLayer => instance.playerLayer;
    public static int PlayerLayerNum => instance.playerLayerNum;
    public static LayerMask IgnoreLayer => instance.ignoreLayer;
    public static int IgnoreLayerNum => instance.ignoreLayerNum;
    public static int Interectable => instance.interectable;

    private void Awake() =>
        instance = this;
}
