using UnityEngine;
using UnityEngine.VFX;

/// <summary>
/// Abstract Monobehaviour for spell muzzles.
/// </summary>
public abstract class SpellMuzzleBehaviourAbstract : MonoBehaviour, IVisualEffect
{
    // Effects
    private VisualEffect[] hitEffectVFX;
    private ParticleSystem[] hitEffectParticleSystem;

    /// <summary>
    /// Spell audio source.
    /// </summary>
    public AudioSource AudioS { get; private set; }

    /// <summary>
    /// This variable is set on spell behaviour after the spell is cast.
    /// </summary>
    public abstract ISpell Spell { get; set; }

    public float TimeSpawned { get; set; }

    protected virtual void Awake()
    {
        AudioS = GetComponent<AudioSource>();
        hitEffectVFX = GetComponentsInChildren<VisualEffect>();
        hitEffectParticleSystem = GetComponentsInChildren<ParticleSystem>();
    }

    /// <summary>
    /// Disables spell Muzzle.
    /// </summary>
    /// <param name="parent"></param>
    public void DisableMuzzleSpell()
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
