using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Scriptable object responsible for creating a spell's damage behaviour.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Damage Behaviour/Spell Damage Single Target", 
    fileName = "Spell Damage Single Target")]
public class DamageSingleTargetSO : DamageBehaviourAbstractSO
{
    /// <summary>
    /// Damage for single target spells. Works with a collider.
    /// </summary>
    /// <param name="parent">Parent spell behaviour.</param>
    /// <param name="overridePosition">Transform to override position.</param>
    /// <param name="other">Collider to damage.</param>
    /// <param name="damageMultiplier">Damage multiplier. It's 1 by default.</param>
    public override void Damage(SpellBehaviourAbstract parent, Transform overridePosition = null,
        Collider other = null, float damageMultiplier = 1)
    {
        DamageLogic(parent, overridePosition, other, damageMultiplier);
    }

    /// <summary>
    /// Damage logic.
    /// </summary>
    /// <param name="parent">Parent spell behaviour.</param>
    /// <param name="overridePosition">Transform to override position.</param>
    /// <param name="other">Collider to get IDamageables to damage.</param>
    /// <param name="damageMultiplier">Damage multiplier. It's 1 by default.</param>
    protected override void DamageLogic(SpellBehaviourAbstract parent, Transform overridePosition,
        Collider other = null, float damageMultiplier = 1)
    {
        SpellInteractionWithObjectsLogic(parent, other);

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

                ApplyStatusEffect(parent);

                if (parent.ShotHitTriggered == false)
                {
                    runStatsLogic.ShotsHit++;
                    parent.ShotHitTriggered = true;
                }

                float totalDamageDone = character.TakeDamage(
                    parent.WhoCast.CommonAttributes.BaseDamageMultiplier *
                    parent.WhoCast.CommonAttributes.DamageElementMultiplier[parent.Spell.Element] *
                    parent.Spell.Damage(parent.WhoCast.CommonAttributes.Type) *
                    damageMultiplier,
                    criticalChance,
                    parent.WhoCast.CommonAttributes.CriticalDamageModifier,
                    parent.Spell.Element,
                    parent.WhoCast.transform.position);

                if (parent.WhoCast.CommonAttributes.Type == CharacterType.Player)
                {
                    parent.WhoCast.Heal(
                        totalDamageDone *
                        ((parent.WhoCast as PlayerStats).PlayerAttributes.LifeSteal * 0.01f),
                        StatsType.Health, true);
                }
            }
        }
    }

    /// <summary>
    /// Spell ambience interaction.
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="other"></param>
    private void SpellInteractionWithObjectsLogic(SpellBehaviourAbstract parent,
        Collider other = null)
    {
        // Triggers an interaction if the elements match
        if (other.TryGetComponent(out IInteractionWithSpell singleTargetInteraction))
        {
            singleTargetInteraction.ExecuteInteraction(parent.Spell.Element);
        }
    }
}
