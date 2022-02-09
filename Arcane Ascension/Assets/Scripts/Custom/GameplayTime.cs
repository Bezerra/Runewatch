using System;
using UnityEngine;

/// <summary>
/// Class responsible for updating current gameplay time
/// </summary>
public static class GameplayTime
{
    public static float CurrentTime { get; set; }
    private static bool isStopwatchActive;
    public static TimeSpan GameTimer { get; private set; }

    /// <summary>
    /// Resets the timer.
    /// </summary>
    public static void ResetTimer()
    {
        isStopwatchActive = false;
        CurrentTime = 0;
        PlayerPrefs.SetFloat(PPrefsGeneral.CurrentSessionGameTime.ToString(), 0);
    }

    /// <summary>
    /// Updates the timer (Must run on Update()).
    /// </summary>
    public static void UpdateTimer()
    {
        if (isStopwatchActive)
        {
            CurrentTime += Time.deltaTime;
        }
        GameTimer = TimeSpan.FromSeconds(CurrentTime);
    }

    /// <summary>
    /// Plays/Resumes the timer.
    /// </summary>
    public static void PlayTimer() => isStopwatchActive = true;

    /// <summary>
    /// Pauses the timer.
    /// </summary>
    public static void PauseTimer() => isStopwatchActive = false;

    public static void SaveTimer() =>
        PlayerPrefs.SetFloat(
            PPrefsGeneral.CurrentSessionGameTime.ToString(), GameplayTime.CurrentTime);

    public static void LoadTimer() =>
        GameplayTime.CurrentTime = PlayerPrefs.GetFloat(
            PPrefsGeneral.CurrentSessionGameTime.ToString(), 0);
}
