using System.Collections.Generic;

/// <summary>
/// Interface implemented by objects with continuous spell behaviours.
/// </summary>
public interface ISpellContinuousBehaviour
{
    IList<SpellBehaviourAbstractContinuousSO> SpellBehaviourContinuous { get; }
    IList<SpellOnHitBehaviourAbstractContinuousSO> OnHitBehaviourContinuous { get; }
    IList<SpellMuzzleBehaviourAbstractContinuousSO> MuzzleBehaviourContinuous { get; }
}
