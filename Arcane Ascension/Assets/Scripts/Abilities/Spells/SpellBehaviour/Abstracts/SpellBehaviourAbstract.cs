using System;
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

    /// <summary>
    /// Spell interface property.
    /// </summary>
    public abstract ISpell Spell { get; }

    /// <summary>
    /// Spell audio source.
    /// </summary>
    public AudioSource AudioS { get; private set; }

    /// <summary>
    /// Who cast character's hands.
    /// </summary>
    public Transform Hand { get; private set; }

    /// <summary>
    /// Who cast character's eyes.
    /// </summary>
    public Transform Eyes { get; private set; }

    /// <summary>
    /// Updated when who cast is set.
    /// </summary>
    public LayerMask LayerOfWhoCast { get; set; }

    /// <summary>
    /// Layer of the last character hit.
    /// </summary>
    public LayerMask LayerOfCharacterHit { get; set; }

    /// <summary>
    /// Character hit.
    /// </summary>
    public Stats CharacterHit { get; set; }
    
    /// <summary>
    /// Gets position on spawn or everytime it hits.
    /// </summary>
    public Vector3 PositionOnHit { get; set; }

    /// <summary>
    /// Updated when spawned.
    /// </summary>
    public Vector3 PositionWhenSpawned { get; set; }

    /// <summary>
    /// Used if the character who cast the spell is an enemy.
    /// </summary>
    public Enemy AICharacter { get; private set; }

    /// <summary>
    /// Character damageable interface.
    /// </summary>
    public IDamageable ThisIDamageable { get; private set; }

    /// <summary>
    /// Character mana interface.
    /// </summary>
    public IMana ThisIMana { get; private set; }

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
                LayerOfWhoCast = whoCast.gameObject.layer;
                if (whoCast.TryGetComponent<Character>(out Character character))
                {
                    Hand = character.Hand;
                    PositionWhenSpawned = Hand.transform.position;
                    Eyes = character.Eyes;
                }

                if (whoCast.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    AICharacter = enemy;
                }
                else
                {
                    AICharacter = null;
                }

                ThisIDamageable = WhoCast.GetComponent<IDamageable>();
                ThisIMana = WhoCast.GetComponent<IMana>();
            }
        }
    }
    
    protected virtual void Awake()
    {
        hitEffectVFX = GetComponentsInChildren<VisualEffect>();
        hitEffectParticleSystem = GetComponentsInChildren<ParticleSystem>();
        AudioS = GetComponent<AudioSource>();
    }

    protected virtual void OnEnable()
    {
        PositionOnHit = transform.position;
        if (Hand != null) PositionWhenSpawned = Hand.transform.position;
    }

    protected virtual void OnDisable()
    {
        PositionOnHit = default;
        LayerOfCharacterHit = LayerOfWhoCast;
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

    public bool HasEffect => hitEffectVFX != null || hitEffectParticleSystem != null;
}
