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
    protected readonly float[] GOLDFLOORMULTIPLIERS =
        new float[] { 1f, 1f, 1f, 1.25f, 1.25f, 1.25f, 1.5f, 1.5f, 1.5f };

    [SerializeField] private bool testRoomIsolatedScene;
    [Range(1, 9)] [SerializeField] private int floorToTestOnIsolatedScenes;

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

    private static void OnPlayerInCombat(bool condition) => 
        EventPlayerInCombat?.Invoke(condition);
    public static event Action<bool> EventPlayerInCombat;

    // Loot
    [Header("Loot variables")]
    // The FIXED loot on this room
    protected LootType lootTypeOnCurrentRoom;
    // Other possible random loot
    [SerializeField] protected LootRates lootRates;
    [RangeMinMax(0, 1000)] [SerializeField] protected Vector2 goldQuantity;
    protected IList<(LootType, Vector3)> droppedLoot;
    private System.Random random;
    [SerializeField] protected Transform lootSpawnPosition;
    protected CharacterSaveDataController stpData;
    // ARCANE POWER DROPS ON ENEMIES FOR NOW, UNCOMMENT THIS LATER IF NEEDED
    //[RangeMinMax(0, 1000)] [SerializeField] protected Vector2 arcanePowerQuantity;

    // Enemies
    protected IList<EnemySpawnPoint> enemySpawnPoints;
    private int quantityOfEnemiesSpawned;
    private bool haveEnemiesSpawned;
    private IList<GameObject> spawnedFirstWaveEnemies;
    private IList<GameObject> spawnedSecondWaveEnemies;
    protected RunSaveDataController runSaveData;

    // Doors and exit blockers
    protected ContactPointDoor[] contactPointsDoors;
    protected IList<BoxCollider> exitBlockers;

    private LevelGenerator levelGenerator;
    public LevelGenerator LevelGenerator
    {
        private get => levelGenerator;
        set
        {
            levelGenerator = value;
            LevelGenerator.EndedGeneration += EventSpawnEnemies;
            LevelGenerator.EndedGeneration += EventGetFixedRoomLoot;
        }
    }

    /// <summary>
    /// Sets fixed loot on this room and all room's doors icons.
    /// </summary>
    private void EventGetFixedRoomLoot()
    {
        int randomNumber = UnityEngine.Random.Range(0, 101);
        if (randomNumber < 50)
        {
            lootTypeOnCurrentRoom = LootType.PassiveOrb;
        }
        else
        {
            lootTypeOnCurrentRoom = LootType.UnknownSpell;
        }

        foreach (DoorIconReward door in GetComponentsInChildren<DoorIconReward>(true))
        {
            if (door != null)
                door.UpdateIcon(lootTypeOnCurrentRoom);
        }

        LevelGenerator.EndedGeneration -= EventGetFixedRoomLoot;
    }
    

    protected virtual void Awake()
    {
        stpData = FindObjectOfType<CharacterSaveDataController>();
        runSaveData = FindObjectOfType<RunSaveDataController>();

        if (testRoomIsolatedScene) 
            GetComponentInChildren<RoomOcclusion>().gameObject.SetActive(false);

        // Gets enemies for easy floors, medium floors or hard floors,
        // depending on the current floor saved on save data.
        FloorFormation[] floorFormations = GetComponentsInChildren<FloorFormation>();
        
        if (floorFormations != null && floorFormations.Length > 0)
        {
            int currentFloor;

            if (testRoomIsolatedScene == false)
                currentFloor = floorToTestOnIsolatedScenes;
            else
                currentFloor = runSaveData.SaveData.DungeonSavedData.Floor;
            
            for (int i = 0; i < floorFormations.Length; i++)
            {
                if (currentFloor > 6)
                {
                    if (floorFormations[i].FloorFormationType ==
                        FloorFormationType.SeventhNinethFloors)
                    {
                        enemySpawnPoints =
                            floorFormations[i].GetComponentsInChildren<EnemySpawnPoint>(true);
                        continue;
                    }
                }
                else if (currentFloor > 3)
                {
                    if (floorFormations[i].FloorFormationType ==
                        FloorFormationType.ForthSixthFloors)
                    {
                        enemySpawnPoints =
                            floorFormations[i].GetComponentsInChildren<EnemySpawnPoint>(true);
                        continue;
                    }
                }
                else if (currentFloor > 0)
                {
                    if (floorFormations[i].FloorFormationType ==
                        FloorFormationType.FirstThirdFloors)
                    {
                        enemySpawnPoints =
                            floorFormations[i].GetComponentsInChildren<EnemySpawnPoint>(true);
                        continue;
                    }
                }

                // Destroys the formations that are not part of this floor
                Destroy(floorFormations[i].gameObject);
                continue;
            }
        }

        contactPointsDoors = GetComponentsInChildren<ContactPointDoor>();
        droppedLoot = new List<(LootType, Vector3)>();
        GameProgressCollider[] gameProgressColliders = 
            GetComponentsInChildren<GameProgressCollider>(true);
        exitBlockers = new List<BoxCollider>();
        foreach (GameProgressCollider gpc in gameProgressColliders)
            exitBlockers.Add(gpc.GetComponent<BoxCollider>());
        random = new System.Random();
        playerInCombat = false;
        hasSpawnedSecondWave = false;
        spawnedFirstWaveEnemies = new List<GameObject>();
        spawnedSecondWaveEnemies = new List<GameObject>();

        // For testing in isolated scenes
        if (testRoomIsolatedScene)
        {
            EventSpawnEnemies();
            SpawnEnemies();
        }
    }


    private void OnDisable()
    {
        if (LevelGenerator != null)
        {
            LevelGenerator.EndedGeneration -= EventSpawnEnemies;
        }
    }
        

    /// <summary>
    /// Creates all enemies and disables them.
    /// </summary>
    private void EventSpawnEnemies()
    {
        // If the list is empty, ignores the rest
        if (enemySpawnPoints == null || enemySpawnPoints.Count == 0)
            return;

        GameObject levelParent = 
            GameObject.FindGameObjectWithTag("LevelParent");

        RunSaveDataController runData = FindObjectOfType<RunSaveDataController>();

        DifficultyType dificulty = DifficultyType.Normal;
        if (runData != null)
            Enum.TryParse(runData.SaveData.Difficulty, out dificulty);

        foreach (EnemySpawnPoint enemySpawnPoint in enemySpawnPoints)
        {
            GameObject spawnedEnemy = Instantiate(
                listOfEnemies.SpawnEnemy(
                    enemySpawnPoint.PointInformation[dificulty]),
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

            if (levelParent != null)
                spawnedEnemy.transform.SetParent(levelParent.transform);

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

                // Spawns first wave
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
            }

            haveEnemiesSpawned = true;

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
    }

    /// <summary>
    /// Second wave of enemies.
    /// </summary>
    /// <returns>Waits for seconds</returns>
    private IEnumerator SpawnSecondWaveEnemies()
    {
        yield return new WaitForSeconds(timeToSpawnSecondWave);

        if (hasSpawnedSecondWave == false)
        {
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

                PlayerInCombat = true;
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
                        // Procedural gen rooms drop gold; Boss raid rooms drop arcane power
                        if (currency.CurrencyType == CurrencyType.Gold ||
                            currency.CurrencyType == CurrencyType.ArcanePower)
                            currency.Amount = goldQuantity * 
                                GOLDFLOORMULTIPLIERS[
                                    runSaveData.SaveData.DungeonSavedData.Floor - 1];

                        // ARCANE POWER DROPS ON ENEMIES FOR NOW, UNCOMMENT THIS LATER IF NEEDED
                        //else
                        //    currency.Amount = arcanePowerQuantity;
                    }
                }

                // Spawns fixed item of this room
                SpawnFixedRoomItem();
            }
        }
    }

    /// <summary>
    /// Spawns the fixed loot of this room.
    /// </summary>
    protected void SpawnFixedRoomItem()
    {
        if (lootTypeOnCurrentRoom == LootType.PassiveOrb)
        {
            ItemLootPoolCreator.Pool.InstantiateFromPool(
                LootType.PassiveOrb.ToString(),
                lootSpawnPosition.position,
                Quaternion.identity);
        }
        else
        {
            ItemLootPoolCreator.Pool.InstantiateFromPool(
                LootType.UnknownSpell.ToString(),
                lootSpawnPosition.position,
                Quaternion.identity);
        }

        foreach (DoorIconReward door in GetComponentsInChildren<DoorIconReward>(true))
        {
            door.DisableObject();
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

        // Has second wave
        if (spawnsSecondWave)
        {
            // If second wave hasn't spawned yet
            if (hasSpawnedSecondWave == false)
            {
                // If there is a second wave and all enemies are dead
                if (quantityOfEnemiesSpawned == 0)
                {
                    // Spawns second wave
                    foreach (GameObject spawnedEnemy in spawnedSecondWaveEnemies)
                    {
                        spawnedEnemy.SetActive(true);
                        quantityOfEnemiesSpawned++;
                        if (spawnedEnemy.TryGetComponentInChildrenFirstGen(
                            out Stats spawnedenemyStats))
                        {
                            spawnedenemyStats.EventDeath += EnemyDeath;

                            if (spawnedenemyStats.CommonAttributes.Type == 
                                CharacterType.Boss)
                            {
                                spawnedenemyStats.EventDeath += BossDeath;
                            }
                        }
                    }
                    hasSpawnedSecondWave = true;
                }
                
                // Ignores the rest of the code
                return;
            }
        }

        // If all enemies are dead and second wave has already spawned
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
