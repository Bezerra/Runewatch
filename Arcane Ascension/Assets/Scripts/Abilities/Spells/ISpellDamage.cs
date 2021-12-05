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
    float DelayToDoDamage { get; }
    Vector2 MinMaxDamage { get; }
    Sprite ElementIcon { get; }
    Sprite TargetTypeIcon { get; }
    DamageBehaviourAbstractSO DamageBehaviour { get; }
    StatusBehaviourAbstractSO StatusBehaviour { get; }

    /// <summary>
    /// Spell damage.
    /// </summary>
    /// <param name="characterType">Character type who cast this spell.</param>
    /// <returns>A number with damage.</returns>
    float Damage(CharacterType characterType);
}
