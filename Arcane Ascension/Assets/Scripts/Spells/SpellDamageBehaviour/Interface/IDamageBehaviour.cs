using UnityEngine;

/// <summary>
/// Interface for damage behaviours.
/// </summary>
public interface IDamageBehaviour
{
    /// <summary>
    /// Damages something.
    /// </summary>
    /// <param name="parent">Parent spell behaviour.</param>
    /// <param name="other">Collision to damage.</param>
    /// <param name="damageMultiplier">Damage multiplier. It's 1 by default.</param>
    void Damage(SpellBehaviourAbstract parent, Collider other = null, float damageMultiplier = 1);
}
