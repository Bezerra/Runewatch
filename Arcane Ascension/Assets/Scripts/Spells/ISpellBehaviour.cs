using System.Collections.Generic;

/// <summary>
/// Interface implemented by objects with spell behaviours.
/// </summary>
public interface ISpellBehaviour
{
    SpellCastType CastType { get; }
    float Speed { get; }
    float Cooldown { get; }
    float CooldownCounter { get; set; }
    IList<SpellBehaviourAbstractOneShotSO> SpellBehaviourOneShot{ get; }
    SpellOnHitBehaviourAbstractOneShotSO OnHitBehaviourOneShot { get; }
    SpellMuzzleBehaviourAbstractOneShotSO MuzzleBehaviourOneShot { get; }
    AttackBehaviourAbstractOneShotSO AttackBehaviourOneShot { get; }
    IList<SpellBehaviourAbstractContinuousSO> SpellBehaviourContinuous { get; }
    SpellOnHitBehaviourAbstractContinuousSO OnHitBehaviourContinuous { get; }
    SpellMuzzleBehaviourAbstractContinuousSO MuzzleBehaviourContinuous { get; }
    AttackBehaviourAbstractContinuousSO AttackBehaviourContinuous { get; }
}
