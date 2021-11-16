using System.Text;
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
        EncryptDecrypt(JsonUtility.ToJson(this), 5);

    /// <summary>
    /// Converts json to this class.
    /// </summary>
    /// <param name="json">Json.</param>
    public void LoadFromJson(string json)
    {
        string loadedData = EncryptDecrypt(JsonUtility.FromJson<string>(json), 5);
        JsonUtility.FromJsonOverwrite(loadedData, this);
    }

    private string EncryptDecrypt(string data, int key)
    {
        StringBuilder input = new StringBuilder(data);
        StringBuilder output = new StringBuilder(data.Length);

        char character;

        for (int i = 0; i < data.Length; i++)
        {
            character = input[i];
            character = (char)(character ^ key);
            output.Append(character);
        }

        return output.ToString();
    }
}
