using UnityEngine;
using UnityEngine.VFX;

/// <summary>
/// Abstract class responsible for executing a spell behaviour.
/// </summary>
public abstract class SpellBehaviourAbstract : MonoBehaviour, IVisualEffect
{
    // Effects
    private VisualEffect[] hitEffectVFX;
    private ParticleSystem[] hitEffectParticleSystem;
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
        hitEffectVFX = GetComponentsInChildren<VisualEffect>();
        hitEffectParticleSystem = GetComponentsInChildren<ParticleSystem>();
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
