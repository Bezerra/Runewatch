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
}
