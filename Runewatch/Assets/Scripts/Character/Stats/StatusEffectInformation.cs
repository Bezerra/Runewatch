using UnityEngine;

/// <summary>
/// Class with status effect information.
/// </summary>
public class StatusEffectInformation: IStatusEffectInformation
{
    /// <summary>
    /// Time the status effect was applied.
    /// </summary>
    public float TimeApplied { get; set; }

    /// <summary>
    /// Maximum duration of the status effect.
    /// </summary>
    public float Duration { get; }

    /// <summary>
    /// Status effect icon.
    /// </summary>
    public Sprite Icon { get; }

    public StatusEffectInformation(float timeApplied, float duration, Sprite icon)
    {
        TimeApplied = timeApplied;
        Duration = duration;
        Icon = icon;
    }
}
