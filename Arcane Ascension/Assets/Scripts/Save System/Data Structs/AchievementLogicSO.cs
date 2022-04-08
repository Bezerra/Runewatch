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
    private float damageDone;
    private float damageTaken;
    private float mostDamageDone;
    private int[] runTime;

    private RunSaveDataController runSaveData;

    public void TriggerAchievement(AchievementType achievementType, float value = 0, int[] valueArray = null)
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
            case AchievementType.DamageTaken:
                damageTaken += value;
                break;
        }
    }

    /// <summary>
    /// Saves all achievements on run save data.
    /// </summary>
    public void SaveAchievements()
    {
        runSaveData.SaveData.AchievementsSaveData.EnemiesKilled = enemiesKilled;
        runSaveData.SaveData.AchievementsSaveData.ArcanePowerObtained = arcanePowerObtained;
        runSaveData.SaveData.AchievementsSaveData.GoldObtained = goldObtained;
        runSaveData.SaveData.AchievementsSaveData.DamageDone = damageDone;
        runSaveData.SaveData.AchievementsSaveData.DamageTaken = damageTaken;
        runSaveData.SaveData.AchievementsSaveData.RunTime = runTime;
    }

    /// <summary>
    /// Loads current achievements from a save file to this scriptable object.
    /// </summary>
    /// <param name="runSaveData"></param>
    public void LoadAchievements(RunSaveDataController runSaveData)
    {
        // Resets variables
        enemiesKilled = 0;
        arcanePowerObtained = 0;
        goldObtained = 0;
        damageDone = 0;
        damageTaken = 0;
        runTime = new int[3];

        // Gets run save data every time the game loads
        this.runSaveData = runSaveData;

        enemiesKilled = runSaveData.SaveData.AchievementsSaveData.EnemiesKilled;
        arcanePowerObtained = runSaveData.SaveData.AchievementsSaveData.ArcanePowerObtained;
        goldObtained = runSaveData.SaveData.AchievementsSaveData.GoldObtained;
        damageDone = runSaveData.SaveData.AchievementsSaveData.DamageDone;
        damageTaken = runSaveData.SaveData.AchievementsSaveData.DamageTaken;
        runTime = runSaveData.SaveData.AchievementsSaveData.RunTime;
    }
}
