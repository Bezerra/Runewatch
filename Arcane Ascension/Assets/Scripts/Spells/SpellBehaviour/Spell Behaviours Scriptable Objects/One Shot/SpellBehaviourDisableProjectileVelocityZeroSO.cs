using UnityEngine;

/// <summary>
/// Scriptable object responsible for disable parent spell behaviour when velocity is zero.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/Spell Behaviour Disable Projectile If Velocity Zero", 
    fileName = "Spell Behaviour Disable Projectile If Velocity Zero")]
public class SpellBehaviourDisableProjectileVelocityZeroSO : SpellBehaviourAbstractOneShotSO
{
    [Header("This variables is used to disable the spell after colliding with something")]
    [Range(1, 10)] [SerializeField] private float disableAfterSecondsAfterCollision;

    public override void StartBehaviour(SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
    }

    public override void ContinuousUpdateBehaviour(SpellBehaviourOneShot parent)
    {
        // If the spell hits something
        if (parent.Rb.velocity == Vector3.zero)
        {
            if (Time.time - parent.TimeOfImpact > disableAfterSecondsAfterCollision)
                DisableSpell(parent);
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
