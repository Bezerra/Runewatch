using UnityEngine;

/// <summary>
/// Interface implemented by status effects.
/// </summary>
public interface IStatusEffectInformation
{
    /// <summary>
    /// Status effect icon.
    /// </summary>
    Sprite Icon { get; }

    /// <summary>
    /// Time the status was applied.
    /// </summary>
    float TimeApplied { get; set; }

    /// <summary>
    /// Maximum duration of the status.
    /// </summary>
    float Duration { get; }
}
