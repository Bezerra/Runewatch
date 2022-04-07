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
    private float damageDone;
    private float damageTaken;
    private float[] runTime;

    private RunSaveDataController runSaveData;

    public void TriggerAchievement(AchievementType achievementType)
    {
        switch(achievementType)
        {
            case AchievementType.EnemiesKilled:
                enemiesKilled++;
                break;
        }
    }

    public void SaveAchievements()
    {
        runSaveData.SaveData.AchievmentsSaveData.EnemiesKilled = enemiesKilled;

        Debug.Log("SAVE GAME " + enemiesKilled);
    }

    public void LoadAchievements(RunSaveDataController runSaveData)
    {
        // Resets variables
        enemiesKilled = 0;
        arcanePowerObtained = 0;
        damageDone = 0;
        damageTaken = 0;

        // Gets run save data
        this.runSaveData = runSaveData;

        enemiesKilled = runSaveData.SaveData.AchievmentsSaveData.EnemiesKilled;

        Debug.Log("LOAD GAME " + enemiesKilled);
    }
}
