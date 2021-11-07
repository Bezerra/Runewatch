using UnityEngine;

/// <summary>
/// Interface implemented by all abilities.
/// </summary>
public interface IAbility
{
    Sprite Icon { get; }
    string Name { get; }
    string Description { get; }
    byte ID { get; }
    int Tier { get; }
}
