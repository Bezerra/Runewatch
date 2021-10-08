using UnityEngine;

/// <summary>
/// Interface for damage behaviours.
/// </summary>
public interface IDamageBehaviour
{
    void Damage(Collider other, SpellBehaviourAbstract parent);
}
