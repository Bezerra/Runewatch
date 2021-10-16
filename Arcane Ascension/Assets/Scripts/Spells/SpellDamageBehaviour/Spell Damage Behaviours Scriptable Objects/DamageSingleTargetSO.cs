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
            // If IDamageable hit is different than who casts the spell
            if (!character.Equals(parent.ThisIDamageable))
            {
                character.TakeDamage(parent.WhoCast.Attributes.BaseDamageMultiplier *
                    parent.Spell.Damage, parent.WhoCast.Attributes.CriticalChance, parent.Spell.Element);
            }
        }
    }
}
