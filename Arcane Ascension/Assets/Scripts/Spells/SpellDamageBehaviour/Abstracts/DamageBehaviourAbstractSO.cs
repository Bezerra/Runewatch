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
    /// <param name="other">Collider to damage.</param>
    /// <param name="damageMultiplier">Damage multiplier. It's 1 by default.</param>
    public abstract void Damage(SpellBehaviourAbstract parent, Collider other = null, float damageMultiplier = 1);

    /// <summary>
    /// Damage logic.
    /// </summary>
    /// <param name="parent">Parent spell behaviour.</param>
    /// <param name="other">Collider to get IDamageables to damage.</param>
    /// <param name="damageMultiplier">Damage multiplier. It's 1 by default.</param>
    protected abstract void DamageLogic(SpellBehaviourAbstract parent, Collider other = null, float damageMultiplier = 1);
}
