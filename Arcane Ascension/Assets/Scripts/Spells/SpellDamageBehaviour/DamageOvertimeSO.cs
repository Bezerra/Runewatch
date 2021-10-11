using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Scriptable object responsible for creating a spell's damage behaviour.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Damage Behaviour/Spell Damage Overtime", fileName = "Spell Damage Overtime")]
public class DamageOvertimeSO : ScriptableObject
{
    public void Damage(Collider other, SpellBehaviourAbstract parent)
    {
        if (other.gameObject.TryGetComponentInParent<IDamageable>(out IDamageable character))
            character.TakeDamageOvertime(
                parent.WhoCast.Attributes.BaseDamageMultiplier * parent.Spell.Damage,
                parent.Spell.Element,
                parent.Spell.TimeInterval,
                parent.Spell.MaxTime);
    }
}
