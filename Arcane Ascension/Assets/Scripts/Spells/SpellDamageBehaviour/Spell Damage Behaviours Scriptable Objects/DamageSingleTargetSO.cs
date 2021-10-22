using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Scriptable object responsible for creating a spell's damage behaviour.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Damage Behaviour/Spell Damage Single Target", fileName = "Spell Damage Single Target")]
public class DamageSingleTargetSO : DamageBehaviourAbstractSO
{
    /// <summary>
    /// Damage for single target spells. Works with a collider.
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
            // If IDamageable hit is different than who casts the spell
            if (!character.Equals(parent.ThisIDamageable))
            {
                character.TakeDamage(parent.WhoCast.Attributes.BaseDamageMultiplier *
                    parent.Spell.Damage * damageMultiplier, parent.WhoCast.Attributes.CriticalChance, parent.Spell.Element);
            }
        }
    }
}
