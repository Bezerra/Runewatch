using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Scriptable object responsible for moving the spell towards a target.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Homing", 
    fileName = "Spell Behaviour Homing")]
sealed public class SpellBehaviourHomingSO : SpellBehaviourAbstractOneShotSO
{
    public override void StartBehaviour(SpellBehaviourOneShot parent)
    {
        // Creates a box collider to check every enemy in front of the player
        Collider[] enemyColliders = 
            Physics.OverlapBox(parent.transform.position, 
            new Vector3(1.5f, 1.5f, parent.Spell.MaximumDistance), 
            Quaternion.LookRotation(parent.transform.forward), 
            Layers.EnemySensiblePoint);

        // If it found enemies
        if (enemyColliders.Length > 0)
        {
            float distance = Mathf.Infinity;
            for (int i = 0; i < enemyColliders.Length; i++)
            {
                // If there is nothing blocking the enemy
                if (parent.transform.CanSee(enemyColliders[i].transform, 
                    Layers.EnemySensiblePointWallsFloor))
                {
                    // If the distance to this enemy is less than the current distance calculated
                    // for all enemies
                    if (Vector3.Distance(
                        parent.transform.position, enemyColliders[i].transform.position) < distance)
                    {
                        // Sets that distance and homing target
                        distance = Vector3.Distance(
                            parent.transform.position, enemyColliders[i].transform.position);

                        parent.HomingTarget = enemyColliders[i].transform;
                    }
                }
            }
        }
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
        // Guides the spell towards the homing target
        if (parent.HomingTarget != null)
        {
            parent.transform.rotation = 
                Quaternion.LookRotation(parent.transform.Direction(parent.HomingTarget.position));
            parent.Rb.velocity = parent.transform.forward * parent.Spell.Speed;
        }
    }

    public override void HitTriggerBehaviour(Collider other, SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
    }
}
