/// <summary>
/// Class responsible for controlling what happens inside a normal level piece room.
/// </summary>
public class LevelPieceGameProgressControlFinalRoom : LevelPieceGameProgressControlAbstract
{
    private RunSaveData saveData;

    protected override void Awake()
    {
        base.Awake();
        saveData = FindObjectOfType<RunSaveDataController>().SaveData;
    }

    protected override void Start()
    {
        if (saveData.DungeonSavedData.Floor % 3 == 0)
        {
            BossRoom();
        }
        else
        {
            ChangeFloorRoom();
        }

        // Base start happens before initial logic, so if it's not a boss room,
        // it will destroy enemy spawn points first, so base start won't spawn enemies
        base.Start();
    }

    /// <summary>
    /// If it's a boss room, disables portal stuff.
    /// </summary>
    private void BossRoom()
    {
        NextFloorPortalSpawnPoint[] nextFloorPortalSpawnPoints = 
            GetComponentsInChildren<NextFloorPortalSpawnPoint>();

        if (nextFloorPortalSpawnPoints.Length > 0)
        {
            foreach (NextFloorPortalSpawnPoint nextFloorPortalSpawnPoint in nextFloorPortalSpawnPoints)
            {
                nextFloorPortalSpawnPoint.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// If it's a portal room, destroys boss stuff.
    /// </summary>
    private void ChangeFloorRoom()
    {
        if (enemySpawnPoints.Count > 0)
        {
            foreach(EnemySpawnPoint enemySpawn in enemySpawnPoints)
            {
                Destroy(enemySpawn.gameObject);
            }

            enemySpawnPoints = null;
        }
    }
}
