using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Abstract scriptable object for damage behaviours.
/// </summary>
[InlineEditor]
public abstract class DamageBehaviourAbstractSO : ScriptableObject, IDamageBehaviour
{
    /// <summary>
    /// Executes damage behaviour.
    /// </summary>
    /// <param name="other">Collision collider.</param>
    /// <param name="parent">Parent spell.</param>
    public abstract void Damage(Collider other, SpellBehaviourAbstract parent);
}
