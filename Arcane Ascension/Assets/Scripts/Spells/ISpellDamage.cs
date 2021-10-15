/// <summary>
/// Interface implemented by objects with spell damage.
/// </summary>
public interface ISpellDamage
{
    SpellDamageType DamageType { get; }
    bool AppliesDamageOvertime { get; }
    ElementType Element { get; }
    float TimeInterval { get; }
    float MaxTime { get; }
    float AreaOfEffect { get; }
    float Damage { get; }
    DamageBehaviourAbstractSO DamageBehaviour { get; }
}
