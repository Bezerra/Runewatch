using UnityEngine;

/// <summary>
/// Scriptable object responsible for disable parent spell behaviour if spell is too distant from the caster.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Disable Projectile At Maximum Distance", 
    fileName = "Spell Behaviour Disable Projectile At Maximum Distance")]
public class SpellBehaviourDisableProjectileAtMaximumDistanceSO : SpellBehaviourAbstractSO
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
        if (parent.WhoCast != null)
        {
            if (Vector3.Distance(parent.transform.position, parent.WhoCast.transform.position) >    
                parent.Spell.MaximumDistance)
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

                if (parent.HasEffect)
                {
                    parent.ColliderTrigger.enabled = false;

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
                }
                else
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
