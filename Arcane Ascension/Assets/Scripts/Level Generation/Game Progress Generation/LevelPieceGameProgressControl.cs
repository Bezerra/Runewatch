using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Class responsible for controlling what happens when the player is inside a level piece room.
/// </summary>
public class LevelPieceGameProgressControl : MonoBehaviour
{
    [SerializeField] private BoxCollider[] exitBlockers;
    [SerializeField] private EnemySpawnPoint[] enemySpawnPoints;
    [SerializeField] private Transform shopkeeperTransform;
    [SerializeField] private AvailableListOfEnemiesToSpawnSO listOfEnemies;

    // Variables to keep track of progress
    public bool RoomSpawnsShopkeeper { get; set; }
    private int quantityOfEnemiesSpawned;
    private bool haveEnemiesSpawned;
    private ContactPointDoor[] contactPointsDoors;

    private void Awake() =>
        contactPointsDoors = GetComponentsInChildren<ContactPointDoor>();

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
                if (RoomSpawnsShopkeeper && shopkeeperTransform != null)
                {
                    CharactersAndNpcsPoolCreator.Pool.InstantiateFromPool(
                        "Shopkeeper", shopkeeperTransform.position, 
                        shopkeeperTransform.rotation);
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
