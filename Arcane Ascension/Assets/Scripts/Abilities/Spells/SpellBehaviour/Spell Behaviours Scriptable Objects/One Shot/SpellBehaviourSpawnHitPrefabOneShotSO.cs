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
        Vector3 directionToInitialSpawn = parent.transform.Direction(parent.PositionOnHit);

        Vector3 positionToSpawnHit;

        // Direction to do a raycast.
        // Uses directionToInitialSpawn so the hit will be a little behind the wall to prevent bugs
        Ray direction = new Ray(
            parent.transform.position + directionToInitialSpawn * 0.1f,
            ((parent.transform.position + directionToInitialSpawn * 0.1f).
            Direction(other.ClosestPoint(parent.transform.position))));

        RaycastHit spellHitPoint;
        // Calculates rotation of the spell to cast
        Quaternion spellLookRotation;
        if (Physics.Raycast(direction, out spellHitPoint))
        {
            spellLookRotation =
                Quaternion.LookRotation(spellHitPoint.normal, other.transform.up);
            positionToSpawnHit = spellHitPoint.point + spellHitPoint.normal * 0.3f;
        }
        else
        {
            spellLookRotation =
                Quaternion.LookRotation(parent.transform.position.Direction(directionToInitialSpawn),
                    parent.transform.up);
            positionToSpawnHit = parent.transform.position + directionToInitialSpawn * 0.3f;
        }

        // This piece of code prevents undesired hit spawns, and ONLY happens if projectile wasn't reflected.
        // This variable is set on SpelLBehaviourBounce.
        // For ex: if the player shoots upwards on the corner of a wall, the detection
        // will happen on the next frame and it will spawn the hit prefab on the other side of the wall
        if (parent.ProjectileReflected == false)
        {
            if (Vector3.Dot(parent.transform.forward, spellHitPoint.normal) > 0)
                return;
        }

        // Creates spell hit
        // If Hit prefab exists and layer is different than who cast
        // Update() will run from its monobehaviour script
        if (parent.Spell.OnHitBehaviourOneShot != null &&
            other.gameObject.layer != parent.LayerOfWhoCast)
        {
            // Spawns hit in direction of collider hit normal
            GameObject onHitBehaviourGameObject = SpellHitPoolCreator.Pool.InstantiateFromPool(
                parent.Spell.Name, positionToSpawnHit,
                spellLookRotation);

            // If the collider hit has a surface, it will player a sound set to that surface,
            // else it will play a default sound if it has one, else it doesn't play any sound
            if (onHitBehaviourGameObject.TryGetComponent<SpellOnHitBehaviourOneShot>(
                out SpellOnHitBehaviourOneShot onHitBehaviour))
            {
                // Sets hit spell that spawned it
                if (onHitBehaviour.Spell != parent.Spell)
                    onHitBehaviour.Spell = parent.Spell;

                if (other.TryGetComponent(out ISurface surface))
                {
                    if (parent.Spell.SurfaceSounds.ContainsKey(surface.SurfaceType))
                    {
                        parent.Spell.SurfaceSounds[surface.SurfaceType].PlaySound(onHitBehaviour.AudioS);
                    }
                    else
                    {
                        // If there's a sound and hit is not an enemy
                        if (parent.Spell.Sounds.Hit != null)
                        {
                            onHitBehaviour.Spell.Sounds.Hit.PlaySound(onHitBehaviour.AudioS);
                        }
                    }
                }
                else
                {
                    // If there's a sound and hit is not an enemy
                    if (parent.Spell.Sounds.Hit != null)
                    {
                        onHitBehaviour.Spell.Sounds.Hit.PlaySound(onHitBehaviour.AudioS);
                    }
                }
            }
        }
    }
}
