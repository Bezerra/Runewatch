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
    /// Executes on hit. Creates hit impact.
    /// </summary>
    /// <param name="other">Collider.</param>
    public abstract void HitTriggerBehaviour(Collider other, SpellBehaviourOneShot parent);

    /// <summary>
    /// Applies damage behaviour.
    /// </summary>
    /// <param name="other">Colliders to get IDamageable from.</param>
    /// <param name="parent">Parent spell behaviour.</param>
    /// <param name="damageMultiplier">Damage multiplier. It's 1 by default.</param>
    protected void DamageBehaviour(Collider other, SpellBehaviourOneShot parent, float damageMultiplier = 1) =>
        parent.Spell.DamageBehaviour.Damage(other, parent, damageMultiplier);

    /// <summary>
    /// Stops spell speed.
    /// After the spell is set to zero velocity, it will start a counter to disable it on update behaviour.
    /// </summary>
    protected void StopSpellSpeed(SpellBehaviourOneShot parent) =>
        parent.Rb.velocity = Vector3.zero;
}
