using UnityEngine;

/// <summary>
/// Class responsible for controlling run stats scene button methods.
/// </summary>
public class RunStatsScene : MonoBehaviour
{
    public void OnContinueButton()
    {
        RunSaveDataController runSaveDataController = FindObjectOfType<RunSaveDataController>();
        runSaveDataController.SaveData.Floor += 1;
        runSaveDataController.Save();
    }
}
