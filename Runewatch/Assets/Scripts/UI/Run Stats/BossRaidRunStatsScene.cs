using UnityEngine;

/// <summary>
/// Class responsible for controlling run stats scene button methods.
/// </summary>
public class BossRaidRunStatsScene : MonoBehaviour
{
    /// <summary>
    /// Executes on continue button in run stats screen. Saves progress variables.
    /// </summary>
    public void OnContinueButton()
    {
        BossRaidRunSaveDataController runSaveDataController = 
            FindObjectOfType<BossRaidRunSaveDataController>();

        // Every class that implements ISaveable, will save its values
        // This will save everything, the current elements we want to save in here are
        // floor lever, and all player information during the run (stats, passives, spells)
        runSaveDataController?.Save();
    }
}
