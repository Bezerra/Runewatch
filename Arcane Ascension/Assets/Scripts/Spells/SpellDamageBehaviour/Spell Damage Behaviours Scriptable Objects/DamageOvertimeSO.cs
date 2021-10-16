using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Scriptable object responsible for creating a spell's damage behaviour.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Damage Behaviour/Spell Damage Overtime", fileName = "Spell Damage Overtime")]
public class DamageOvertimeSO : DamageBehaviourAbstractSO
{
    /// <summary>
    /// Applies damage overtime once. Works with a collision.
    /// </summary>
    /// <param name="other">Collision to damage.</param>
    /// <param name="parent">Parent spell behaviour.</param>
    public override void Damage(Collision other, SpellBehaviourAbstract parent)
    {
        if (other.gameObject.TryGetComponentInParent<IDamageable>(out IDamageable character))
            DamageLogic(character, parent);
    }

    /// <summary>
    /// Applies damage overtime once. Works with a collider.
    /// </summary>
    /// <param name="other">Collider to damage.</param>
    /// <param name="parent">Parent spell behaviour.</param>
    public override void Damage(Collider other, SpellBehaviourAbstract parent)
    {
        if (other.gameObject.TryGetComponentInParent<IDamageable>(out IDamageable character))
            DamageLogic(character, parent);
    }

    /// <summary>
    /// Damage logic.
    /// </summary>
    /// <param name="character">Character to damage.</param>
    /// <param name="parent">Parent spell behaviour.</param>
    private void DamageLogic(IDamageable character, SpellBehaviourAbstract parent)
    {
        // If the target is different than who cast the spell
        if (!character.Equals(parent.ThisIDamageable))
        {
            character.TakeDamageOvertime(
            parent.WhoCast.Attributes.BaseDamageMultiplier * parent.Spell.Damage,
            parent.Spell.Element,
            parent.Spell.TimeInterval,
            parent.Spell.MaxTime);
        }
    }
}
