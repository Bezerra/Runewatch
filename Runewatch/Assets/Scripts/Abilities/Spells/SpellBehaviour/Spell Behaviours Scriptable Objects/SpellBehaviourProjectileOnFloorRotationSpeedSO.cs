using UnityEngine;

/// <summary>
/// Scriptable object responsible for rotating the projectile to player's forward and updating its speed.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Projectile On Floor " +
    "Rotation and Speed", 
    fileName = "Spell Behaviour Projectile On Floor Rotation and Speed")]
sealed public class SpellBehaviourProjectileOnFloorRotationSpeedSO : SpellBehaviourAbstractSO
{
    public override void StartBehaviour(SpellBehaviourOneShot parent)
    {
        parent.transform.rotation = Quaternion.LookRotation(parent.WhoCast.transform.forward, Vector3.up);
        parent.Rb.velocity = parent.transform.forward * parent.Spell.Speed;
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
