using UnityEngine;

/// <summary>
/// Scriptable object responsible for disable parent spell behaviour when velocity is zero.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Disable Projectile Spell Max Time", 
    fileName = "Spell Behaviour Disable Projectile Spell Max Time")]
public class SpellBehaviourDisableProjectileSpellMaxTimeSO : SpellBehaviourAbstractOneShotSO
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
        // If the spell hits something
        if (parent.SpellStartedMoving)
        {
            if (Time.time - parent.TimeSpawned > parent.Spell.MaxTime)
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
            if (Time.time - parent.TimeSpawned > parent.Spell.MaxTime * 3)
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
