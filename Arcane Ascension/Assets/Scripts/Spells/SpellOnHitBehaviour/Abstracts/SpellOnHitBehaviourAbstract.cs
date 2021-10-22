using UnityEngine;
using UnityEngine.VFX;

/// <summary>
/// Abstract class responsible for on hit monobehaviours.
/// </summary>
public abstract class SpellOnHitBehaviourAbstract : MonoBehaviour, IVisualEffect
{
    public abstract ISpell Spell { get; set; }

    public abstract float TimeSpawned { get; set; }

    // Effects
    private VisualEffect[] hitEffectVFX;
    private ParticleSystem[] hitEffectParticleSystem;

    protected virtual void Awake()
    {
        hitEffectVFX = GetComponentsInChildren<VisualEffect>();
        hitEffectParticleSystem = GetComponentsInChildren<ParticleSystem>();
    }

    /// <summary>
    /// Disables spell Muzzle.
    /// </summary>
    /// <param name="parent"></param>
    public void DisableHitSpell()
    {
        gameObject.SetActive(false);
    }

    public void EffectPlay()
    {
        if (hitEffectVFX != null)
        {
            foreach (VisualEffect visualEffect in hitEffectVFX)
                visualEffect.Play();
        }
        if (hitEffectParticleSystem != null)
        {
            foreach (ParticleSystem particleSystem in hitEffectParticleSystem)
                particleSystem.Play();
        }

    }

    public void EffectStop()
    {
        if (hitEffectVFX != null)
        {
            foreach (VisualEffect visualEffect in hitEffectVFX)
                visualEffect.Stop();
        }
        if (hitEffectParticleSystem != null)
        {
            foreach (ParticleSystem particleSystem in hitEffectParticleSystem)
                particleSystem.Stop();
        }
    }

    public int EffectGetAliveParticles
    {
        get
        {
            int result = 0;

            if (hitEffectVFX != null)
            {
                foreach (VisualEffect visualEffect in hitEffectVFX)
                    result += visualEffect.aliveParticleCount;
            }
            if (hitEffectParticleSystem != null)
            {
                foreach (ParticleSystem particleSystem in hitEffectParticleSystem)
                    result += particleSystem.particleCount;
            }

            return result;
        }
    }

    public bool EffectNotNull => hitEffectVFX != null || hitEffectParticleSystem != null;
}
