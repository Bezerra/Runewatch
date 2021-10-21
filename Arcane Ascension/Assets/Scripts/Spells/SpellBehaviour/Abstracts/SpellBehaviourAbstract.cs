using UnityEngine;
using UnityEngine.VFX;

/// <summary>
/// Abstract class responsible for executing a spell behaviour.
/// </summary>
public abstract class SpellBehaviourAbstract : MonoBehaviour, IVisualEffect
{
    // Effects
    private VisualEffect hitEffectVFX;
    private ParticleSystem hitEffectParticleSystem;
    public abstract ISpell Spell { get; }
    public Transform Hand { get; private set; }
    public Transform Eyes { get; private set; }
    public IDamageable ThisIDamageable { get; private set; }
    private Stats whoCast;
    public Stats WhoCast 
    { 
        get => whoCast; 
        set 
        {
            // Updates variables if who cast is different from the last time this spell was cast
            if (whoCast != value)
            {
                whoCast = value;
                Hand = WhoCast.GetComponent<Character>().Hand;
                Eyes = WhoCast.GetComponent<Character>().Eyes;
                ThisIDamageable = WhoCast.GetComponent<IDamageable>();
            }
        }
    }

    protected virtual void Awake()
    {
        hitEffectVFX = GetComponentInChildren<VisualEffect>();
        hitEffectParticleSystem = GetComponentInChildren<ParticleSystem>();
    }

    /// <summary>
    /// Method called after instantiating the spell.
    /// Must be called manually through this method instead of OnEnable or Start in order to prevent bugs.
    /// </summary>
    public abstract void TriggerStartBehaviour();

    /// <summary>
    /// Immediatly disables spell gameobject.
    /// </summary>
    public void DisableSpell()
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
