using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Scriptable object responsible for creating a spell's damage behaviour.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Damage Behaviour/Spell Damage AoE", fileName = "Spell Damage AoE")]
public class DamageAoESO : DamageBehaviourAbstractSO
{
    /// <summary>
    /// Applies AoE damage. Works with a collision.
    /// </summary>
    /// <param name="other">Collision get IDamageables to damage.</param>
    /// <param name="parent">Parent spell behaviour.</param>
    public override void Damage(Collision other, SpellBehaviourAbstract parent)
    {
        Collider[] collisions = Physics.OverlapSphere(
                    other.contacts[0].point, parent.Spell.AreaOfEffect, Layers.EnemyWithWalls);

        // Creates a new list with IDamageable characters
        IList<IDamageable> charactersToDoDamage = new List<IDamageable>();

        // If the enemy is directly hit
        if (other.gameObject.TryGetComponentInParent<IDamageable>(out IDamageable enemyDirectHit))
        {
            if (!enemyDirectHit.Equals(parent.ThisIDamageable))
            {
                charactersToDoDamage.Add(enemyDirectHit);
            }
        }

        DamageLogic(charactersToDoDamage, collisions, parent);
    }

    /// <summary>
    /// Applies AoE damage. Works with a collider.
    /// </summary>
    /// <param name="other">Colliger to get IDamageables to damage.</param>
    /// <param name="parent">Parent spell behaviour.</param>
    public override void Damage(Collider other, SpellBehaviourAbstract parent)
    {
        Collider[] collisions = Physics.OverlapSphere(
                    parent.transform.position, parent.Spell.AreaOfEffect, Layers.EnemyWithWalls);

        // Creates a new list with IDamageable characters
        IList<IDamageable> charactersToDoDamage = new List<IDamageable>();

        // If the enemy is directly hit
        if (other.gameObject.TryGetComponentInParent<IDamageable>(out IDamageable enemyDirectHit))
        {
            if (!enemyDirectHit.Equals(parent.ThisIDamageable))
            {
                charactersToDoDamage.Add(enemyDirectHit);
            }
        }

        DamageLogic(charactersToDoDamage, collisions, parent);
    }

    /// <summary>
    /// Damage logic.
    /// </summary>
    /// <param name="charactersToDoDamage">Characters to do damage to.</param>
    /// <param name="collidersToDamage">Colliders with all enemies hit.</param>
    /// <param name="parent">Parent spell behaviour.</param>
    private void DamageLogic(IList<IDamageable> charactersToDoDamage, Collider[] collidersToDamage, SpellBehaviourAbstract parent)
    {
        // Adds all IDamageable characters found to a list
        for (int i = 0; i < collidersToDamage.Length; i++)
        {
            // Creates a ray from spell to hit
            Ray dir = new Ray(
                        parent.transform.position,
                        (collidersToDamage[i].transform.position - parent.transform.position).normalized);

            if (Physics.Raycast(dir, out RaycastHit characterHit, parent.Spell.AreaOfEffect * 0.5f,
                Layers.EnemyWithWalls))
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

        // Damages all IDamageable characters
        // Damaging each enemy on this list will prevent the same enemy from receiving damage more than once
        if (charactersToDoDamage.Count > 0)
        {
            for (int i = 0; i < charactersToDoDamage.Count; i++)
            {
                charactersToDoDamage[i].TakeDamage(
                    parent.WhoCast.Attributes.BaseDamageMultiplier * parent.Spell.Damage,
                    parent.Spell.Element);
            }
        }
    }
}
