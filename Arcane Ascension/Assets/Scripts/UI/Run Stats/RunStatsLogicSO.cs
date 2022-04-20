using UnityEngine;
using Sirenix.OdinInspector;

#pragma warning disable 0414 // variable assigned but not used.

/// <summary>
/// Scriptable object with run stats logic.
/// </summary>
[InlineEditor]
[CreateAssetMenu(menuName = "Create Once/Run Stats",
    fileName = "Run Stats Logic")]
public class RunStatsLogicSO : ScriptableObject, IRunStatsLogic
{
    private int enemiesKilled;
    private int arcanePowerObtained;
    private int goldObtained;
    private int damageDone;
    private int damageTaken;
    private int mostDamageDone;
    private int runTime;
    private int bestRunTime;
    public int BestRunTime => bestRunTime;

    private RunSaveDataController runSaveData;
    private CharacterSaveDataController characterSaveData;

    public void TriggerRunStats(RunStatsType achievementType, int value = 0)
    {
        if (characterSaveData == null)
            characterSaveData = FindObjectOfType<CharacterSaveDataController>(); 

        if (runSaveData == null)
            runSaveData = FindObjectOfType<RunSaveDataController>();

        if (runSaveData == null) return;

        switch(achievementType)
        {
            case RunStatsType.EnemiesKilled:
                enemiesKilled++;
                break;
            case RunStatsType.ArcanePowerObtained:
                arcanePowerObtained += (int)value;
                break;
            case RunStatsType.GoldObtained:
                goldObtained += (int)value;
                break;
            case RunStatsType.DamageDone:
                damageDone += value;
                break;
            case RunStatsType.MostDamageDone:
                if (value > mostDamageDone) 
                    mostDamageDone = value;
                break;
            case RunStatsType.DamageTaken:
                damageTaken += value;
                break;
            case RunStatsType.RunTime:
                runTime = value;

                if (runSaveData.SaveData.DungeonSavedData.Floor == 9 &&
                    runTime < bestRunTime)
                {
                    bestRunTime = value;
                    SaveTimerAchievement();
                }
                break;
        }
    }

    public void SaveTimerAchievement()
    {
        characterSaveData.SaveData.BestRunTime = bestRunTime;
        characterSaveData.SaveAchievements();
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
        runTime = 0;

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
        // Gets character save data every time the game loads
        this.characterSaveData = characterSaveData;

        bestRunTime = characterSaveData.SaveData.BestRunTime;
    }
}
