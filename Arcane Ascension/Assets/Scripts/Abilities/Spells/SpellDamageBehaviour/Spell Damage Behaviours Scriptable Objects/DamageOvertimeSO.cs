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
    /// <param name="parent">Parent spell behaviour.</param>
    /// <param name="other">Collider to damage.</param>
    /// <param name="damageMultiplier">Damage multiplier. It's 1 by default.</param>
    public override void Damage(SpellBehaviourAbstract parent, Collider other = null, float damageMultiplier = 1)
    {
        DamageLogic(parent, other, damageMultiplier);
    }

    /// <summary>
    /// Damage logic.
    /// </summary>
    /// <param name="parent">Parent spell behaviour.</param>
    /// <param name="other">Collider to get IDamageables to damage.</param>
    /// <param name="damageMultiplier">Damage multiplier. It's 1 by default.</param>
    protected override void DamageLogic(SpellBehaviourAbstract parent, Collider other = null, float damageMultiplier = 1)
    {
        if (other.gameObject.TryGetComponentInParent<IDamageable>(out IDamageable character))
        {
            // If the target is different than who cast the spell
            if (!character.Equals(parent.ThisIDamageable))
            {
                character.TakeDamageOvertime(
                parent.WhoCast.CommonAttributes.BaseDamageMultiplier *
                parent.WhoCast.CommonAttributes.DamageElementMultiplier[parent.Spell.Element] *
                parent.Spell.Damage,
                parent.Spell.Element,
                parent.Spell.TimeInterval,
                parent.Spell.MaxTime);
            }
        }
    }
}
