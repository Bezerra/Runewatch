using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for saving each posessed passive effect to a file.
/// </summary>
public class SaveSkillTreePassivesInformation : MonoBehaviour
{
    [SerializeField] private AllSkillTreePassivesSO skillTreePassives;

    private void Awake()
    {
        // Organizes passives by id
        skillTreePassives.UpdateID();
    }

    /// <summary>
    /// Runs when the game starts. Will only run once per game session start.
    /// Called with start game button.
    /// </summary>
    public void SavePassiveEffectsData()
    {
        CharacterSaveDataController characterSaveDataController = 
            FindObjectOfType<CharacterSaveDataController>();

        // Creates list with empty passives
        IList<byte> currentPassives = new List<byte>();

        // Adds saved passves to current passives
        CharacterSaveData saveData = characterSaveDataController.LoadGame();
        if (saveData != null)
        {
            for (int i = 0; i < saveData.CurrentSkillTreePassives.Length; i++)
            {
                currentPassives.Add(saveData.CurrentSkillTreePassives[i]);
            }
        }

        // Foreach current passive the player has, it will write its information to a file and save it
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



        // IN THE END, THROW AN EVENT HERE, TO REMOVE A BLACK SCREEN FROM TOP MAYBE
        // SO ALL UPDATES WILL BE APPLIED ON UI BEFORE THE BLACK SCREEN DISAPPEARS
    }
}
