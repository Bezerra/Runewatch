using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Class responsible for saving each posessed passive effect to a file.
/// </summary>
public class MainMenuButtonsControl : MonoBehaviour
{
    [Header("Skill tree passives scriptable object")]
    [SerializeField] private AllSkillTreePassivesSO skillTreePassives;

    [Header("Buttons")]
    [SerializeField] private Image continueButtonImage;
    [SerializeField] private Button continueButton;

    // Components
    private CharacterSaveDataController characterSaveDataController;
    private RunSaveDataController runSaveDataController;

    private void Awake()
    {
        // Organizes passives by id
        skillTreePassives.UpdateID();
        characterSaveDataController = FindObjectOfType<CharacterSaveDataController>();
        runSaveDataController = FindObjectOfType<RunSaveDataController>();

        Time.timeScale = 1;
    }

    private void OnEnable()
    {
        if (runSaveDataController.FileExists())
        {
            continueButtonImage.enabled = true;
            continueButton.enabled = true;
            continueButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        }
        else
        {
            continueButtonImage.enabled = false;
            continueButton.enabled = false;
            continueButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.gray;
        }
    }

    /// <summary>
    /// Runs when the game starts. Will only run once per game session start.
    /// Called with start game button.
    /// All passives bought will only be applied once this method is called.
    /// </summary>
    public void SaveNewGameInformation(int difficulty)
    {


        // Creates list with empty passives
        IList<byte> currentPassives = new List<byte>();

        // Adds saved passives to current passives
        CharacterSaveData saveData = characterSaveDataController.SaveData;

        // First it adds all skills to a list (to use Contains later)
        if (saveData != null)
        {
            for (int i = 0; i < saveData.CurrentSkillTreePassives.Length; i++)
            {
                currentPassives.Add(saveData.CurrentSkillTreePassives[i]);
            }
        }

        // Foreach current passive the player has, it will write its information to a file and save it.
        // This will update all skill tree passives on that file.
        for (int i = 0; i < skillTreePassives.PassivesList.Count; i++)
        {
            if (currentPassives.Contains(skillTreePassives.PassivesList[i].ID))
            {
                characterSaveDataController.WriteInformation
                    (skillTreePassives.PassivesList[i].PassiveType,
                    skillTreePassives.PassivesList[i].Amount);
            }
        }
        characterSaveDataController.Save();

        // Cleats activated cheats
        PlayerPrefs.SetInt(PPrefsCheats.God.ToString(), 0);
        PlayerPrefs.SetInt(PPrefsCheats.Mana.ToString(), 0);
        PlayerPrefs.SetInt(PPrefsCheats.Fly.ToString(), 0);

        // Sets floor to initial floor
        RunSaveDataController runSaveDataController =
            FindObjectOfType<RunSaveDataController>();
        runSaveDataController.DeleteFile();
        runSaveDataController.SaveData.DungeonSavedData.Floor = 1;
        switch (difficulty)
        {
            case 0:
                runSaveDataController.SaveData.Difficulty = DifficultyType.Normal.ToString();
                break;
            case 1:
                runSaveDataController.SaveData.Difficulty = DifficultyType.Medium.ToString();
                break;
            case 2:
                runSaveDataController.SaveData.Difficulty = DifficultyType.Hard.ToString();
                break;
            case 3:
                runSaveDataController.SaveData.Difficulty = DifficultyType.Extreme.ToString();
                break;
        }
        runSaveDataController.Save();
    }
}
