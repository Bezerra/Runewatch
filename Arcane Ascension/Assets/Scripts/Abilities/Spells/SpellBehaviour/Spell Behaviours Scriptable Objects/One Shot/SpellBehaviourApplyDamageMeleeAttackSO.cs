using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Scriptable object responsible for melee attack behaviours.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Apply Damage Melee Attack", 
    fileName = "Spell Behaviour Apply Damage Melee Attack")]
sealed public class SpellBehaviourApplyDamageMeleeAttackSO : SpellBehaviourAbstractOneShotSO
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
            parent.PositionOnHit = parent.Hand.position +
                parent.WhoCast.transform.position.Direction(parent.Hand.position) * 1.5f;

            /*
            Colliders[] colliderHit = Physics.OverlapSphere

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
            */
        }

        parent.SpellStartedMoving = true;
        parent.TimeSpawned = Time.time;
        parent.Spell.DamageBehaviour.Damage(parent);
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
