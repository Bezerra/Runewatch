using System.Collections.Generic;

/// <summary>
/// Interface implemented by objects with one shot spell behaviours.
/// </summary>
public interface ISpellOneShotBehaviour
{
    IList<SpellBehaviourAbstractOneShotSO> SpellBehaviourOneShot { get; }
    IList<SpellOnHitBehaviourAbstractOneShotSO> OnHitBehaviourOneShot { get; }
    IList<SpellMuzzleBehaviourAbstractOneShotSO> MuzzleBehaviourOneShot { get; }
}
