using UnityEngine;
using Sirenix.OdinInspector;

#pragma warning disable 0414 // variable assigned but not used.

/// <summary>
/// Scriptable object with achievements logic.
/// </summary>
[InlineEditor]
[CreateAssetMenu(menuName = "Create Once/Achievements",
    fileName = "Achievements Logic")]
public class AchievementLogicSO : ScriptableObject, IAchievementLogic
{
    private int enemiesKilled;
    private int arcanePowerObtained;
    private int goldObtained;
    private int damageDone;
    private int damageTaken;
    private int mostDamageDone;
    private int[] runTime;
    private int[] bestRunTime;
    private int bestRunTimeInSeconds;

    private RunSaveDataController runSaveData;
    private CharacterSaveDataController characterSaveData;

    public void TriggerAchievement(AchievementType achievementType, int value = 0, int[] valueArray = null)
    {
        switch(achievementType)
        {
            case AchievementType.EnemiesKilled:
                enemiesKilled++;
                break;
            case AchievementType.ArcanePowerObtained:
                arcanePowerObtained += (int)value;
                break;
            case AchievementType.GoldObtained:
                goldObtained += (int)value;
                break;
            case AchievementType.DamageDone:
                damageDone += value;
                break;
            case AchievementType.MostDamageDone:
                if (value > mostDamageDone) 
                    mostDamageDone = value;
                break;
            case AchievementType.DamageTaken:
                damageTaken += value;
                break;
            case AchievementType.RunTime:
                runTime = valueArray;
                if (value > bestRunTimeInSeconds)
                {
                    bestRunTime = valueArray;
                    bestRunTimeInSeconds = value;
                }
                break;
        }
    }

    /// <summary>
    /// Saves all achievements on run save data. This method is called after a run is over.
    /// </summary>
    public void SaveAchievements()
    {
        // Run save data is saved on its class
        runSaveData.SaveData.AchievementsSaveData.EnemiesKilled = enemiesKilled;
        runSaveData.SaveData.AchievementsSaveData.ArcanePowerObtained = arcanePowerObtained;
        runSaveData.SaveData.AchievementsSaveData.GoldObtained = goldObtained;
        runSaveData.SaveData.AchievementsSaveData.DamageDone = damageDone;
        runSaveData.SaveData.AchievementsSaveData.MostDamageDone = mostDamageDone;
        runSaveData.SaveData.AchievementsSaveData.DamageTaken = damageTaken;
        runSaveData.SaveData.AchievementsSaveData.RunTime = runTime;

        // Character save data is saved in here
        characterSaveData.SaveData.BestRunTimeInSeconds = bestRunTimeInSeconds;
        characterSaveData.SaveData.BestRunTime = bestRunTime;
        characterSaveData.SaveAchievements();
    }

    /// <summary>
    /// Loads current achievements from a save file to this scriptable object.
    /// </summary>
    /// <param name="runSaveData">Run save data.</param>
    public void LoadRunAchievements(RunSaveDataController runSaveData)
    {
        // Resets variables
        enemiesKilled = 0;
        arcanePowerObtained = 0;
        goldObtained = 0;
        damageDone = 0;
        mostDamageDone = 0;
        damageTaken = 0;
        runTime = null;

        // Gets run save data every time the game loads
        this.runSaveData = runSaveData;

        enemiesKilled = runSaveData.SaveData.AchievementsSaveData.EnemiesKilled;
        arcanePowerObtained = runSaveData.SaveData.AchievementsSaveData.ArcanePowerObtained;
        goldObtained = runSaveData.SaveData.AchievementsSaveData.GoldObtained;
        damageDone = runSaveData.SaveData.AchievementsSaveData.DamageDone;
        mostDamageDone = runSaveData.SaveData.AchievementsSaveData.MostDamageDone;
        damageTaken = runSaveData.SaveData.AchievementsSaveData.DamageTaken;
        runTime = runSaveData.SaveData.AchievementsSaveData.RunTime;
    }

    /// <summary>
    /// Loads current achievements from a save file to this scriptable object.
    /// </summary>
    /// <param name="runSaveData">Character save data.</param>
    public void LoadCharacterAchievements(CharacterSaveDataController characterSaveData)
    {
        // Resets variables
        bestRunTimeInSeconds = 0;
        bestRunTime = null;

        // Gets character save data every time the game loads
        this.characterSaveData = characterSaveData;

        bestRunTimeInSeconds = characterSaveData.SaveData.BestRunTimeInSeconds;
        bestRunTime = characterSaveData.SaveData.BestRunTime;

        Debug.Log(bestRunTimeInSeconds);
    }
}
