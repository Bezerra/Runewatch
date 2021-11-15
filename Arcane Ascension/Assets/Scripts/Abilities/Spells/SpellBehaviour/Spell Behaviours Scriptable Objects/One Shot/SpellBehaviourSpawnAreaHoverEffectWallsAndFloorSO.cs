using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Scriptable object responsible for moving the spell forward on start.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Spawn Area Hover Effect Walls And Floor", 
    fileName = "Spell Behaviour Spawn Area Hover Effect Walls And Floor")]
sealed public class SpellBehaviourSpawnAreaHoverEffectWallsAndFloorSO : SpellBehaviourAbstractOneShotSO
{
    [SerializeField] private LayerMask layersToCheck;
    [Range(0f, 0.3f)] [SerializeField] private float distanceFromWall = 0.1f;

    private Vector3 DISTANTVECTOR = new Vector3(10000, 10000, 10000);

    public override void StartBehaviour(SpellBehaviourOneShot parent)
    {
        
    }
    public override void ContinuousUpdateBeforeSpellBehaviour(SpellBehaviourOneShot parent)
    {
        // Will be set to true in another movement behaviour start behaviour
        if (parent.ColliderTrigger != null)
            parent.ColliderTrigger.enabled = false;

        // If it's an enemy
        if (parent.WhoCast.CommonAttributes.Type == CharacterType.Monster)
        {
            // If time while casting reaches the time limit set on enemy spell list,
            // shows one shot with release spell area.
            // Code only happens once.
            float timeEnemyIsStopped = Time.time - parent.AICharacter.TimeEnemyStoppedWhileAttacking;
            if (timeEnemyIsStopped > parent.AICharacter.CurrentlySelectedSpell.StoppingTime *
                parent.AICharacter.CurrentlySelectedSpell.PercentageStoppingTimeTriggerAoESpell &&
                parent.AreaHoverVFX == null)
            {
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
                    }

                    parent.AreaHoverAreaHit = floorHit;

                    parent.AreaHoverVFX.transform.SetPositionAndRotation(
                        floorHit.point + floorHit.normal * distanceFromWall,
                        Quaternion.LookRotation(floorHit.normal, floorHit.collider.transform.up));
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
        }

        Ray eyesForward = new Ray(parent.Eyes.position, parent.Eyes.forward);
        if (Physics.Raycast(eyesForward, out RaycastHit objectHit, 50, layersToCheck))
        {
            // Now, it creates a ray from HAND to eyes previous target
            Ray handForward = new Ray(parent.Hand.position, parent.Hand.position.Direction(objectHit.point));
            if (Physics.Raycast(handForward, out RaycastHit floorHit, 50, layersToCheck))
            {
                // Sets area hover hit
                parent.AreaHoverAreaHit = floorHit;

                // Sets position to the raycast hit and rotation to that hit normal
                parent.AreaHoverVFX.transform.SetPositionAndRotation(
                    floorHit.point + floorHit.normal * distanceFromWall,
                    Quaternion.LookRotation(floorHit.normal, floorHit.collider.transform.up));

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
            // If there is no wall, tries to cast a ray to bottom from maximum distance

            Ray noWallRayToFloor =
                new Ray(parent.Eyes.position + parent.Eyes.forward * parent.Spell.MaximumDistance, -Vector3.up);

            if (Physics.Raycast(noWallRayToFloor, out RaycastHit airToFloorHit, 50, layersToCheck))
            {
                // Sets area hover hit
                parent.AreaHoverAreaHit = airToFloorHit;

                // Sets position to the raycast hit and rotation to that hit normal
                parent.AreaHoverVFX.transform.SetPositionAndRotation(
                    airToFloorHit.point + airToFloorHit.normal * distanceFromWall,
                    Quaternion.LookRotation(airToFloorHit.normal, airToFloorHit.collider.transform.up));
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
        // If the spell is already in motion, it will disable this game object
        if (parent.SpellStartedMoving == true)
        {
            // Happens at least once
            if (parent.AreaHoverVFX.activeSelf)
            {
                parent.AreaHoverVFX.SetActive(false);
            }
            return;
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
