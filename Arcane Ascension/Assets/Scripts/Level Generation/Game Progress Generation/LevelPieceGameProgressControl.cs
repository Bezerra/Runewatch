using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Class responsible for controlling what happens when the player is inside a level piece room.
/// </summary>
public class LevelPieceGameProgressControl : MonoBehaviour
{
    [SerializeField] private BoxCollider[] exitBlockers;
    [SerializeField] private EnemySpawnPoint[] enemySpawnPoints;
    [SerializeField] private AvailableListOfEnemiesToSpawnSO listOfEnemies;

    // Variables to keep track of progress
    // Shopkeeper
    private ShopkeeperGizmosMesh[] shopkeeper;
    public bool RoomSpawnsShopkeeper { get; set; }

    // Chests
    private ChestGizmosMesh chest;
    public AbilityType AbilityType { get; set; }
    public bool RoomSpawnsChest { get; set; }
    private GameObject spawnedChest;
    private Chest chestScript;

    // Enemies
    private int quantityOfEnemiesSpawned;
    private bool haveEnemiesSpawned;

    // Doors
    private ContactPointDoor[] contactPointsDoors;

    private void Awake()
    {
        contactPointsDoors = GetComponentsInChildren<ContactPointDoor>();
        shopkeeper = GetComponentsInChildren<ShopkeeperGizmosMesh>(true);
        chest = GetComponentInChildren<ChestGizmosMesh>(true);
    }

    public void SpawnChestAfterGeneration()
    {
        if (RoomSpawnsChest)
        {
            if (AbilityType == AbilityType.Spell)
            {
                spawnedChest = RunProgressPoolCreator.Pool.InstantiateFromPool(
                    "Spell Chest", chest.transform.position,
                    chest.transform.rotation);
            }
            else
            {
                spawnedChest = RunProgressPoolCreator.Pool.InstantiateFromPool(
                    "Passive Chest", chest.transform.position,
                    chest.transform.rotation);
            }
        }
        if (spawnedChest.TryGetComponent(out chestScript))
        {
            chestScript.CanOpen = false;
        }
    }

    /// <summary>
    /// Blocks/unblocks all exits.
    /// Triggered when the player enters a room or defeats all enemies.
    /// </summary>
    private void BlockUnblockExits(bool condition)
    {
        if (exitBlockers.Length > 0)
        {
            if (condition)
            {
                foreach (BoxCollider exitBlock in exitBlockers)
                {
                    exitBlock.enabled = true;
                }

                foreach (ContactPointDoor contactPoint in contactPointsDoors)
                {
                    contactPoint.BlockPassage();
                    contactPoint.ClosePassage();
                }
            }
            else
            {
                foreach (BoxCollider exitBlock in exitBlockers)
                {
                    exitBlock.enabled = false;
                }

                foreach (ContactPointDoor contactPoint in contactPointsDoors)
                {
                    // Keeps passage closed but unblocks it
                    contactPoint.ClosePassage();
                    contactPoint.UnblockPassage();
                }

                // If this piece is supposed to spawn shopkeeper, spawns it
                if (RoomSpawnsShopkeeper && shopkeeper != null)
                {
                    for (int i = 0; i < shopkeeper.Length; i++)
                    {
                        Collider[] collisions = Physics.OverlapSphere(
                            shopkeeper[i].transform.position, 4, 
                            Layers.PlayerNormalAndInvisibleLayer);

                        if(collisions.Length == 0)
                        {
                            RunProgressPoolCreator.Pool.InstantiateFromPool(
                                "Shopkeeper", shopkeeper[i].transform.position,
                                shopkeeper[i].transform.rotation);

                            break;
                        }
                    }
                }

                // If this piece is supposed to spawn a chest, spawns it
                // Type of chest randomly set on the end of level generator
                if (RoomSpawnsChest && chestScript != null)
                {
                    chestScript.CanOpen = true;
                }
            }
        }
    }

    /// <summary>
    /// Spawns enemies on enemy points of this room.
    /// </summary>
    public void SpawnEnemies()
    {
        if (haveEnemiesSpawned == false)
        {
            if (enemySpawnPoints.Length > 0)
            {
                BlockUnblockExits(true);

                foreach (EnemySpawnPoint enemySpawnPoint in enemySpawnPoints)
                {
                    GameObject enemySpawnedGO = Instantiate(
                        listOfEnemies.SpawnEnemy(
                            enemySpawnPoint.PointInformation[DifficultyType.Normal]),
                        enemySpawnPoint.transform.position,
                        enemySpawnPoint.transform.rotation);

                    quantityOfEnemiesSpawned++;
                    if (enemySpawnedGO.TryGetComponentInChildrenFirstGen(out Stats enemyStats))
                    {
                        enemyStats.EventDeath += EnemyDeath;
                    }
                }
                haveEnemiesSpawned = true;
            }
        }
    }

    /// <summary>
    /// When an enemy dies, checks if all enemies are dead to unblock room exits.
    /// </summary>
    /// <param name="enemyStats">EnemyStats of the dead enemy</param>
    private void EnemyDeath(Stats enemyStats)
    {
        enemyStats.EventDeath -= EnemyDeath;
        quantityOfEnemiesSpawned--;

        // If all enemies area dead
        if (quantityOfEnemiesSpawned == 0)
        {
            BlockUnblockExits(false);
        }
    }
}
