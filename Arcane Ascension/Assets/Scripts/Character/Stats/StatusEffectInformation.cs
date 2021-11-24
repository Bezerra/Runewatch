/// <summary>
/// Class with status effect information.
/// </summary>
public class StatusEffectInformation
{
    public float TimeApplied { get; set; }
    public float Duration { get; }

    public StatusEffectInformation(float timeApplied, float duration)
    {
        TimeApplied = timeApplied;
        Duration = duration;
    }
}
