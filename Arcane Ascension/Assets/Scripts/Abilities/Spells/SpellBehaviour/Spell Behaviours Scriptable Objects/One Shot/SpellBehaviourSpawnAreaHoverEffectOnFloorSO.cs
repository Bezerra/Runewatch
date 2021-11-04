using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Scriptable object responsible for moving the spell forward on start.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Spawn Area Hover Effect On Floor", 
    fileName = "Spell Behaviour Spawn Area Hover Effect On Floor")]
sealed public class SpellBehaviourSpawnAreaHoverEffectOnFloorSO : SpellBehaviourAbstractOneShotSO
{
    [SerializeField] private LayerMask layersToCheck;
    [Range(0f, 0.3f)] [SerializeField] private float distanceFromWall = 0.1f;

    private Vector3 DISTANTVECTOR = new Vector3(10000, 10000, 10000);

    public override void StartBehaviour(SpellBehaviourOneShot parent)
    {
        
    }
    public override void ContinuousUpdateBeforeSpellBehaviour(SpellBehaviourOneShot parent)
    {
        if (parent.AreaHoverVFX == null)
        {
            parent.AreaHoverVFX =
                SpellAreaHoverPoolCreator.Pool.InstantiateFromPool(
                parent.Spell.Name, DISTANTVECTOR,
                Quaternion.identity);
        }

        // Will be set to true in another movement behaviour start behaviour
        parent.ColliderTrigger.enabled = false;

        Ray eyesForward = new Ray(parent.Eyes.position, parent.Eyes.forward);
        if (Physics.Raycast(eyesForward, out RaycastHit objectHit, 100, layersToCheck))
        {
            // Now, it creates a ray from HAND to eyes previous target
            Ray handForward = new Ray(parent.Hand.position, parent.Hand.position.Direction(objectHit.point));
            if (Physics.Raycast(handForward, out RaycastHit handObjectHit, 100, layersToCheck))
            {
                // Now, it creates a ray from that last hit point to the floor
                Ray handHitToFloor = new Ray(
                    handObjectHit.point + handObjectHit.normal, -Vector3.up);

                if (Physics.Raycast(handHitToFloor, out RaycastHit floorHit, 100, layersToCheck))
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
                parent.AreaHoverVFX.gameObject.SetActive(false);
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
