using System.Collections.Generic;

/// <summary>
/// Interface implemented by objects with one shot spell behaviours.
/// </summary>
public interface ISpellOneShotBehaviour
{
    IList<SpellBehaviourAbstractOneShotSO> SpellBehaviourOneShot { get; }
    SpellOnHitBehaviourAbstractOneShotSO OnHitBehaviourOneShot { get; }
    SpellMuzzleBehaviourAbstractOneShotSO MuzzleBehaviourOneShot { get; }
}
