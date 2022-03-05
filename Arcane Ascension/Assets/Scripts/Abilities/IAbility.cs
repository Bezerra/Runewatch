using UnityEngine;

/// <summary>
/// Interface implemented by all abilities.
/// </summary>
public interface IAbility
{
    /// <summary>
    /// Icon of the ability.
    /// </summary>
    Sprite Icon { get; }

    /// <summary>
    /// Name of the ability.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Description of the ability.
    /// </summary>
    string Description { get; }

    /// <summary>
    /// ID of the ability.
    /// </summary>
    byte ID { get; }

    /// <summary>
    /// Tier of the ability.
    /// </summary>
    int Tier { get; }

    /// <summary>
    /// Does this ability affects or is affected by some stats.
    /// </summary>
    AffectsType Relation { get; }
}
