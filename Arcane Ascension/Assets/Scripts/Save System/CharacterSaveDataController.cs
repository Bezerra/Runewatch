using UnityEngine;

/// <summary>
/// Class responsible for executing character save data save and load.
/// </summary>
public class CharacterSaveDataController : MonoBehaviour
{
    // Basic singleton so save and load methods can be used anywhere.
    private static CharacterSaveDataController instance;

    public CharacterSaveData SaveData { get; private set; }
    private FileManager fileManager;

    private void Awake()
    {
        instance = this;
        SaveData = new CharacterSaveData();
        fileManager = new FileManager();
    }

    /// <summary>
    /// Saves current passives byte list and arcane power to a file.
    /// </summary>
    /// <param name="currentSkillTreePassives">Current skill tree passives.</param>
    /// <param name="arcanePower">Current arcane power.</param>
    public static void SaveGame(byte[] currentSkillTreePassives, int arcanePower)
    {
        instance.SaveData.CurrentSkillTreePassives = currentSkillTreePassives;
        instance.SaveData.ArcanePower = arcanePower;

        // Writes file with saved JSON
        instance.Save();
    }

    /// <summary>
    /// Makes every class that implements ISaveable save their stats.
    /// </summary>
    /// <param name="passiveType">Current skill tree passives.</param>
    /// <param name="amountToAdd">Current arcane power.</param>
    public void WriteInformation(SkillTreePassiveType passiveType, byte amountToAdd)
    {
        switch(passiveType)
        {
            case SkillTreePassiveType.DefaultSpell:
                instance.SaveData.SkillTreeSaveData.DefaultSpell = amountToAdd;
                break;

            case SkillTreePassiveType.Vitality:
                instance.SaveData.SkillTreeSaveData.MaximumHealth = amountToAdd;
                break;
        }
    }

    /// <summary>
    /// Writes file with saved JSON.
    /// </summary>
    public void Save()
    {
        // Writes file with saved JSON
        if (fileManager.WriteToFile("SAVECHARACTERPROGRESS.d4", instance.SaveData.ToJson()))
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
            instance.SaveData.LoadFromJson(json);
            Debug.Log("Character Data Loaded");
            return instance.SaveData;
        }
        else
        {
            Debug.Log("No character save found");
            return null;
        }
    }
}
