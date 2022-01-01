using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Scriptable object responsible for spawning an hover vfx on ground.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Spawn Area Hover Effect On Floor", 
    fileName = "Spell Behaviour Spawn Area Hover Effect On Floor")]
sealed public class SpellBehaviourSpawnAreaHoverEffectOnFloorSO : SpellBehaviourAbstractSO
{
    [SerializeField] private LayerMask layersToCheck;
    [Range(0f, 0.3f)] [SerializeField] private float distanceFromWall = 0.05f;

    private Vector3 DISTANTVECTOR = new Vector3(10000, 10000, 10000);

    public override void StartBehaviour(SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
    }
    public override void ContinuousUpdateBeforeSpellBehaviour(SpellBehaviourOneShot parent)
    {
        if (parent.WhoCast == null)
        {
            if (parent.AreaHoverVFX != null)
            {
                parent.AreaHoverVFX.SetActive(false);
                return;
            }
        }

        // Will be set to true in another movement behaviour start behaviour
        if (parent.ColliderTrigger != null)
            parent.ColliderTrigger.enabled = false;

        // If it's an enemy
        if (parent.WhoCast.CommonAttributes.Type != CharacterType.Player)
        {
            // If time while casting reaches the time limit set on enemy spell list,
            // shows one shot with release spell area.
            // Code only happens once.
            float timeEnemyIsStopped = Time.time - parent.AICharacter.TimeEnemyStoppedWhileAttacking;
            if (timeEnemyIsStopped > parent.AICharacter.CurrentlySelectedSpell.StoppingTime * 
                parent.AICharacter.CurrentlySelectedSpell.PercentageStoppingTimeTriggerAoESpell &&
                parent.AreaHoverVFX == null)
            {
                if (parent.AICharacter.CurrentTarget == null)
                    return;

                Ray playerFloorPosition = new Ray(
                    parent.AICharacter.CurrentTarget.position, Vector3.down);

                if (Physics.Raycast(playerFloorPosition, out RaycastHit floorHit, 5, Layers.WallsFloor))
                {
                    if (parent.AreaHoverVFX == null)
                    {
                        parent.AreaHoverVFX =
                            SpellAreaHoverPoolCreator.Pool.InstantiateFromPool(
                            parent.Spell.Name, DISTANTVECTOR,
                            Quaternion.identity);

                        if (parent.AreaHoverVFX.TryGetComponent(out AreaTargetSizeUpdate targetSize))
                            targetSize.UpdateAreaTargetSize(parent.Spell.AreaOfEffect);
                    }

                    parent.AreaHoverAreaHit = floorHit;

                    parent.AreaHoverVFX.transform.SetPositionAndRotation(
                        floorHit.point + floorHit.normal * distanceFromWall,
                        Quaternion.LookRotation(floorHit.normal, floorHit.collider.transform.up) *
                        Quaternion.Euler(90, 0, 0));
                }
            }
            return;
        }

        // If it's not an enemy, it will execute this code for player

        if (parent.AreaHoverVFX == null)
        {
            parent.AreaHoverVFX =
                SpellAreaHoverPoolCreator.Pool.InstantiateFromPool(
                parent.Spell.Name, DISTANTVECTOR,
                Quaternion.identity);

            if (parent.AreaHoverVFX.TryGetComponent(out AreaTargetSizeUpdate targetSize))
                targetSize.UpdateAreaTargetSize(parent.Spell.AreaOfEffect);
        }

        Ray eyesForward = new Ray(parent.Eyes.position, parent.Eyes.forward);
        if (Physics.Raycast(eyesForward, out RaycastHit objectHit, parent.Spell.MaximumDistance, layersToCheck))
        {
            // Now, it creates a ray from HAND to eyes previous target
            Ray handForward = new Ray(parent.Hand.position, parent.Hand.position.Direction(objectHit.point));
            if (Physics.Raycast(handForward, out RaycastHit handObjectHit, parent.Spell.MaximumDistance, layersToCheck))
            {
                // Now, it creates a ray from that last hit point to the floor
                Ray handHitToFloor = new Ray(
                    handObjectHit.point + handObjectHit.normal * 0.01f, -Vector3.up);

                if (Physics.Raycast(handHitToFloor, out RaycastHit floorHit, 50, Layers.WallsFloorWithoutWallsSpells))
                {
                    // Sets area hover hit
                    parent.AreaHoverAreaHit = floorHit;

                    // Sets position to the raycast hit and rotation to that hit normal
                    parent.AreaHoverVFX.transform.SetPositionAndRotation(
                        floorHit.point + floorHit.normal * distanceFromWall,
                        Quaternion.LookRotation(floorHit.normal, floorHit.collider.transform.up) *
                        Quaternion.Euler(90, 0, 0));

                    return;
                }
                else
                {
                    // Sets position far from the scene
                    parent.AreaHoverVFX.transform.SetPositionAndRotation(
                        DISTANTVECTOR,
                        Quaternion.identity);
                    return;
                }
            }
            else
            {
                // Sets position far from the scene
                parent.AreaHoverVFX.transform.SetPositionAndRotation(
                    DISTANTVECTOR,
                    Quaternion.identity);
                return;
            }
        }
        else
        {
            // If there is no wall, tries to cast a ray to bottom from maximum distance

            Ray noWallRayToFloor = 
                new Ray(parent.Eyes.position + parent.Eyes.forward * parent.Spell.MaximumDistance, -Vector3.up);

            if (Physics.Raycast(noWallRayToFloor, out RaycastHit airToFloorHit, 50, Layers.WallsFloorWithoutWallsSpells))
            {
                // Sets area hover hit
                parent.AreaHoverAreaHit = airToFloorHit;

                // Sets position to the raycast hit and rotation to that hit normal
                parent.AreaHoverVFX.transform.SetPositionAndRotation(
                    airToFloorHit.point + airToFloorHit.normal * distanceFromWall,
                    Quaternion.LookRotation(airToFloorHit.normal, airToFloorHit.collider.transform.up) *
                        Quaternion.Euler(90, 0, 0));
            }
            else
            {
                // Sets position far from the scene
                parent.AreaHoverVFX.transform.SetPositionAndRotation(
                    DISTANTVECTOR,
                    Quaternion.identity);
            }
        }
    }

    public override void ContinuousUpdateBehaviour(SpellBehaviourOneShot parent)
    {
        if (parent.WhoCast == null)
        {
            if (parent.AreaHoverVFX != null)
            {
                parent.AreaHoverVFX.SetActive(false);
                return;
            }
        }

        // If the spell is already in motion, it will disable this game object
        if (parent.SpellStartedMoving == true)
        {
            if (parent.AreaHoverVFX != null)
            {
                // Happens at least once
                if (parent.AreaHoverVFX.activeSelf)
                {
                    parent.AreaHoverVFX.SetActive(false);
                }
                return;
            }
        }
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
