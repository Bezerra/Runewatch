using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Abstract scriptable object for damage behaviours.
/// </summary>
[InlineEditor]
public abstract class DamageBehaviourAbstractSO : ScriptableObject, IDamageBehaviour
{
    /// <summary>
    /// Executes damage behaviour for collisions.
    /// </summary>
    /// <param name="other">Collision to damage.</param>
    /// <param name="parent">Parent spell.</param>
    public abstract void Damage(Collision other, SpellBehaviourAbstract parent);

    /// <summary>
    /// Executes damage behaviour for colliders.
    /// </summary>
    /// <param name="other">Collider to damage.</param>
    /// <param name="parent">Parent spell.</param>
    public abstract void Damage(Collider other, SpellBehaviourAbstract parent);
}
