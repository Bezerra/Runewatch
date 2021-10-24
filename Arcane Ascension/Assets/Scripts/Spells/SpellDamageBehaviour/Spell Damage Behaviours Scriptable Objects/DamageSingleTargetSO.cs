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
                float criticalChance = parent.WhoCast.Attributes.CriticalChance;

                // Critical on sensible point
                if (other != null)
                {
                    float sphereSize = parent.Spell.CastType == SpellCastType.ContinuousCast ?
                        sphereSize = 0.1f : sphereSize = 0.2f;

                    if (Physics.OverlapSphere(parent.PositionOnSpawnAndHit, sphereSize, Layers.EnemySensiblePoint).Length > 0)
                    {
                        criticalChance = 1;
                    }
                }
                
                character.TakeDamage(parent.WhoCast.Attributes.BaseDamageMultiplier *
                    parent.Spell.Damage * damageMultiplier, criticalChance, parent.Spell.Element);
            }
        }
    }
}
