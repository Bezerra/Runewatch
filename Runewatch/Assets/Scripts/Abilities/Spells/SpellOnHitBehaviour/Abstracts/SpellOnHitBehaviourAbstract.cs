using UnityEngine;
using UnityEngine.VFX;

/// <summary>
/// Abstract class responsible for on hit monobehaviours.
/// </summary>
public abstract class SpellOnHitBehaviourAbstract : MonoBehaviour, IVisualEffect
{
    /// <summary>
    /// Spell audio source.
    /// </summary>
    public AudioSource AudioS { get; private set; }

    // Only used to have spell information if needed
    [SerializeField] private SpellSO spellInformation;

    /// <summary>
    /// Property used to know spell information if needed.
    /// </summary>
    public SpellSO SpellInformation => spellInformation;

    /// <summary>
    /// Spell interface property to keep spell of which spell spawned this behaviour.
    /// </summary>
    public abstract ISpell Spell { get; set; }

    /// <summary>
    /// Time of spawn.
    /// </summary>
    public abstract float TimeSpawned { get; set; }

    // Variables for casting loop fades
    protected YieldInstruction wffu;

    // Effects
    private VisualEffect[] hitEffectVFX;
    private ParticleSystem[] hitEffectParticleSystem;

    protected virtual void Awake()
    {
        hitEffectVFX = GetComponentsInChildren<VisualEffect>();
        hitEffectParticleSystem = GetComponentsInChildren<ParticleSystem>();
        AudioS = GetComponent<AudioSource>();
        wffu = new WaitForFixedUpdate();
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

    public bool HasEffect => hitEffectVFX != null || hitEffectParticleSystem != null;
}
