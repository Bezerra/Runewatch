using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Scriptable object responsible for creating single target damage plus mana steal behaviour.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Damage Behaviour/Spell Damage Mana Steal", fileName = "Spell Damage Mana Steal")]
public class DamageManaStealSO : DamageBehaviourAbstractSO
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
                // Critical chance logic
                float criticalChance = parent.WhoCast.CommonAttributes.CriticalChance;

                // This will only happen if the spell didn't hit an immune collider
                if (damageMultiplier > 0)
                {
                    // Ray to check if damage is moving towards a critical spot
                    Ray criticalSpotRay = new Ray(
                        parent.PositionOnHit,
                        parent.transform.forward);

                    // Raycast from spell direction to forward, to check if it hit a critical point
                    if (Physics.Raycast(criticalSpotRay, 3, Layers.EnemySensiblePoint))
                    {
                        criticalChance = 1;
                    }
                }
                else
                {
                    criticalChance = 0;
                }

                // Damages character
                character.TakeDamage(
                    parent.WhoCast.CommonAttributes.BaseDamageMultiplier *
                    parent.WhoCast.CommonAttributes.DamageElementMultiplier[parent.Spell.Element] *
                    parent.Spell.Damage(parent.WhoCast.CommonAttributes.Type) *
                    damageMultiplier,
                    criticalChance,
                    parent.WhoCast.CommonAttributes.CriticalDamageModifier,
                    parent.Spell.Element,
                    parent.WhoCast.transform.position);

                // Heals mana
                if (parent.ThisIMana != null)
                {
                    PlayerStats playerStats = (PlayerStats)parent.WhoCast;
                    playerStats.Heal(
                        playerStats.PlayerAttributes.ManaRegenSteal, 
                        StatsType.Mana);
                }
            }
        }
    }
}
