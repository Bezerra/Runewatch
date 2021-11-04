using System.Collections.Generic;

/// <summary>
/// Interface implemented by objects with continuous spell behaviours.
/// </summary>
public interface ISpellContinuousBehaviour
{
    IList<SpellBehaviourAbstractContinuousSO> SpellBehaviourContinuous { get; }
    SpellOnHitBehaviourAbstractContinuousSO OnHitBehaviourContinuous { get; }
    SpellMuzzleBehaviourAbstractContinuousSO MuzzleBehaviourContinuous { get; }
}
