/// <summary>
/// Interface implemented by classes that contain run stats information.
/// </summary>
public interface IRunStatsLogic
{
    /// <summary>
    /// Triggers a run stats.
    /// </summary>
    /// <param name="achievementType">Type of run stats.</param>
    void TriggerRunStats(RunStatsType achievementType, int value = 0);
}
