using UnityEngine;

/// <summary>
/// has methods to control gameplay timer.
/// </summary>
public class ControlGameplayTimer : MonoBehaviour
{
    public void ResetTimer() => GameplayTime.ResetTimer();
    public void PlayTimer() => GameplayTime.PlayTimer();
    public void PauseTimer() => GameplayTime.PauseTimer();
    public void SaveTimer() => GameplayTime.SaveTimer();
    public void LoadTimer() => GameplayTime.LoadTimer();
}
