using UnityEngine;

/// <summary>
/// Class responsible for saving data.
/// </summary>
public class SaveData
{
    // Public fields for JSON
    public PlayerSaveData PlayerSavedData;
    public DungeonSaveData DungeonSavedData;

    public SaveData()
    {
        PlayerSavedData = new PlayerSaveData();
        DungeonSavedData = new DungeonSaveData();
    }

    /// <summary>
    /// Converts class to json.
    /// </summary>
    /// <returns>String.</returns>
    public string ToJson() =>
        JsonUtility.ToJson(this);

    /// <summary>
    /// Converts json to this class.
    /// </summary>
    /// <param name="json">Json.</param>
    public void LoadFromJson(string json) =>
        JsonUtility.FromJsonOverwrite(json, this);
}
