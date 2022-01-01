using System.Collections.Generic;

/// <summary>
/// Interface implemented by objects with one shot spell behaviours.
/// </summary>
public interface ISpellOneShotBehaviour
{
    IList<SpellBehaviourAbstractSO> SpellBehaviourOneShot { get; }
    IList<SpellOnHitBehaviourAbstractSO> OnHitBehaviourOneShot { get; }
    IList<SpellMuzzleBehaviourAbstractSO> MuzzleBehaviourOneShot { get; }
}
