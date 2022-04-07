/// <summary>
/// Class responsible for saving run data.
/// </summary>
public class RunSaveData: AbstractSaveData
{
    // Public fields for JSON
    public PlayerSaveData PlayerSavedData;
    public DungeonSaveData DungeonSavedData;
    public AchievementsSaveData AchievementsSaveData;
    public string Difficulty;

    public RunSaveData()
    {
        PlayerSavedData = new PlayerSaveData();
        DungeonSavedData = new DungeonSaveData();
        AchievementsSaveData = new AchievementsSaveData();
    }
}
