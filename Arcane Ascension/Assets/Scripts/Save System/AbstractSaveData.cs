using System.Text;
using UnityEngine;

/// <summary>
/// Class parent of save data classes.
/// </summary>
public abstract class AbstractSaveData
{
    private readonly int ENCRYPTIONKEY = 777;

    /// <summary>
    /// Converts class to json.
    /// </summary>
    /// <returns>String.</returns>
    public string ToJson() =>
        EncryptDecrypt(JsonUtility.ToJson(this), ENCRYPTIONKEY);

    /// <summary>
    /// Converts json values to this class.
    /// </summary>
    /// <param name="json">Json.</param>
    public void LoadFromJson(string json) =>
        JsonUtility.FromJsonOverwrite(EncryptDecrypt(json, ENCRYPTIONKEY), this);

    /// <summary>
    /// Encrypts or decrypts a string
    /// </summary>
    /// <param name="data">String to encrypt or decrypt.</param>
    /// <param name="key">Key of encryption.</param>
    /// <returns>Encrypted/Decrypted string.</returns>
    protected string EncryptDecrypt(string data, int key)
    {
        // REMOVE COMMENTS TO ENCRYPT CODE
        /*
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
        */
        return data;
    }
}
