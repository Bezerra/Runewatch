using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Abstract Class responsible for controlling what happens inside level piece room.
/// </summary>
public abstract class LevelPieceGameProgressControlAbstract : MonoBehaviour
{
    [SerializeField] protected AvailableListOfEnemiesToSpawnSO listOfEnemies;

    // Enemies
    private EnemySpawnPoint[] enemySpawnPoints;
    private int quantityOfEnemiesSpawned;
    private bool haveEnemiesSpawned;

    // Doors and exit blockers
    protected ContactPointDoor[] contactPointsDoors;
    protected IList<BoxCollider> exitBlockers;

    protected virtual void Awake()
    {
        enemySpawnPoints = GetComponentsInChildren<EnemySpawnPoint>();
        contactPointsDoors = GetComponentsInChildren<ContactPointDoor>();
        
        GameProgressCollider[] gameProgressColliders = GetComponentsInChildren<GameProgressCollider>(true);
        exitBlockers = new List<BoxCollider>();
        foreach (GameProgressCollider gpc in gameProgressColliders)
            exitBlockers.Add(gpc.GetComponent<BoxCollider>());
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
    /// Blocks/unblocks all exits.
    /// Triggered when the player enters a room or defeats all enemies.
    /// </summary>
    protected virtual void BlockUnblockExits(bool condition)
    {
        if (exitBlockers.Count > 0)
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
