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
                instance.SaveData.SkillTreeSaveData.IgnisExpertise = amountToAdd;
                break;
            case SkillTreePassiveType.FulgurExpertise:
                instance.SaveData.SkillTreeSaveData.FulgurExpertise = amountToAdd;
                break;
            case SkillTreePassiveType.AquaExpertise:
                instance.SaveData.SkillTreeSaveData.AquaExpertise = amountToAdd;
                break;
            case SkillTreePassiveType.TerraExpertise:
                instance.SaveData.SkillTreeSaveData.TerraExpertise = amountToAdd;
                break;
            case SkillTreePassiveType.NaturaExpertise:
                instance.SaveData.SkillTreeSaveData.NaturaExpertise = amountToAdd;
                break;
            case SkillTreePassiveType.LuxExpertise:
                instance.SaveData.SkillTreeSaveData.LuxExpertise = amountToAdd;
                break;
            case SkillTreePassiveType.UmbraExpertise:
                instance.SaveData.SkillTreeSaveData.UmbraExpertise = amountToAdd;
                break;
            case SkillTreePassiveType.Vitality:
                instance.SaveData.SkillTreeSaveData.Vitality = amountToAdd;
                break;
            case SkillTreePassiveType.Insight:
                instance.SaveData.SkillTreeSaveData.Insight = amountToAdd;
                break;
            case SkillTreePassiveType.Meditation:
                instance.SaveData.SkillTreeSaveData.Meditation = amountToAdd;
                break;
            case SkillTreePassiveType.Agility:
                instance.SaveData.SkillTreeSaveData.Agility = amountToAdd;
                break;
            case SkillTreePassiveType.Luck:
                instance.SaveData.SkillTreeSaveData.Luck = amountToAdd;
                break;
            case SkillTreePassiveType.Precision:
                instance.SaveData.SkillTreeSaveData.Precision = amountToAdd;
                break;
            case SkillTreePassiveType.Overpowering:
                instance.SaveData.SkillTreeSaveData.Overpowering = amountToAdd;
                break;
            case SkillTreePassiveType.Resilience:
                instance.SaveData.SkillTreeSaveData.Resilience = amountToAdd;
                break;
            case SkillTreePassiveType.FleetingForm:
                instance.SaveData.SkillTreeSaveData.FleetingForm = amountToAdd;
                break;
            case SkillTreePassiveType.ManaFountain:
                instance.SaveData.SkillTreeSaveData.ManaFountain = amountToAdd;
                break;
            case SkillTreePassiveType.ArcaneKnowledge:
                instance.SaveData.SkillTreeSaveData.ArcaneKnowledge = amountToAdd;
                break;
            case SkillTreePassiveType.Wealth:
                instance.SaveData.SkillTreeSaveData.Wealth = amountToAdd;
                break;
            case SkillTreePassiveType.Pickpocket:
                instance.SaveData.SkillTreeSaveData.Pickpocket = amountToAdd;
                break;
            case SkillTreePassiveType.Dealer:
                instance.SaveData.SkillTreeSaveData.Dealer = amountToAdd;
                break;
            case SkillTreePassiveType.MasteryOfTheArts:
                instance.SaveData.SkillTreeSaveData.MasteryOfTheArts = amountToAdd;
                break;
            case SkillTreePassiveType.Destiny:
                instance.SaveData.SkillTreeSaveData.Destiny = amountToAdd;
                break;
            case SkillTreePassiveType.Reaper:
                instance.SaveData.SkillTreeSaveData.Reaper = amountToAdd;
                break;
            case SkillTreePassiveType.Healer:
                instance.SaveData.SkillTreeSaveData.Healer = amountToAdd;
                break;
            case SkillTreePassiveType.DefaultSpell:
                instance.SaveData.SkillTreeSaveData.DefaultSpell = amountToAdd;
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
    public CharacterSaveData LoadGame()
    {
        if (instance.fileManager.ReadFile("SAVECHARACTERPROGRESS.d4", out string json))
        {
            SaveData.LoadFromJson(json);
            Debug.Log("Character Data Loaded");
            return SaveData;
        }
        else
        {
            Debug.Log("No character save found");
            return null;
        }
    }
}
