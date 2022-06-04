using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

/// <summary>
/// Class responsible for updating end run stats screen text.
/// </summary>
public class EndRunStats : MonoBehaviour
{
    [SerializeField] private RunStatsLogicSO runStatsLogic;

    [SerializeField] private TextMeshProUGUI enemiesKilled;
    [SerializeField] private TextMeshProUGUI floorReached;
    [SerializeField] private TextMeshProUGUI difficulty;
    [SerializeField] private TextMeshProUGUI arcanePowerObtained;
    [SerializeField] private TextMeshProUGUI goldObtained;
    [SerializeField] private TextMeshProUGUI damageDone;
    [SerializeField] private TextMeshProUGUI damageTaken;
    [SerializeField] private TextMeshProUGUI mostDamageDone;
    [SerializeField] private TextMeshProUGUI accuracy;
    [SerializeField] private TextMeshProUGUI runTime;
    [SerializeField] private TextMeshProUGUI bestRunTime;

    private RunSaveDataController runSaveData;

    /// <summary>
    /// Used by each end run scene animation.
    /// </summary>
    private void Awake()
    {
        runSaveData = FindObjectOfType<RunSaveDataController>();
    }

    /// <summary>
    /// Used by each end run scene animation.
    /// </summary>
    public void UpdateText()
    {
        if (runStatsLogic == null) return;
        if (enemiesKilled == null) return;

        // Update stats text
        enemiesKilled.text = 
            runSaveData.SaveData.AchievementsSaveData.EnemiesKilled.ToString();

        if (floorReached != null)
        {
            floorReached.text =
                runSaveData.SaveData.DungeonSavedData.Floor.ToString();
        }

        difficulty.text =
            runSaveData.SaveData.Difficulty.ToString();

        arcanePowerObtained.text = 
            runSaveData.SaveData.AchievementsSaveData.ArcanePowerObtained.ToString();

        goldObtained.text = 
            runSaveData.SaveData.AchievementsSaveData.GoldObtained.ToString();

        damageDone.text = 
            runSaveData.SaveData.AchievementsSaveData.DamageDone.ToString();

        damageTaken.text =
            runSaveData.SaveData.AchievementsSaveData.DamageTaken.ToString();

        mostDamageDone.text = 
            runSaveData.SaveData.AchievementsSaveData.MostDamageDone.ToString();

        accuracy.text =
            runSaveData.SaveData.AchievementsSaveData.Accuracy.ToString() + " %";

        TimeSpan timeSpan = 
            TimeSpan.FromSeconds(runSaveData.SaveData.AchievementsSaveData.RunTime);
        runTime.text = timeSpan.ToString(@"hh\:mm\:ss");

        // Must use property instead of save file, save file is not updating the correct way
        timeSpan =
            TimeSpan.FromSeconds(runStatsLogic.BestRunTime);
        bestRunTime.text = timeSpan.ToString(@"hh\:mm\:ss");

        // Deletes run progress file
        RunSaveDataController runSaveDataController =
            FindObjectOfType<RunSaveDataController>();
        runSaveDataController.DeleteFile();
    }

    public void SaveAchievementsLastFloor()
    {
        // Ignores if it's boss raid stats
        // (Should remake this part, boss raid shouldn't be using this script)
        if (SceneManager.GetActiveScene().name == SceneEnum.BossRaidStats.ToString())
            return;

        if (runSaveData.SaveData.DungeonSavedData.Floor == 9)
        {
            runSaveData.SaveAchievements();
            runStatsLogic.SaveTimerAchievement();
        }
    }
}
