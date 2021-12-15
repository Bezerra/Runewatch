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

    private void Start()
    {
        if (saveData.DungeonSavedData.Floor % 3 == 0)
        {
            BossRoom();
        }
        else
        {
            ChangeFloorRoom();
        }
    }

    /// <summary>
    /// If it's a boss room, destroys portal stuff.
    /// </summary>
    private void BossRoom()
    {
        NextFloorPortalSpawnPoint[] nextFloorPortalSpawnPoints = GetComponentsInChildren<NextFloorPortalSpawnPoint>();
        if (nextFloorPortalSpawnPoints.Length > 0)
        {
            foreach (NextFloorPortalSpawnPoint nextFloorPortalSpawnPoint in nextFloorPortalSpawnPoints)
            {
                Destroy(nextFloorPortalSpawnPoint.gameObject);
            }
        }
    }

    /// <summary>
    /// If it's a portal room, destroys boss stuff.
    /// </summary>
    private void ChangeFloorRoom()
    {
        EnemySpawnPoint[] enemySpawns = GetComponentsInChildren<EnemySpawnPoint>();
        if (enemySpawns.Length > 0)
        {
            foreach(EnemySpawnPoint enemySpawn in enemySpawns)
            {
                Destroy(enemySpawn.gameObject);
            }
        }
    }
}
