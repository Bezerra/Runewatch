using System.Collections.Generic;

/// <summary>
/// Interface implemented by objects with spell behaviours.
/// </summary>
public interface ISpellBehaviour
{
    IList<SpellBehaviourAbstractSO> SpellBehaviour{ get; }
    SpellCastType CastType { get; }
    float Speed { get; }
    float Cooldown { get; }
    float CooldownCounter { get; set; }
    SpellOnHitBehaviourAbstractSO OnHitBehaviour { get; }
    SpellMuzzleBehaviourAbstractSO MuzzleBehaviour { get; }
    AttackBehaviourAbstractSO AttackBehaviour { get; }
}
