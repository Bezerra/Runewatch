using UnityEngine;

/// <summary>
/// Scriptable object responsible for setting projectile on the floor.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Set Projectile On Floor", 
    fileName = "Spell Behaviour Set Projectile On Floor")]
sealed public class SpellBehaviourSetProjectileOnFloorSO : SpellBehaviourAbstractSO
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
        // Sets the projectile close to the floor
        if (Physics.Raycast(
            new Ray(parent.transform.position + Vector3.up, Vector3.down),
            out RaycastHit floorHit, 10f, Layers.WallsFloor))
        {
            parent.transform.position = new Vector3(
                parent.transform.position.x,
                floorHit.point.y + floorHit.normal.y * 0.2f,
                parent.transform.position.z);
        }
    }

    public override void HitTriggerBehaviour(Collider other, SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
    }
}
