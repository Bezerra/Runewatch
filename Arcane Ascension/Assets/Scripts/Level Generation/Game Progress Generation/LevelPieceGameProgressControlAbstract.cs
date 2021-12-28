using System;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Abstract Class responsible for controlling what happens inside level piece room.
/// </summary>
public abstract class LevelPieceGameProgressControlAbstract : MonoBehaviour
{
    [SerializeField] protected AvailableListOfEnemiesToSpawnSO listOfEnemies;

    private bool playerInCombat;
    protected bool PlayerInCombat
    {
        get => playerInCombat;
        set
        {
            playerInCombat = value;
            OnPlayerInCombat(value);
        }
    }

    private static void OnPlayerInCombat(bool condition) => EventPlayerInCombat?.Invoke(condition);
    public static event Action<bool> EventPlayerInCombat;

    // Loot
    [Header("Loot variables")]
    [SerializeField] protected LootRates lootRates;
    [RangeMinMax(0, 1000)] [SerializeField] protected Vector2 goldQuantity;
    [RangeMinMax(0, 1000)] [SerializeField] protected Vector2 arcanePowerQuantity;
    protected IList<(LootType, Vector3)> droppedLoot;
    private System.Random random;
    [SerializeField] protected Transform lootSpawnPosition;

    // Enemies
    private EnemySpawnPoint[] enemySpawnPoints;
    private int quantityOfEnemiesSpawned;
    private bool haveEnemiesSpawned;
    private IList<GameObject> spawnedEnemies;

    // Doors and exit blockers
    protected ContactPointDoor[] contactPointsDoors;
    protected IList<BoxCollider> exitBlockers;

    protected virtual void Awake()
    {
        enemySpawnPoints = GetComponentsInChildren<EnemySpawnPoint>();
        contactPointsDoors = GetComponentsInChildren<ContactPointDoor>();
        droppedLoot = new List<(LootType, Vector3)>();
        GameProgressCollider[] gameProgressColliders = GetComponentsInChildren<GameProgressCollider>(true);
        exitBlockers = new List<BoxCollider>();
        foreach (GameProgressCollider gpc in gameProgressColliders)
            exitBlockers.Add(gpc.GetComponent<BoxCollider>());
        random = new System.Random();
        playerInCombat = false;

        spawnedEnemies = new List<GameObject>();
    }

    /// <summary>
    /// Creates all enemies and disables them.
    /// </summary>
    private void Start()
    {
        foreach (EnemySpawnPoint enemySpawnPoint in enemySpawnPoints)
        {
            GameObject spawnedEnemy = Instantiate(
                    listOfEnemies.SpawnEnemy(enemySpawnPoint.PointInformation[DifficultyType.Normal]),
                    enemySpawnPoint.transform.position,
                    enemySpawnPoint.transform.rotation);

            spawnedEnemies.Add(spawnedEnemy);
            spawnedEnemy.SetActive(false);
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

                foreach (GameObject spawnedEnemy in spawnedEnemies)
                {
                    spawnedEnemy.SetActive(true);
                    quantityOfEnemiesSpawned++;
                    if (spawnedEnemy.TryGetComponentInChildrenFirstGen(out Stats enemyStats))
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

                // Gets random drops and spawns them
                GetDrop(lootSpawnPosition.position);
                IEnumerator<(LootType, Vector3)> itemEnumerator = droppedLoot.GetEnumerator();
                while (itemEnumerator.MoveNext())
                {
                    GameObject spawnedLoot = ItemLootPoolCreator.Pool.InstantiateFromPool(
                        itemEnumerator.Current.Item1.ToString(),
                        itemEnumerator.Current.Item2, 
                        Quaternion.identity);

                    if (spawnedLoot.TryGetComponent(out ICurrency currency))
                    {
                        if (currency.CurrencyType == CurrencyType.Gold)
                            currency.Amount = goldQuantity;
                        else
                            currency.Amount = arcanePowerQuantity;
                    }
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

    /// <summary>
    /// Gets a drop and sets random position with a received position.
    /// </summary>
    /// <param name="position">Position to set the item.</param>
    protected void GetDrop(Vector3 position)
    {
        for (int i = 0; i < lootRates.LootPieces.Count; i++)
        {
            if (lootRates.LootPieces[i].LootRate.PercentageCheck(random))
            {
                Vector3 newPosition = position + new Vector3(
                    UnityEngine.Random.Range(-1f, 1f), 0,
                    UnityEngine.Random.Range(-1f, 1f));

                droppedLoot.Add((lootRates.LootPieces[i].LootType, newPosition));
            }
        }
    }
}
