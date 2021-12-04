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

    /// <summary>
    /// THIS SCRIPT EXECUTION ORDER RUNS BEFORE EVERY OTHER SCRIPT,
    /// in order to avoid nulls if file don't exist.
    /// </summary>
    private void Awake()
    {
        instance = this;
        SaveData = new CharacterSaveData();
        fileManager = new FileManager();
        SaveData = LoadGame();
    }

    /// <summary>
    /// Saves current passives byte list and arcane power to a file.
    /// </summary>
    /// <param name="currentSkillTreePassives">Current skill tree passives.</param>
    /// <param name="arcanePower">Current arcane power.</param>
    public void SaveGame(byte[] currentSkillTreePassives, int arcanePower)
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
                instance.SaveData.IgnisExpertise = amountToAdd;
                break;
            case SkillTreePassiveType.FulgurExpertise:
                instance.SaveData.FulgurExpertise = amountToAdd;
                break;
            case SkillTreePassiveType.AquaExpertise:
                instance.SaveData.AquaExpertise = amountToAdd;
                break;
            case SkillTreePassiveType.TerraExpertise:
                instance.SaveData.TerraExpertise = amountToAdd;
                break;
            case SkillTreePassiveType.NaturaExpertise:
                instance.SaveData.NaturaExpertise = amountToAdd;
                break;
            case SkillTreePassiveType.LuxExpertise:
                instance.SaveData.LuxExpertise = amountToAdd;
                break;
            case SkillTreePassiveType.UmbraExpertise:
                instance.SaveData.UmbraExpertise = amountToAdd;
                break;
            case SkillTreePassiveType.Vitality:
                instance.SaveData.Vitality = amountToAdd;
                break;
            case SkillTreePassiveType.Insight:
                instance.SaveData.Insight = amountToAdd;
                break;
            case SkillTreePassiveType.Meditation:
                instance.SaveData.Meditation = amountToAdd;
                break;
            case SkillTreePassiveType.Agility:
                instance.SaveData.Agility = amountToAdd;
                break;
            case SkillTreePassiveType.Luck:
                instance.SaveData.Luck = amountToAdd;
                break;
            case SkillTreePassiveType.Precision:
                instance.SaveData.Precision = amountToAdd;
                break;
            case SkillTreePassiveType.Overpowering:
                instance.SaveData.Overpowering = amountToAdd;
                break;
            case SkillTreePassiveType.Resilience:
                instance.SaveData.Resilience = amountToAdd;
                break;
            case SkillTreePassiveType.FleetingForm:
                instance.SaveData.FleetingForm = amountToAdd;
                break;
            case SkillTreePassiveType.ManaFountain:
                instance.SaveData.ManaFountain = amountToAdd;
                break;
            case SkillTreePassiveType.ArcaneKnowledge:
                instance.SaveData.ArcaneKnowledge = amountToAdd;
                break;
            case SkillTreePassiveType.Wealth:
                instance.SaveData.Wealth = amountToAdd;
                break;
            case SkillTreePassiveType.Pickpocket:
                instance.SaveData.Pickpocket = amountToAdd;
                break;
            case SkillTreePassiveType.Dealer:
                instance.SaveData.Dealer = amountToAdd;
                break;
            case SkillTreePassiveType.MasteryOfTheArts:
                instance.SaveData.MasteryOfTheArts = amountToAdd;
                break;
            case SkillTreePassiveType.Destiny:
                instance.SaveData.Destiny = amountToAdd;
                break;
            case SkillTreePassiveType.Reaper:
                instance.SaveData.Reaper = amountToAdd;
                break;
            case SkillTreePassiveType.Healer:
                instance.SaveData.Healer = amountToAdd;
                break;
            case SkillTreePassiveType.DefaultSpell:
                instance.SaveData.DefaultSpell = amountToAdd;
                break;
        }
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
    /// Loads a JSON with all saved data.
    /// Makes every class that implements ISaveable load their stats.
    /// </summary>
    public CharacterSaveData LoadGame()
    {
        if (instance.fileManager.ReadFile("CHARACTERFILE.d4s", out string json))
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
}
