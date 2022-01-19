using UnityEngine;

/// <summary>
/// Class responsible for executing character save data save and load.
/// </summary>
public class CharacterSaveDataController : MonoBehaviour
{
    public CharacterSaveData SaveData { get; private set; }
    private FileManager fileManager;

    /// <summary>
    /// THIS SCRIPT EXECUTION ORDER RUNS BEFORE EVERY OTHER SCRIPT,
    /// in order to avoid nulls if file don't exist.
    /// </summary>
    private void Awake()
    {
        SaveData = new CharacterSaveData();
        SaveData = LoadGame();
        fileManager = new FileManager();
    }

    /// <summary>
    /// Writes file with saved JSON.
    /// </summary>
    public void Save()
    {
        // Writes file with saved JSON
        if (fileManager.WriteToFile("CHARACTERFILE.d4s", SaveData.ToJson()))
            Debug.Log("Character Data Saved");
    }

    /// <summary>
    /// Saves current passives byte list and arcane power to a file.
    /// </summary>
    /// <param name="currentSkillTreePassives">Current skill tree passives.</param>
    /// <param name="arcanePower">Current arcane power.</param>
    public void Save(byte[] currentSkillTreePassives, int arcanePower)
    {
        SaveData.CurrentSkillTreePassives = currentSkillTreePassives;
        SaveData.ArcanePower = arcanePower;

        // Writes file with saved JSON
        Save();
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
            case SkillTreePassiveType.IgnisExpertise:
                SaveData.IgnisExpertise = amountToAdd;
                break;
            case SkillTreePassiveType.FulgurExpertise:
                SaveData.FulgurExpertise = amountToAdd;
                break;
            case SkillTreePassiveType.AquaExpertise:
                SaveData.AquaExpertise = amountToAdd;
                break;
            case SkillTreePassiveType.TerraExpertise:
                SaveData.TerraExpertise = amountToAdd;
                break;
            case SkillTreePassiveType.NaturaExpertise:
                SaveData.NaturaExpertise = amountToAdd;
                break;
            case SkillTreePassiveType.LuxExpertise:
                SaveData.LuxExpertise = amountToAdd;
                break;
            case SkillTreePassiveType.UmbraExpertise:
                SaveData.UmbraExpertise = amountToAdd;
                break;
            case SkillTreePassiveType.Vitality:
                SaveData.Vitality = amountToAdd;
                break;
            case SkillTreePassiveType.Insight:
                SaveData.Insight = amountToAdd;
                break;
            case SkillTreePassiveType.Meditation:
                SaveData.Meditation = amountToAdd;
                break;
            case SkillTreePassiveType.Agility:
                SaveData.Agility = amountToAdd;
                break;
            case SkillTreePassiveType.Luck:
                SaveData.Luck = amountToAdd;
                break;
            case SkillTreePassiveType.Precision:
                SaveData.Precision = amountToAdd;
                break;
            case SkillTreePassiveType.Overpowering:
                SaveData.Overpowering = amountToAdd;
                break;
            case SkillTreePassiveType.Resilience:
                SaveData.Resilience = amountToAdd;
                break;
            case SkillTreePassiveType.FleetingForm:
                SaveData.FleetingForm = amountToAdd;
                break;
            case SkillTreePassiveType.ManaFountain:
                SaveData.ManaFountain = amountToAdd;
                break;
            case SkillTreePassiveType.LifeSteal:
                SaveData.LifeSteal = amountToAdd;
                break;
            case SkillTreePassiveType.Wealth:
                SaveData.Wealth = amountToAdd;
                break;
            case SkillTreePassiveType.Pickpocket:
                SaveData.Pickpocket = amountToAdd;
                break;
            case SkillTreePassiveType.Dealer:
                SaveData.Dealer = amountToAdd;
                break;
            case SkillTreePassiveType.MasteryOfTheArts:
                SaveData.MasteryOfTheArts = amountToAdd;
                break;
            case SkillTreePassiveType.Destiny:
                SaveData.Destiny = amountToAdd;
                break;
            case SkillTreePassiveType.Reaper:
                SaveData.Reaper = amountToAdd;
                break;
            case SkillTreePassiveType.Healer:
                SaveData.Healer = amountToAdd;
                break;
            case SkillTreePassiveType.DefaultSpell:
                SaveData.DefaultSpell = amountToAdd;
                break;
        }
    }

    /// <summary>
    /// Loads a JSON with all saved data.
    /// Makes every class that implements ISaveable load their stats.
    /// </summary>
    public CharacterSaveData LoadGame()
    {
        if (fileManager.ReadFile("CHARACTERFILE.d4s", out string json))
        {
            SaveData.LoadFromJson(json);
            Debug.Log("Character Data Loaded");
            return SaveData;
        }
        else
        {
            if (fileManager.WriteToFile("CHARACTERFILE.d4s", SaveData.ToJson()))
                Debug.Log("No character save found. Created new file.");
            SaveData.LoadFromJson(json);
            return SaveData;
        }
    }

    /// <summary>
    /// Deletes run progress fgile.
    /// </summary>
    public void DeleteFile()
    {
        fileManager.DeleteFile("CHARACTERFILE.d4s");
        SaveData = new CharacterSaveData();
    }

    /// <summary>
    /// Checks if file exists
    /// </summary>
    /// <returns>True if the file exists.</returns>
    public bool FileExists()
    {
        if (fileManager.FileExists("CHARACTERFILE.d4s"))
            return true;
        return false;
    }
}
