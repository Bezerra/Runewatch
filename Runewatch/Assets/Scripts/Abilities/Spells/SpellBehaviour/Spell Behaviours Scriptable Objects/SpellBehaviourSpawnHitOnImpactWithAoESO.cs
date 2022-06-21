using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Scriptable object responsible for spawning hit of AoE hover spell.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Spawn Hit On Impact " +
    "With AoE",
    fileName = "Spell Behaviour Spawn Hit On Impact With AoE")]
public class SpellBehaviourSpawnHitOnImpactWithAoESO : SpellBehaviourAbstractSO
{
    public override void StartBehaviour(SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
    }

    public override void ContinuousUpdateBeforeSpellBehaviour(SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
    }

    public override void ContinuousUpdateBehaviour(SpellBehaviourOneShot parent)
    {
        if (parent.CurrentWallHitQuantity < 1)
            parent.TimeSpawned = Time.time;
    }

    public override void ContinuousFixedUpdateBehaviour(SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
    }

    public override void HitTriggerBehaviour(Collider other, SpellBehaviourOneShot parent)
    {
        parent.CurrentWallHitQuantity++;
        parent.TimeOfImpact = Time.time;

        // Safe margin to do calculations
        Vector3 positionSafeMargin;

        // Direction 
        Vector3 direction;

        // Ray to desired direction
        Ray rayToDirection;

        if (other.TryGetComponentInParent(out SelectionBase character))
        {
            // Creates a ray from the previous obtained position to floor
            rayToDirection = new Ray(parent.transform.position, Vector3.down);
        }
        else
        {
            // Creates a safe margin from the spell position to the previous position
            positionSafeMargin = (parent.transform.position - parent.Rb.velocity.normalized);

            // Calculates direction to closest point near the spell
            direction = positionSafeMargin.Direction(other.ClosestPoint(parent.transform.position));

            // Creates a ray from the previous obtained position to the closest point
            rayToDirection = new Ray(positionSafeMargin, direction);
        }

        // If there is a wall or floor on that closest point
        if (Physics.Raycast(rayToDirection, out RaycastHit hit, 5, Layers.WallsFloor))
        {
            // Spawns hit in direction of collider hit normal
            GameObject onHitBehaviourGameObject = SpellHitPoolCreator.Pool.InstantiateFromPool(
                parent.Spell.Name, hit.point,
                Quaternion.LookRotation(hit.normal) * Quaternion.Euler(90, 0, 0));

            if (onHitBehaviourGameObject.TryGetComponent<SpellOnHitBehaviourOneShot>(
                out SpellOnHitBehaviourOneShot onHitBehaviour))
            {
                // Sets hit Spell to this spell
                if (onHitBehaviour.Spell != parent.Spell)
                    onHitBehaviour.Spell = parent.Spell;
            }
        }
    }
}
