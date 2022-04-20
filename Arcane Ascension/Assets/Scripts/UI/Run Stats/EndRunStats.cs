using UnityEngine;
using TMPro;
using System.Text;

/// <summary>
/// Class responsible for updating end run stats screen text.
/// </summary>
public class EndRunStats : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI enemiesKilled;
    [SerializeField] private TextMeshProUGUI arcanePowerObtained;
    [SerializeField] private TextMeshProUGUI goldObtained;
    [SerializeField] private TextMeshProUGUI damageDone;
    [SerializeField] private TextMeshProUGUI damageTaken;
    [SerializeField] private TextMeshProUGUI mostDamageDone;
    [SerializeField] private TextMeshProUGUI runTime;
    [SerializeField] private TextMeshProUGUI bestRunTime;

    private RunSaveDataController runSaveData;
    private CharacterSaveDataController characterSaveData;

    private void Awake()
    {
        runSaveData = FindObjectOfType<RunSaveDataController>();
        characterSaveData = FindObjectOfType<CharacterSaveDataController>();
    }

    private void Start()
    {
        // Update stats text
        enemiesKilled.text = 
            runSaveData.SaveData.AchievementsSaveData.EnemiesKilled.ToString();

        arcanePowerObtained.text = 
            runSaveData.SaveData.AchievementsSaveData.ArcanePowerObtained.ToString();

        goldObtained.text = 
            runSaveData.SaveData.AchievementsSaveData.GoldObtained.ToString();

        damageDone.text = 
            runSaveData.SaveData.AchievementsSaveData.DamageDone.ToString();

        mostDamageDone.text = 
            runSaveData.SaveData.AchievementsSaveData.MostDamageDone.ToString();

        damageTaken.text = 
            runSaveData.SaveData.AchievementsSaveData.DamageTaken.ToString();

        int[] val = runSaveData.SaveData.AchievementsSaveData.RunTime;
        StringBuilder runVal = new StringBuilder();
        runVal.Append(val[0]);
        runVal.Append(":");
        runVal.Append(val[1]);
        runVal.Append(":");
        runVal.Append(val[2]);
        runTime.text = runVal.ToString();

        int[] val2 = characterSaveData.SaveData.BestRunTime;
        if (val2 != null && val2.Length > 0)
        {
            StringBuilder bestRunVal = new StringBuilder();
            bestRunVal.Append(val2[0]);
            bestRunVal.Append(":");
            bestRunVal.Append(val2[1]);
            bestRunVal.Append(":");
            bestRunVal.Append(val2[2]);
            bestRunTime.text = bestRunVal.ToString();
        }
        else
        {
            bestRunTime.text = runTime.text;
        }


        // Deletes run progress file
        RunSaveDataController runSaveDataController =
            FindObjectOfType<RunSaveDataController>();
        //runSaveDataController.DeleteFile();
    }
}
