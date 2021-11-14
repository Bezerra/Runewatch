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
        // Creates a new list with IDamageable characters
        IList<IDamageable> charactersToDoDamage = new List<IDamageable>();

        Collider[] collisions = Physics.OverlapSphere(
                    parent.transform.position, parent.Spell.AreaOfEffect, Layers.EnemyWithWalls);

        // If the enemy is directly hit
        if (other.gameObject.TryGetComponentInParent<IDamageable>(out IDamageable enemyDirectHit))
        {
            if (!enemyDirectHit.Equals(parent.ThisIDamageable))
            {
                charactersToDoDamage.Add(enemyDirectHit);
            }
        }

        // Adds all IDamageable characters found to a list
        for (int i = 0; i < collisions.Length; i++)
        {
            // Creates a ray from spell to hit
            Ray dir = new Ray(
                        parent.PositionOnHit,
                        (collisions[i].transform.position - parent.transform.position).normalized);

            if (parent.LayerOfCharacterHit != parent.LayerOfWhoCast)
            {
                if (Physics.Raycast(dir, out RaycastHit characterHit, parent.Spell.AreaOfEffect * 0.5f,
                    Layers.PlayerEnemyWithWallsFloor))
                {
                    // If the collider is an IDamageable (meaning there wasn't a wall in the ray path)
                    if (characterHit.collider.TryGetComponentInParent<IDamageable>(out IDamageable character))
                    {
                        // If the target is different than who cast the spell
                        if (!character.Equals(parent.ThisIDamageable))
                        {
                            if (charactersToDoDamage.Contains(character) == false)
                            {
                                charactersToDoDamage.Add(character);
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
