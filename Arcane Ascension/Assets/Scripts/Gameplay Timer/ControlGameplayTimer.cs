using System;
using UnityEngine;

/// <summary>
/// has methods to control gameplay timer.
/// </summary>
public class ControlGameplayTimer : MonoBehaviour
{
    [Header("Achievements")]
    [SerializeField] protected RunStatsLogicSO achievementLogic;

    public void ResetTimer() => GameplayTime.ResetTimer();
    public void PlayTimer() => GameplayTime.PlayTimer();
    public void PauseTimer() => GameplayTime.PauseTimer();
    public void SaveTimer()
    {
        GameplayTime.SaveTimer();

        // Run timer achievement
        TimeSpan timer = GameplayTime.GameTimer;
        int[] sessionTime = new int[]
        {
            timer.Hours, timer.Minutes, timer.Seconds
        };

        achievementLogic.TriggerRunStats(RunStatsType.RunTime, value: 
            (int)timer.TotalSeconds, valueArray: sessionTime);
    }

    public void LoadTimer() => GameplayTime.LoadTimer();

    private void OnValidate()
    {
        if (achievementLogic == null)
        {
            Debug.LogError($"Achievement logic on {name} not set.");
        }
    }
}
