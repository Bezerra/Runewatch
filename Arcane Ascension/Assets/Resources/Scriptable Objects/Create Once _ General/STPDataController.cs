using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Executes skill tree passives effects.
/// </summary>
public class STPDataController : MonoBehaviour
{
    [SerializeField] private AllSkillTreePassivesSO skillTreePassives;

    private CharacterSaveDataController characterSaveDataController;
    public CharacterSaveData Data => characterSaveDataController.SaveData;

    private void Awake()
    {
        characterSaveDataController = FindObjectOfType<CharacterSaveDataController>();
        skillTreePassives.UpdateID();
    }

    /// <summary>
    /// Runs when the game starts / continue. Will only run once per game session.
    /// </summary>
    public void Execute()
    {
        // Creates list with empty passives
        IList<byte> currentPassives = new List<byte>();

        // Adds saved passves to current passives
        CharacterSaveData saveData = CharacterSaveDataController.LoadGame();
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
