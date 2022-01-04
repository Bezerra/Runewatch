using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Abstract Class responsible for controlling what happens inside level piece room.
/// </summary>
public abstract class LevelPieceGameProgressControlAbstract : MonoBehaviour
{
    [SerializeField] protected AvailableListOfEnemiesToSpawnSO listOfEnemies;

    [SerializeField] protected bool spawnsSecondWave = true;
    [Range(5f, 30f)] [SerializeField] protected float timeToSpawnSecondWave = 7f;
    private bool hasSpawnedSecondWave;

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
    protected CharacterSaveDataController stpData;

    // Enemies
    protected IList<EnemySpawnPoint> enemySpawnPoints;
    private int quantityOfEnemiesSpawned;
    private bool haveEnemiesSpawned;
    private IList<GameObject> spawnedFirstWaveEnemies;
    private IList<GameObject> spawnedSecondWaveEnemies;

    // Doors and exit blockers
    protected ContactPointDoor[] contactPointsDoors;
    protected IList<BoxCollider> exitBlockers;

    protected virtual void Awake()
    {
        enemySpawnPoints = GetComponentsInChildren<EnemySpawnPoint>(true);
        contactPointsDoors = GetComponentsInChildren<ContactPointDoor>();
        droppedLoot = new List<(LootType, Vector3)>();
        GameProgressCollider[] gameProgressColliders = 
            GetComponentsInChildren<GameProgressCollider>(true);
        exitBlockers = new List<BoxCollider>();
        foreach (GameProgressCollider gpc in gameProgressColliders)
            exitBlockers.Add(gpc.GetComponent<BoxCollider>());
        random = new System.Random();
        playerInCombat = false;
        stpData = FindObjectOfType<CharacterSaveDataController>();
        hasSpawnedSecondWave = false;

        spawnedFirstWaveEnemies = new List<GameObject>();
        spawnedSecondWaveEnemies = new List<GameObject>();
    }

    /// <summary>
    /// Creates all enemies and disables them.
    /// </summary>
    protected virtual void Start()
    {
        // If the list is empty, ignores the rest
        if (enemySpawnPoints == null || enemySpawnPoints.Count == 0)
            return;

        foreach (EnemySpawnPoint enemySpawnPoint in enemySpawnPoints)
        {
            GameObject spawnedEnemy = Instantiate(
                listOfEnemies.SpawnEnemy(enemySpawnPoint.PointInformation[DifficultyType.Normal]),
                enemySpawnPoint.transform.position,
                enemySpawnPoint.transform.rotation);

            if (enemySpawnPoint.Wave == EnemyWave.FirstWave)
            {
                spawnedFirstWaveEnemies.Add(spawnedEnemy);
            }
            else if (enemySpawnPoint.Wave == EnemyWave.SecondWave)
            {
                spawnedSecondWaveEnemies.Add(spawnedEnemy);
            }

            spawnedEnemy.SetActive(false);
        }
    }

    /// <summary>
    /// Spawns enemies on enemy points of this room.
    /// </summary>
    public void SpawnEnemies()
    {
        // If the list is empty, ignores the rest
        if (enemySpawnPoints == null || enemySpawnPoints.Count == 0)
            return;

        if (haveEnemiesSpawned == false)
        {
            if (enemySpawnPoints.Count > 0)
            {
                BlockUnblockExits(true);

                foreach (GameObject spawnedEnemy in spawnedFirstWaveEnemies)
                {
                    spawnedEnemy.SetActive(true);
                    quantityOfEnemiesSpawned++;
                    if (spawnedEnemy.TryGetComponentInChildrenFirstGen(out Stats enemyStats))
                    {
                        enemyStats.EventDeath += EnemyDeath;

                        if (enemyStats.CommonAttributes.Type == CharacterType.Boss)
                        {
                            enemyStats.EventDeath += BossDeath;
                        }
                    }
                }

                haveEnemiesSpawned = true;
            }
        }

        // If no enemies were on second wave
        if (spawnedSecondWaveEnemies.Count == 0)
        {
            hasSpawnedSecondWave = true;
            return;
        }

        // If this rooms spawns a second wave
        if (spawnsSecondWave)
            StartCoroutine(SpawnSecondWaveEnemies());
    }

    /// <summary>
    /// Second wave of enemies.
    /// </summary>
    /// <returns>Waits for seconds</returns>
    private IEnumerator SpawnSecondWaveEnemies()
    {
        yield return new WaitForSeconds(timeToSpawnSecondWave);

        foreach (GameObject spawnedEnemy in spawnedSecondWaveEnemies)
        {
            spawnedEnemy.SetActive(true);
            quantityOfEnemiesSpawned++;
            if (spawnedEnemy.TryGetComponentInChildrenFirstGen(out Stats enemyStats))
            {
                enemyStats.EventDeath += EnemyDeath;

                if (enemyStats.CommonAttributes.Type == CharacterType.Boss)
                {
                    enemyStats.EventDeath += BossDeath;
                }
            }
        }

        hasSpawnedSecondWave = true;
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

        // If second wave has not spawned yet
        if (spawnsSecondWave)
        {
            if (hasSpawnedSecondWave == false)
                return;
        }

        // If all enemies area dead
        if (quantityOfEnemiesSpawned == 0)
        {
            BlockUnblockExits(false);
        }
    }

    /// <summary>
    /// Triggered when a boss dies.
    /// </summary>
    /// <param name="enemyStats">Boss stats.</param>
    protected virtual void BossDeath(Stats enemyStats) =>
        enemyStats.EventDeath -= BossDeath;

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
