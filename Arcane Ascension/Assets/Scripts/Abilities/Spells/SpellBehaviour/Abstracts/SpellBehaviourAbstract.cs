using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

/// <summary>
/// Abstract class responsible for executing a spell behaviour.
/// </summary>
public abstract class SpellBehaviourAbstract : MonoBehaviour, IVisualEffect, IReset
{
    private Vector3 DISTANTVECTOR = new Vector3(10000, 10000, 10000);

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

    /// <summary>
    /// Property set if who cast type was the player.
    /// </summary>
    public PlayerCastSpell PlayerCastSpell { get; set; }

    /// <summary>
    /// Property only used for player spells logic. Is set to true
    /// in the exact moment the player releases the attack key.
    /// </summary>
    public bool AttackButtonReleased { get; private set; }

    // Sound fade variables
    private YieldInstruction wffu;
    private IEnumerator soundFadeCoroutine;

    private Stats whoCast;
    public Stats WhoCast 
    { 
        get => whoCast; 
        set 
        {
            if (value == null)
                return;

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

                if (whoCast.CommonAttributes.Type == CharacterType.Player)
                {
                    PlayerCastSpell = 
                        whoCast.GetComponentInChildren<PlayerCastSpell>();
                    PlayerCastSpell.ReleasedAttackButton += ReleasedAttackButtonControl;

                    ThisIMana = WhoCast.GetComponent<IMana>();
                }

                if (whoCast.TryGetComponent(out Enemy enemy))
                {
                    AICharacter = enemy;
                }
                else
                {
                    AICharacter = null;
                }

                ThisIDamageable = WhoCast.GetComponent<IDamageable>();
                whoCast.EventDeath += DisableSpell;
            }
        }
    }

    protected virtual void Awake()
    {
        hitEffectVFX = GetComponentsInChildren<VisualEffect>();
        hitEffectParticleSystem = GetComponentsInChildren<ParticleSystem>();
        AudioS = GetComponent<AudioSource>();
        wffu = new WaitForFixedUpdate();
    }

    protected virtual void OnEnable()
    {
        PositionOnHit = transform.position;

        if (Hand != null) 
            PositionWhenSpawned = Hand.transform.position;
    }

    protected virtual void OnDisable()
    {
        PositionOnHit = default;
        LayerOfCharacterHit = LayerOfWhoCast;
        AttackButtonReleased = false;

        if (PlayerCastSpell != null)
            PlayerCastSpell.ReleasedAttackButton -= ReleasedAttackButtonControl;

        if (whoCast != null)
            whoCast.EventDeath -= DisableSpell;
    }

    public void ResetAfterPoolDisable()
    {
        CharacterHit = null;
        WhoCast = null;
        ThisIDamageable = null;
        AICharacter = null;
        PlayerCastSpell = null;
        AttackButtonReleased = false;
        Eyes = null;
        Hand = null;

        if (PlayerCastSpell != null)
            PlayerCastSpell.ReleasedAttackButton -= ReleasedAttackButtonControl;
    }

    /// <summary>
    /// Sets AttackButtonReleased to true every time the player releases attack button.
    /// </summary>
    private void ReleasedAttackButtonControl() =>
        AttackButtonReleased = true;

    /// <summary>
    /// Method called after instantiating the spell.
    /// Must be called manually through this method instead of OnEnable or Start in order to prevent bugs.
    /// </summary>
    public abstract void TriggerStartBehaviour();

    /// <summary>
    /// Immediatly disables spell through death eventgameobject.
    /// </summary>
    public virtual void DisableSpell(Stats emptyVar) => DisableSpell();

    /// <summary>
    /// Immediatly disables spell gameobject.
    /// </summary>
    public void DisableSpell()
    {
        gameObject.SetActive(false);

        if (soundFadeCoroutine != null)
        {
            StopCoroutine(soundFadeCoroutine);
            soundFadeCoroutine = null;
        }
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

    /// <summary>
    /// Fades in or fades out sounds.
    /// </summary>
    /// <param name="condition"></param>
    public void SoundFadeControl(bool condition)
    {
        if (condition)
        {
            soundFadeCoroutine = FadeInCoroutine();
            StartCoroutine(soundFadeCoroutine);
        }
        else
        {
            if (soundFadeCoroutine != null)
            {
                StopCoroutine(soundFadeCoroutine);
                soundFadeCoroutine = FadeOutCoroutine();
                StartCoroutine(soundFadeCoroutine);
            }
        }
    }

    /// <summary>
    /// Fades out volume.
    /// </summary>
    /// <returns>Wait for fixed update.</returns>
    public IEnumerator FadeOutCoroutine()
    {
        while (AudioS.volume > 0)
        {
            if (gameObject.activeSelf == false) break;

            AudioS.volume -= Time.deltaTime;

            if (AudioS.volume < 0)
            {
                AudioS.volume = 0;
                break;
            }

            yield return wffu;
        }
    }

    /// <summary>
    /// Fades in volume.
    /// </summary>
    /// <returns>Wait for fixed update.</returns>
    public IEnumerator FadeInCoroutine()
    {
        AudioS.volume = 0;

        while (AudioS.volume < Spell.Sounds.Projectile.Volume)
        {
            AudioS.volume += Time.deltaTime;
  
            if (AudioS.volume > Spell.Sounds.Projectile.Volume)
            {
                AudioS.volume = Spell.Sounds.Projectile.Volume;
                break;
            }

            yield return wffu;
        }
    }
}
