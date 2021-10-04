using UnityEngine;

/// <summary>
/// Abstract scriptable object used to create one shot spell behaviours.
/// </summary>
public abstract class SpellBehaviourAbstractOneShotSO: SpellBehaviourAbstractSO
{
    /// <summary>
    /// Executes on start.
    /// </summary>
    public abstract void StartBehaviour(SpellBehaviourOneShot parent);

    /// <summary>
    /// Executes on update.
    /// </summary>
    public abstract void ContinuousUpdateBehaviour(SpellBehaviourOneShot parent);

    /// <summary>
    /// Executes on fixed update.
    /// </summary>
    public abstract void ContinuousFixedUpdateBehaviour(SpellBehaviourOneShot parent);

    /// <summary>
    /// Executes on hit.
    /// </summary>
    /// <param name="other">Collider.</param>
    public abstract void HitBehaviour(Collider other, SpellBehaviourOneShot parent);
}
