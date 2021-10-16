using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Scriptable object responsible for creating a spell's damage behaviour.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Damage Behaviour/Spell Damage Overtime", fileName = "Spell Damage Overtime")]
public class DamageOvertimeSO : DamageBehaviourAbstractSO
{
    /// <summary>
    /// Applies damage overtime once. Works with a collider.
    /// </summary>
    /// <param name="other">Collider to damage.</param>
    /// <param name="parent">Parent spell behaviour.</param>
    public override void Damage(Collider other, SpellBehaviourAbstract parent)
    {
        DamageLogic(other, parent);
    }

    /// <summary>
    /// Damage logic.
    /// </summary>
    /// <param name="other">Collider to get IDamageables to damage.</param>
    /// <param name="parent">Parent spell behaviour.</param>
    protected override void DamageLogic(Collider other, SpellBehaviourAbstract parent)
    {
        if (other.gameObject.TryGetComponentInParent<IDamageable>(out IDamageable character))
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
}
