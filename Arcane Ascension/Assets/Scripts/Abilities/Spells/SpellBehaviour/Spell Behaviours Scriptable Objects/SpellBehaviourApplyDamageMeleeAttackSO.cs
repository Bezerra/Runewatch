using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Scriptable object responsible for melee attack behaviours.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Apply Damage Melee Attack", 
    fileName = "Spell Behaviour Apply Damage Melee Attack")]
sealed public class SpellBehaviourApplyDamageMeleeAttackSO : SpellBehaviourAbstractSO
{
    /// <summary>
    /// Sets variables used to do damage. Applies damage behaviour.
    /// </summary>
    /// <param name="parent">Parent spell.</param>
    public override void StartBehaviour(SpellBehaviourOneShot parent)
    {
        parent.transform.position = parent.Hand.position;

        // If it's an enemy
        if (parent.AICharacter)
        {
            // Moves the final position a little to the back so it won't be 100% in the position of the player.
            // This corrects a bug where the collision wasn't detecting if this point was exactly in the same
            // position as the player.
            parent.PositionOnHit = parent.Hand.position +
                parent.transform.position.Direction(parent.WhoCast.transform.position) * 0.75f;
        }
        else
        // If it's the player
        {
            // Moves collider a little to the front to help with player's accuracy
            parent.PositionOnHit = parent.Eyes.position +
                parent.Eyes.transform.position.Direction(
                    parent.Eyes.position + parent.Eyes.transform.forward) * 1.5f;

            if (parent.Spell.OnHitBehaviourOneShot != null)
            {
                Collider[] allEnemyCollidersHit =
                Physics.OverlapSphere(
                    parent.PositionOnHit, parent.Spell.AreaOfEffect, Layers.EnemyLayer);

                List<Enemy> enemies = new List<Enemy>();
                for (int i = 0; i < allEnemyCollidersHit.Length; i++)
                {
                    if (allEnemyCollidersHit[i].TryGetComponentInParent(out Enemy enemy))
                    {
                        if (enemies.Contains(enemy) == false)
                        {
                            enemies.Add(enemy);
                        }
                    }
                        
                }

                /*
                // Spawns hit, CAN DELETE, not using
                if (enemies.Count > 0)
                {
                    for (int i = 0; i < enemies.Count; i++)
                    {
                        Vector3 positionToSpawnHit = new Vector3(
                            enemies[i].gameObject.transform.position.x,
                            parent.Hand.position.y, 
                            enemies[i].gameObject.transform.position.z);

                        Vector3 positionMargin = positionToSpawnHit +
                            positionToSpawnHit.Direction(parent.Eyes.transform.position);

                        // Spawns hit in direction of collider hit normal
                        SpellHitPoolCreator.Pool.InstantiateFromPool(
                            parent.Spell.Name, positionMargin,
                            Quaternion.LookRotation(positionMargin.Direction(parent.Eyes.transform.position)));
                    }
                }
                */
            }
        }

        parent.SpellStartedMoving = true;
        parent.TimeSpawned = Time.time;
        parent.Spell.DamageBehaviour.Damage(parent);
        parent.transform.rotation = 
            Quaternion.LookRotation(parent.Hand.transform.forward, Vector3.up);
    }

    public override void ContinuousUpdateBeforeSpellBehaviour(SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
    }

    public override void ContinuousUpdateBehaviour(SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
    }

    public override void ContinuousFixedUpdateBehaviour(SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
    }

    public override void HitTriggerBehaviour(Collider other, SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
    }
}
