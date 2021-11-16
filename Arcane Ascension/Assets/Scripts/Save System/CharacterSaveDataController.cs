using UnityEngine;

/// <summary>
/// Class responsible for executing character save data save and load.
/// </summary>
public class CharacterSaveDataController : MonoBehaviour
{
    // Basic singleton so save and load methods can be used anywhere.
    private static CharacterSaveDataController instance;

    private CharacterSaveData saveData;
    private FileManager fileManager;

    private void Awake()
    {
        instance = this;
        saveData = new CharacterSaveData();
        fileManager = new FileManager();
    }

    /// <summary>
    /// Makes every class that implements ISaveable save their stats.
    /// </summary>
    /// <param name="currentSkillTreePassives">Current skill tree passives.</param>
    /// <param name="arcanePower">Current arcane power.</param>
    public static void SaveGame(byte[] currentSkillTreePassives, int arcanePower)
    {
        instance.saveData.CurrentSkillTreePassives = currentSkillTreePassives;
        instance.saveData.ArcanePower = arcanePower;

        // Writes file with saved JSON
        if (instance.fileManager.WriteToFile("SAVECHARACTERPROGRESS.d4", instance.saveData.ToJson()))
            Debug.Log("Character Data Saved");
    }

    /// <summary>
    /// Loads a JSON with all saved data.
    /// Makes every class that implements ISaveable load their stats.
    /// </summary>
    public static CharacterSaveData LoadGame()
    {
        if (instance.fileManager.ReadFile("SAVECHARACTERPROGRESS.d4", out string json))
        {
            instance.saveData.LoadFromJson(json);
            Debug.Log("Character Data Loaded");
            return instance.saveData;
        }
        else
        {
            Debug.Log("No character save found");
            return null;
        }
    }
}
