using UnityEngine;

/// <summary>
/// Interface implemented by objects with spell damage.
/// </summary>
public interface ISpellDamage
{
    ElementType Element { get; }
    float TimeInterval { get; }
    float MaxTime { get; }
    float AreaOfEffect { get; }
    float Damage { get; }
    Vector2 MinMaxDamage { get; }
    Sprite ElementIcon { get; }
    Sprite TargetTypeIcon { get; }
    DamageBehaviourAbstractSO DamageBehaviour { get; }
}
