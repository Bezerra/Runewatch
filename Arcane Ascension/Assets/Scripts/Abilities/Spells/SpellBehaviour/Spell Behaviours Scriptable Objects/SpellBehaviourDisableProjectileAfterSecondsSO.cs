using UnityEngine;

/// <summary>
/// Scriptable object responsible for disabling projectile after seconds.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Disable Projectile After Seconds", 
    fileName = "Spell Behaviour Disable Projectile After Seconds")]
sealed public class SpellBehaviourDisableProjectileAfterSecondsSO : SpellBehaviourAbstractSO
{
    [Header("This variables is used to disable the spell after X seconds")]
    [Range(1, 10)] [SerializeField] private float disableAfterSeconds;

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
        if (parent.SpellStartedMoving)
        {
            // One shot spells
            if (parent.Rb != null)
            {
                if (parent.Rb.velocity != Vector3.zero)
                {
                    if (Time.time - parent.TimeSpawned > disableAfterSeconds)
                    {
                        if (parent.HasEffect)
                        {
                            if (parent.ParticlesDisable != null &&
                                parent.ParticlesDisable.Length > 0)
                            {
                                foreach (ParticleDisable dis in parent.ParticlesDisable)
                                {
                                    switch (dis.DisableType)
                                    {
                                        case DisableType.Fade:
                                            parent.EffectStop();
                                            break;
                                        case DisableType.Immediate:
                                            dis.gameObject.SetActive(false);
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                parent.EffectStop();
                            }

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
                    if (Time.time - parent.TimeSpawned > disableAfterSeconds * 3)
                    {
                        parent.DisableSpell();
                    }
                }
            }
            // One shot spells with release
            else
            {
                if (Time.time - parent.TimeSpawned > disableAfterSeconds)
                {
                    if (parent.HasEffect)
                    {
                        parent.EffectStop();

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
                if (Time.time - parent.TimeSpawned > disableAfterSeconds * 3)
                {
                    parent.DisableSpell();
                }
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
