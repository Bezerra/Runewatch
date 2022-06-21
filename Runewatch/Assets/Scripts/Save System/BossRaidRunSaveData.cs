/// <summary>
/// Class responsible for saving run data.
/// </summary>
public class BossRaidRunSaveData : AbstractSaveData
{
    // Public fields for JSON
    public BossRaidSaveData PlayerSavedData;

    public BossRaidRunSaveData()
    {
        PlayerSavedData = new BossRaidSaveData();
    }
}
