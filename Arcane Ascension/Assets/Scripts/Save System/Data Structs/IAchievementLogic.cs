/// <summary>
/// Interface implemented by classes that contain achievment information.
/// </summary>
public interface IAchievementLogic
{
    /// <summary>
    /// Triggers an achievement.
    /// </summary>
    /// <param name="achievementType">Type of achievement.</param>
    void TriggerAchievement(AchievementType achievementType, int value = 0, int[] valueArray = null);
}
