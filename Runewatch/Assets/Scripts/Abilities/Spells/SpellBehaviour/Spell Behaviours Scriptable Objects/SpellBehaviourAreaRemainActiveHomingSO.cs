using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Scriptable object responsible for moving an area spell towards a target.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Homing Area Remain Active", 
    fileName = "Spell Behaviour Homing Area Remain Active")]
sealed public class SpellBehaviourAreaRemainActiveHomingSO : SpellBehaviourAbstractSO
{
    [Range(1, 15f)] [SerializeField] private float homingRange = 10f;

    public override void StartBehaviour(SpellBehaviourOneShot parent)
    {
        // Creates a sphere collider to check every enemy in front of the player
        Collider[] enemyColliders = 
            Physics.OverlapSphere(parent.transform.position, homingRange,
            Layers.EnemySensiblePoint);

        // If it found enemies
        if (enemyColliders.Length > 0)
        {
            float distance = Mathf.Infinity;
            for (int i = 0; i < enemyColliders.Length; i++)
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

    public override void ContinuousUpdateBeforeSpellBehaviour(SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
    }

    public override void ContinuousUpdateBehaviour(SpellBehaviourOneShot parent)
    {
        // If there's a homing target,
        // the spell will find the closest enemy and will move towards it
        if (parent.HomingTarget == null)
        {
            // Creates a sphere collider to check every enemy in front of the player
            Collider[] enemyColliders =
                Physics.OverlapSphere(parent.transform.position, homingRange,
                Layers.EnemySensiblePoint);

            // If it found enemies
            if (enemyColliders.Length > 0)
            {
                float distance = Mathf.Infinity;
                for (int i = 0; i < enemyColliders.Length; i++)
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

    public override void ContinuousFixedUpdateBehaviour(SpellBehaviourOneShot parent)
    {
        // Guides the spell towards the homing target
        if (parent.HomingTarget != null)
        {
            Vector3 direction = parent.transform.position.Direction(parent.HomingTarget.position);
            parent.Rb.velocity = direction * parent.Spell.Speed;
        }  
    }

    public override void HitTriggerBehaviour(Collider other, SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
    }
}
