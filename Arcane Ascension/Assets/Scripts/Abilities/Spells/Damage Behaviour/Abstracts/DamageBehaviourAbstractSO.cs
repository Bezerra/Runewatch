using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Abstract scriptable object for damage behaviours.
/// </summary>
[InlineEditor]
public abstract class DamageBehaviourAbstractSO : ScriptableObject, IDamageBehaviour
{
    /// <summary>
    /// Executes damage behaviour for colliders.
    /// </summary>
    /// <param name="parent">Parent spell.</param>
    /// <param name="overridePosition">Transform to override position.</param>
    /// <param name="other">Collider to damage.</param>
    /// <param name="damageMultiplier">Damage multiplier. It's 1 by default.</param>
    public abstract void Damage(SpellBehaviourAbstract parent, Transform overridePosition = null,
        Collider other = null, float damageMultiplier = 1);

    /// <summary>
    /// Damage logic.
    /// </summary>
    /// <param name="parent">Parent spell behaviour.</param>
    /// <param name="overridePosition">Transform to override position.</param>
    /// <param name="other">Collider to get IDamageables to damage.</param>
    /// <param name="damageMultiplier">Damage multiplier. It's 1 by default.</param>
    protected abstract void DamageLogic(SpellBehaviourAbstract parent, 
        Transform overridePosition = null, Collider other = null, float damageMultiplier = 1);

    /// <summary>
    /// If this spell is a single target spell and contains a status effect, it applies that status effect.
    /// </summary>
    /// <param name="parent">Parent spell.</param>
    protected void ApplyStatusEffect(SpellBehaviourAbstract parent)
    {
        if (parent.Spell.StatusBehaviour != null)
        {
            GameObject statusGO =
                StatusBehaviourPoolCreator.Pool.InstantiateFromPool("Status Behaviour");

            if (statusGO.TryGetComponent(out StatusBehaviour statusBehaviour))
            {
                statusBehaviour.Initialize(parent.Spell, parent.WhoCast, parent.CharacterHit, parent);
                statusBehaviour.TriggerStartBehaviour();
            }
        }
    }

    /// <summary>
    /// If this spell is an AoE spell and contains a status effect, it applies that status effect.
    /// </summary>
    /// <param name="parent">Parent spell.</param>
    /// <param name="characterHit">Character hit by the spell.</param>
    protected void ApplyStatusEffect(SpellBehaviourAbstract parent, Stats characterHit)
    {
        if (parent.Spell.StatusBehaviour != null)
        {
            GameObject statusGO =
                StatusBehaviourPoolCreator.Pool.InstantiateFromPool("Status Behaviour");

            if (statusGO.TryGetComponent(out StatusBehaviour statusBehaviour))
            {
                statusBehaviour.Initialize(parent.Spell, parent.WhoCast, characterHit, parent);
                statusBehaviour.TriggerStartBehaviour();
            }
        }
    }
}
