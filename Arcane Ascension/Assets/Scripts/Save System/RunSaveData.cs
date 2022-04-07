/// <summary>
/// Class responsible for saving run data.
/// </summary>
public class RunSaveData: AbstractSaveData
{
    // Public fields for JSON
    public PlayerSaveData PlayerSavedData;
    public DungeonSaveData DungeonSavedData;
    public AchievmentsSaveData AchievmentsSaveData;
    public string Difficulty;

    public RunSaveData()
    {
        PlayerSavedData = new PlayerSaveData();
        DungeonSavedData = new DungeonSaveData();
        AchievmentsSaveData = new AchievmentsSaveData();
    }
}
