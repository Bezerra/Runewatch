using UnityEngine;

/// <summary>
/// has methods to control gameplay timer.
/// </summary>
public class ControlGameplayTimer : MonoBehaviour
{
    public void ResetTimer() => GameplayTime.ResetTimer();
    public void PlayTimer() => GameplayTime.PlayTimer();
    public void PauseTimer() => GameplayTime.PauseTimer();
    public void SaveTimer()
    {
        RunSaveDataController runSaveData = FindObjectOfType<RunSaveDataController>();
        if (runSaveData != null)
        {
            runSaveData.SaveData.CurrentSessionTime = GameplayTime.CurrentTime;
        }
    }
    public void LoadTimer()
    {
        RunSaveDataController runSaveData = FindObjectOfType<RunSaveDataController>();
        if (runSaveData != null)
        {
            GameplayTime.CurrentTime = runSaveData.SaveData.CurrentSessionTime;
        }
    }
    private void OnApplicationQuit()
    {
        Debug.Log(GameplayTime.CurrentTime);
        SaveTimer();
    }
}
