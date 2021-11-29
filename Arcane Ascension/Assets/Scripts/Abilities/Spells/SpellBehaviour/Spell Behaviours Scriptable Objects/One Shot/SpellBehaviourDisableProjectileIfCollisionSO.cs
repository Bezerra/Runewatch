using UnityEngine;

/// <summary>
/// Scriptable object responsible for disable parent spell behaviour when velocity is zero.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Disable Projectile If Collision", 
    fileName = "Spell Behaviour Disable Projectile If Collision")]
public class SpellBehaviourDisableProjectileIfCollisionSO : SpellBehaviourAbstractOneShotSO
{
    [Header("This variables is used to disable the spell after colliding with something")]
    [Range(1, 10)] [SerializeField] private float disableAfterSecondsAfterCollision;

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
        // If the spell hits something
        if (parent.DisableSpellAfterCollision && parent.SpellStartedMoving)
        {
            if (parent.Rb.velocity == Vector3.zero)
            {
                if (parent.HasEffect)
                {
                    parent.ColliderTrigger.enabled = false;
                    parent.EffectStop();
                }
            }

            if (Time.time - parent.TimeOfImpact > disableAfterSecondsAfterCollision)
            {
                if (parent.HasEffect)
                {
                    if (parent.EffectGetAliveParticles == 0)
                    {
                        parent.DisableSpell();
                    }
                }
                else
                {
                    parent.DisableSpell();
                }
            }

            // Safety measure if too much time passes and the effect didn't get disabled
            if (Time.time - parent.TimeSpawned > disableAfterSecondsAfterCollision * 3)
            {
                parent.DisableSpell();
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
