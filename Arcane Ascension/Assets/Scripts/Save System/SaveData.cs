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

    private readonly int ENCRYPTATIONKEY = 777;

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
        EncryptDecrypt(JsonUtility.ToJson(this), ENCRYPTATIONKEY);

    /// <summary>
    /// Converts json values to this class.
    /// </summary>
    /// <param name="json">Json.</param>
    public void LoadFromJson(string json) =>
        JsonUtility.FromJsonOverwrite(EncryptDecrypt(json, ENCRYPTATIONKEY), this);

    /// <summary>
    /// Encrypts or decrypts a string
    /// </summary>
    /// <param name="data">String to encrypt or decrypt.</param>
    /// <param name="key">Key of encryption.</param>
    /// <returns>Encrypted/Decrypted string.</returns>
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
