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
    /// Executes on update before spell is fired.
    /// Used for one shot casts with release, while player is pressing fire.
    /// </summary>
    public abstract void ContinuousUpdateBeforeSpellBehaviour(SpellBehaviourOneShot parent);

    /// <summary>
    /// Executes on fixed update.
    /// </summary>
    public abstract void ContinuousFixedUpdateBehaviour(SpellBehaviourOneShot parent);

    /// <summary>
    /// Executes on hit. Creates hit impact.
    /// </summary>
    /// <param name="other">Collider.</param>
    public abstract void HitTriggerBehaviour(Collider other, SpellBehaviourOneShot parent);
}
