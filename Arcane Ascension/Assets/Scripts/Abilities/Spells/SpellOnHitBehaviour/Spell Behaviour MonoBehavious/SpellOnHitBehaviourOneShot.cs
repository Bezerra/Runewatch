using System.Collections;
using UnityEngine;

/// <summary>
/// Monobehaviour for one shot spell hits.
/// </summary>
public class SpellOnHitBehaviourOneShot : SpellOnHitBehaviourAbstract
{
    /// <summary>
    /// This property is set on spell behaviour after the spell is cast.
    /// </summary>
    public override ISpell Spell { get; set; }

    /// <summary>
    /// Property used to know the time the spell was spawned.
    /// </summary>
    public override float TimeSpawned { get; set; }

    /// <summary>
    /// Property used to know if the class already played a sound.
    /// </summary>
    public bool PlayedSound { get; set; }

    /// <summary>
    /// Property used to know if the class is fading out the sound.
    /// </summary>
    public bool FadingOutSound { get; set; }

    /// <summary>
    /// Runs start behaviour when enabled with with pool.
    /// </summary>
    private void OnEnable()
    {
        TimeSpawned = Time.time;
        EffectPlay();

        if (Spell != null)
        {
            foreach (SpellOnHitBehaviourAbstractSO onHitBehaviour in Spell.OnHitBehaviourOneShot)
                onHitBehaviour.StartBehaviour(this);
        }
    }

    /// <summary>
    /// Keeps running on update.
    /// </summary>
    private void Update()
    {
        if (Spell != null)
        {
            foreach (SpellOnHitBehaviourAbstractSO onHitBehaviour in Spell.OnHitBehaviourOneShot)
                onHitBehaviour.ContinuousUpdateBehaviour(this);
        }
    }

    private void OnDisable()
    {
        PlayedSound = false;
        FadingOutSound = false;
    }

    /// <summary>
    /// Fades out volume.
    /// </summary>
    /// <returns>Wait for fixed update.</returns>
    public IEnumerator FadeOutCoroutine()
    {
        while (AudioS.volume > 0)
        {
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
    /// Fades out volume.
    /// </summary>
    /// <returns>Wait for fixed update.</returns>
    public IEnumerator FadeInCoroutine()
    {
        AudioS.volume = 0;

        while (AudioS.volume < Spell.Sounds.Hit.Volume)
        {
            AudioS.volume += Time.deltaTime;

            if (AudioS.volume > Spell.Sounds.Hit.Volume)
            {
                AudioS.volume = Spell.Sounds.Hit.Volume;
                break;
            }

            yield return wffu;
        }
    }
}
        
