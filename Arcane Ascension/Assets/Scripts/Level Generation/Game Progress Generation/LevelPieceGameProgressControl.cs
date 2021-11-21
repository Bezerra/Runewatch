using UnityEngine;
using ExtensionMethods;
using UnityEngine.AI;

/// <summary>
/// Class responsible for controlling what happens when the player is inside a level piece room.
/// </summary>
public class LevelPieceGameProgressControl : MonoBehaviour
{
    [SerializeField] private BoxCollider[] exitBlockers;
    [SerializeField] private EnemySpawnPoint[] enemySpawnPoints;
    [SerializeField] private AvailableListOfEnemiesToSpawnSO listOfEnemies;

    private int quantityOfEnemiesSpawned;
    private bool haveEnemiesSpawned;

    /// <summary>
    /// Blocks all exits.
    /// </summary>
    private void BlockUnblockExits(bool condition)
    {
        if (exitBlockers.Length > 0)
        {
            foreach (BoxCollider exitBlock in exitBlockers)
            {
                if (condition)
                    exitBlock.enabled = true;
                else
                    exitBlock.enabled = false;
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
                    if (enemySpawnedGO.TryGetComponentInChildren(out Stats enemyStats))
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
