using UnityEngine;
using UnityEngine.VFX;
using ExtensionMethods;

/// <summary>
/// Scriptable object responsible for moving the spell forward on start.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Forward", 
    fileName = "Spell Behaviour Forward")]
sealed public class SpellBehaviourSpawnAreaHoverEffectSO : SpellBehaviourAbstractOneShotSO
{
    public override void StartBehaviour(SpellBehaviourOneShot parent)
    {
        parent.AreaHoverVFX =
            SpellAreaHoverPoolCreator.Pool.InstantiateFromPool(
                parent.Spell.Name, Vector3.zero,
                Quaternion.identity);
    }

    public override void ContinuousUpdateBehaviour(SpellBehaviourOneShot parent)
    {
        Ray eyesForward = new Ray(parent.Eyes.position, parent.Eyes.forward);
        Vector3 eyesTarget;
        if (Physics.Raycast(eyesForward, out RaycastHit objectHit, 100))
        {
            // If it collides against a wall gets a point
            eyesTarget = objectHit.point;
        }
        else
        {
            eyesTarget = default;
        }
        // Now, it creates a ray from HAND to eyes previous target
        Ray handForward = new Ray(parent.Hand.position, parent.Hand.position.Direction(eyesTarget));
        Vector3 finalTarget;
        if (Physics.Raycast(handForward, out RaycastHit handObjectHit, 100))
        {
            // If it its something, then it will lerp spell distance into that point
            finalTarget = handObjectHit.point;
        }
        else
        {
            // Else it will grow forward until spell's max distance
            finalTarget = eyesTarget;
        }

        /*
        if (parent.SpellStartedMoving == false)
        {
            parent.AreaHoverVFX
        }
        else
        {
            parent.AreaHoverVFX.gameObject.SetActive(false);
        }
        */
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
