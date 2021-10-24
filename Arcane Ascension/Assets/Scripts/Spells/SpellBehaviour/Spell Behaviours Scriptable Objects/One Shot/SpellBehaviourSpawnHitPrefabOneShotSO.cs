using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Scriptable object responsible for spawning hit prefab.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Spawn Hit Prefab", 
    fileName = "Spell Behaviour Spawn Hit Prefab")]
public class SpellBehaviourSpawnHitPrefabOneShotSO : SpellBehaviourAbstractOneShotSO
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
        // Left blank on purpose
    }

    public override void ContinuousFixedUpdateBehaviour(SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
    }

    public override void HitTriggerBehaviour(Collider other, SpellBehaviourOneShot parent)
    {
        // Direction of the current hit to initial spawn
        Vector3 directionToInitialSpawn = parent.transform.Direction(parent.PositionOnSpawnAndHit);

        Vector3 positionToSpawnHit;

        // Direction to do a raycast.
        // Uses directionToInitialSpawn so the hit will be a little behind the wall to prevent bugs
        Ray direction = new Ray(
            parent.transform.position + directionToInitialSpawn * 0.1f,
            ((parent.transform.position + directionToInitialSpawn * 0.1f).
            Direction(other.ClosestPoint(parent.transform.position))));

        // Calculates rotation of the spell to cast
        Quaternion spellLookRotation;
        if (Physics.Raycast(direction, out RaycastHit spellHitPoint))
        {
            spellLookRotation =
                Quaternion.LookRotation(spellHitPoint.normal, other.transform.up);
            positionToSpawnHit = spellHitPoint.point + spellHitPoint.normal * 0.3f;
        }
        else
        {
            spellLookRotation =
                Quaternion.LookRotation(parent.transform.position.Direction(directionToInitialSpawn),
                    parent.WhoCast.transform.up);
            positionToSpawnHit = parent.transform.position + directionToInitialSpawn * 0.3f;
        }

        // Creates spell hit
        // Update() will run from its monobehaviour script
        if (parent.Spell.OnHitBehaviourOneShot != null &&
            other.gameObject.layer != parent.LayerOfWhoCast)
        {
            // Spawns hit in direction of collider hit normal
            GameObject onHitBehaviourGameObject = SpellHitPoolCreator.Pool.InstantiateFromPool(
                parent.Spell.Name, positionToSpawnHit,
                spellLookRotation);

            if (onHitBehaviourGameObject.TryGetComponent<SpellOnHitBehaviourOneShot>(out SpellOnHitBehaviourOneShot onHitBehaviour))
            {
                onHitBehaviour.Spell = parent.Spell;
            }
        }
    }
}
