using UnityEngine;

/// <summary>
/// Class responsible for controlling run stats scene button methods.
/// </summary>
public class RunStatsScene : MonoBehaviour
{
    /// <summary>
    /// Executes on continue button in run stats screen. Saves progress variables.
    /// </summary>
    public void OnContinueButton()
    {
        RunSaveDataController runSaveDataController = FindObjectOfType<RunSaveDataController>();

        // If the player reaches run stats screen and presses continue
        // to load the next floor, it will increment the floor level
        runSaveDataController.SaveData.DungeonSavedData.Floor += 1;

        // And only after incrementing floor, then it will call ISaveables

        // Every class that implements ISaveable, will save its values
        // This will save everything, the current elements we want to save in here are
        // floor lever, and all player information during the run (stats, passives, spells)
        runSaveDataController.Save();
    }
}
