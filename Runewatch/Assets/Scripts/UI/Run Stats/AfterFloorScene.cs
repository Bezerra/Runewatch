using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class responsible for saving file after floor is finished.
/// </summary>
public class AfterFloorScene : MonoBehaviour
{
    [Header("Button for scenes after floor only")]
    [SerializeField] private Button button;
    [SerializeField] private RunStatsLogicSO runStatsLogic;

    public void ExecuteButtonClick()
    {
        if (button != null)
        {
            button.onClick.Invoke();
        }
    }   

    /// <summary>
    /// Executes on continue button in run stats screen. Saves progress variables.
    /// </summary>
    public void OnContinueButton()
    {
        RunSaveDataController runSaveDataController = FindObjectOfType<RunSaveDataController>();

        /* ///*  TO TEST STUFF, GOES FROM FLOOR 1 TO 9 //////////////////
        if (runSaveDataController.SaveData.DungeonSavedData.Floor < 9)
            runSaveDataController.SaveData.DungeonSavedData.Floor = 9;
        else
            runSaveDataController.SaveData.DungeonSavedData.Floor += 1;
        */ ////////////////////////////////////////////////////////

        // If the player reaches run stats screen and presses continue
        // to load the next floor, it will increment the floor level
        runSaveDataController.SaveData.DungeonSavedData.Floor += 1;

        runStatsLogic.TriggerRunStats(RunStatsType.Accuracy);

        // And only after incrementing floor, then it will call ISaveables

        // Every class that implements ISaveable, will save its values
        // This will save everything, the current elements we want to save in here are
        // floor level, and all player information during the run (stats, passives, spells),
        // achievements
        runSaveDataController.Save();

        // Saves achievements
        runSaveDataController.SaveAchievements();
    }
}
