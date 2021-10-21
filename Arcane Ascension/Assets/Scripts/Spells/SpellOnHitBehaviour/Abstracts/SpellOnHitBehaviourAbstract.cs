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
    private VisualEffect hitEffectVFX;
    private ParticleSystem hitEffectParticleSystem;

    private void Awake()
    {
        hitEffectVFX = GetComponentInChildren<VisualEffect>();
        hitEffectParticleSystem = GetComponentInChildren<ParticleSystem>();
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
        if (hitEffectVFX != null) hitEffectVFX.Play();
        if (hitEffectParticleSystem != null) hitEffectParticleSystem.Play();
    }

    public void EffectStop()
    {
        if (hitEffectVFX != null) hitEffectVFX.Stop();
        if (hitEffectParticleSystem != null) hitEffectParticleSystem.Stop();
    }

    public int EffectGetAliveParticles
    {
        get
        {
            if (hitEffectVFX != null) return hitEffectVFX.aliveParticleCount;
            if (hitEffectParticleSystem != null) return hitEffectParticleSystem.particleCount;
            return 0;
        }
    }

    public bool EffectNotNull => hitEffectVFX != null || hitEffectParticleSystem != null;
}
