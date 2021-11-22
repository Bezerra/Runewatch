using UnityEngine;
using ExtensionMethods;
using System.Collections.Generic;

/// <summary>
/// Scriptable object responsible for creating a spell's damage behaviour.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Damage Behaviour/Spell Damage Behaviour Forward", fileName = "Spell Damage AoEOvertime")]
public class DamageAoEOvertimeSO : DamageBehaviourAbstractSO
{
    /// <summary>
    /// Applies damage overtime to an area. Works with a collider.
    /// </summary>
    /// <param name="parent">Parent spell behaviour.</param>
    /// <param name="other">Collider to get IDamageables to damage.</param>
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
        Collider[] collisions = Physics.OverlapSphere(
                    parent.transform.position, parent.Spell.AreaOfEffect, Layers.PlayerEnemy);

        // Creates a new list with Stats characters
        // Must be Stats instead of IDamageable to apply status behaviour
        IList<Stats> charactersToDoDamage = new List<Stats>();

        // If the enemy is directly hit
        if (other.gameObject.TryGetComponentInParent<IDamageable>(out IDamageable enemyDirectHit))
        {
            if (!enemyDirectHit.Equals(parent.ThisIDamageable))
            {
                charactersToDoDamage.Add(enemyDirectHit as Stats);
            }
        }

        // Adds all IDamageable characters found to a list
        for (int i = 0; i < collisions.Length; i++)
        {
            // Creates a ray from spell to hit
            Ray dir = new Ray(
                parent.PositionOnHit,
                parent.PositionOnHit.Direction(collisions[i].ClosestPoint(parent.PositionOnHit)));

            // Checks if colliders are hit
            if (Physics.Raycast(dir, out RaycastHit characterHit, parent.Spell.AreaOfEffect * 0.5f,
                Layers.PlayerEnemyWithWallsFloor))
            {
                // If the collider is an IDamageable (meaning there wasn't a wall in the ray path)
                if (characterHit.collider.TryGetComponentInParent<IDamageable>(out IDamageable character))
                {
                    // If the target is different than who cast the spell
                    if (!character.Equals(parent.ThisIDamageable))
                    {
                        // If the layer is different (enemies can't hit enemies)
                        if (characterHit.collider.gameObject.layer != parent.LayerOfWhoCast)
                        {
                            if (charactersToDoDamage.Contains(character as Stats) == false)
                            {
                                charactersToDoDamage.Add(character as Stats);
                            }
                        }
                    }
                }
            }
        }

        // Damages all IDamageable characters
        // Damaging each enemy on this list will prevent the same enemy from receiving damage more than once
        if (charactersToDoDamage.Count > 0)
        {
            for (int i = 0; i < charactersToDoDamage.Count; i++)
            {
                ApplyStatusEffect(parent, charactersToDoDamage[i]);

                charactersToDoDamage[i].TakeDamageOvertime(
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
