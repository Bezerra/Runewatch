using UnityEngine;

/// <summary>
/// Interface for damage behaviours.
/// </summary>
public interface IDamageBehaviour
{
    /// <summary>
    /// Damages something.
    /// </summary>
    /// <param name="other">Collision to damage.</param>
    /// <param name="parent">Parent spell behaviour.</param>
    void Damage(Collision other, SpellBehaviourAbstract parent);

    /// <summary>
    /// Damages something.
    /// </summary>
    /// <param name="other">Collision to damage.</param>
    /// <param name="parent">Parent spell behaviour.</param>
    void Damage(Collider other, SpellBehaviourAbstract parent);
}
