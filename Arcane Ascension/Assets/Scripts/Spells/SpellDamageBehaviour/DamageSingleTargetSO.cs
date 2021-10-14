using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Scriptable object responsible for creating a spell's damage behaviour.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Damage Behaviour/Spell Damage Single Target", fileName = "Spell Damage Single Target")]
public class DamageSingleTargetSO : DamageBehaviourAbstractSO
{
    /// <summary>
    /// Damage for single target spells. Hits once.
    /// </summary>
    /// <param name="other">Collision to damage.</param>
    /// <param name="parent">Parent spell behaviour.</param>
    public override void Damage(Collision other, SpellBehaviourAbstract parent)
    {
        if (other.gameObject.TryGetComponentInParent<IDamageable>(out IDamageable character))
        {
            // If IDamageable hit is different than who casts the spell
            if (!character.Equals(parent.ThisIDamageable))
            {
                character.TakeDamage(parent.WhoCast.Attributes.BaseDamageMultiplier *
                    parent.Spell.Damage, parent.Spell.Element);
            }
        }   
    }
}
