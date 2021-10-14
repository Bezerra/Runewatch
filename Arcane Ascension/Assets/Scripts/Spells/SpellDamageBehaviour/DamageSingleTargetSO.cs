using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Scriptable object responsible for creating a spell's damage behaviour.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Damage Behaviour/Spell Damage Single Target", fileName = "Spell Damage Single Target")]
public class DamageSingleTargetSO : DamageBehaviourAbstractSO
{
    public override void Damage(Collider other, SpellBehaviourAbstract parent)
    {
        if (other.gameObject.TryGetComponentInParent<IDamageable>(out IDamageable character) &&
            other.gameObject.TryGetComponentInParent<Stats>(out Stats stats))
        {
            // If the gameobject hit is different than who casts the spell
            if (stats != parent.WhoCast)
            {
                character.TakeDamage(parent.WhoCast.Attributes.BaseDamageMultiplier *
                    parent.Spell.Damage, parent.Spell.Element);
            }
        }   
    }
}
