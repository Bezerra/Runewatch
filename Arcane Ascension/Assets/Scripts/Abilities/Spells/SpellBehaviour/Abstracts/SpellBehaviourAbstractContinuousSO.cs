using UnityEngine;

/// <summary>
/// Abstract scriptable object used to create continuous spell behaviours.
/// </summary>
public abstract class SpellBehaviourAbstractContinuousSO: SpellBehaviourAbstractSO
{
    /// <summary>
    /// Executes on start.
    /// </summary>
    public abstract void StartBehaviour(SpellBehaviourContinuous parent);

    /// <summary>
    /// Executes on update.
    /// </summary>
    public abstract void ContinuousUpdateBehaviour(SpellBehaviourContinuous parent);

    /// <summary>
    /// Executes on fixed update.
    /// </summary>
    public abstract void ContinuousFixedUpdateBehaviour(SpellBehaviourContinuous parent);

    /// <summary>
    /// Executes on hit.
    /// </summary>
    /// <param name="other">Collider.</param>
    public abstract void HitTriggerBehaviour(Collider other, SpellBehaviourContinuous parent);
}
