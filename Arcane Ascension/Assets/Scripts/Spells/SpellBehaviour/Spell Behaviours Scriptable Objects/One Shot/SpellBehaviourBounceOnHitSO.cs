using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Scriptable object responsible for bouncing on hit.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Bounce On Hit", 
    fileName = "Spell Behaviour Bounce On Hit")]
public class SpellBehaviourBounceOnHitSO : SpellBehaviourAbstractOneShotSO
{
    [Header("Walls and floor layer numbers")]
    [SerializeField] private List<int> layersToStopTheSpell;
    [Range(2, 20)] [SerializeField] private byte hitQuantity;

    public override void StartBehaviour(SpellBehaviourOneShot parent)
    {
        parent.PositionOnSpawnAndHit = new Vector3(
            parent.transform.position.x, parent.transform.position.y, parent.transform.position.z);
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

    /// <summary>
    /// On trigger, rotates the projectle if current hit quantity isn't max quantity yet.
    /// </summary>
    /// <param name="other">Other collider.</param>
    /// <param name="parent">Parent behaviour.</param>
    public override void HitTriggerBehaviour(Collider other, SpellBehaviourOneShot parent)
    {
        if (layersToStopTheSpell.Contains(other.gameObject.layer))
        {
            if (++parent.CurrentWallHitQuantity < hitQuantity)
            {
                RotateProjectile(other, parent);
            }
            else
            {
                parent.DisableSpellAfterCollision = true;
                parent.Rb.velocity = Vector3.zero;
            }
        }
        else
        {
            parent.DisableSpellAfterCollision = true;
        }
    }

    /// <summary>
    /// Reflects projectile.
    /// </summary>
    /// <param name="other"></param>
    /// <param name="parent"></param>
    private void RotateProjectile(Collider other, SpellBehaviourOneShot parent)
    {
        // Direction of the current hit to initial spawn
        Vector3 directionToInitialSpawn = parent.transform.Direction(parent.PositionOnSpawnAndHit);

        // Direction to do a raycast.
        // Uses directionToInitialSpawn so the hit will be a little behind the wall to prevent bugs
        Ray direction = new Ray(
            parent.transform.position + directionToInitialSpawn * 0.1f, 
            ((parent.transform.position + directionToInitialSpawn * 0.1f).
            Direction(other.ClosestPoint(parent.transform.position))));

        // Reflects the current movement vector of the spell
        if (Physics.Raycast(direction, out RaycastHit spellHitPoint))
        {
            Vector3 reflection = Vector3.Reflect(parent.Rb.velocity, spellHitPoint.normal).normalized;

            // Sets new speed based on rotation
            parent.Rb.velocity = reflection * parent.Spell.Speed;

            // Rotates the projectile towards that new speed vector
            parent.transform.rotation = 
                Quaternion.LookRotation(parent.Rb.velocity.Direction(parent.Rb.velocity+reflection), Vector3.up);

            // Sets the last position of the hit to the current hit
            parent.PositionOnSpawnAndHit = 
                new Vector3(
                    parent.transform.position.x, 
                    parent.transform.position.y, 
                    parent.transform.position.z);
        }
    }
}
