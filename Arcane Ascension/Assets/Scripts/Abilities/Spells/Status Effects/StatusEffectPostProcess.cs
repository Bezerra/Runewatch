using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class StatusEffectPostProcess : MonoBehaviour, IFindPlayer
{
    [SerializeField] private Volume burn;
    [SerializeField] private ParticleSystem[] burnParticles;
    [SerializeField] private Volume slow;
    [SerializeField] private ParticleSystem[] slowParticles;
    [SerializeField] private Volume fortify;
    [SerializeField] private ParticleSystem[] fortifyParticles;
    [SerializeField] private Volume corrupt;
    [SerializeField] private ParticleSystem[] corruptParticles;
    [SerializeField] private Volume wispsHealing;
    [SerializeField] private ParticleSystem[] wispsHealingParticles;
    [SerializeField] private Volume haste;
    [SerializeField] private ParticleSystem[] hasteParticles;
    [SerializeField] private Volume sacrifice;
    [SerializeField] private ParticleSystem[] sacrificeParticles;
    [SerializeField] private Volume vulnerable;
    [SerializeField] private ParticleSystem[] vulnerableParticles;

    private IEnumerator statusCoroutine;

    private Dictionary<StatusEffectType, Volume> volumes;
    private Dictionary<StatusEffectType, ParticleSystem[]> particles;
    private Stats playerStats;

    private void Awake()
    {
        particles = new Dictionary<StatusEffectType, ParticleSystem[]>()
        {
            { StatusEffectType.Burn, burnParticles },
            { StatusEffectType.Slow, slowParticles },
            { StatusEffectType.Fortified, fortifyParticles },
            { StatusEffectType.Corrupt, corruptParticles },
            { StatusEffectType.WispsRegen, wispsHealingParticles },
            { StatusEffectType.Haste, hasteParticles },
            { StatusEffectType.Sacrifice, sacrificeParticles },
            { StatusEffectType.Vulnerable, vulnerableParticles },
        };

        volumes = new Dictionary<StatusEffectType, Volume>()
        {
            {StatusEffectType.Burn, burn },
            {StatusEffectType.Slow, slow },
            {StatusEffectType.Fortified, fortify },
            {StatusEffectType.Corrupt, corrupt },
            {StatusEffectType.WispsRegen, wispsHealing },
            {StatusEffectType.Haste, haste },
            {StatusEffectType.Sacrifice, sacrifice },
            {StatusEffectType.Vulnerable, vulnerable },
        };
    }

    private void Start()
    {
        foreach(KeyValuePair<StatusEffectType, ParticleSystem[]> particleArray in 
                particles)
        {
            foreach(ParticleSystem particle in particleArray.Value)
            {
                particle.Stop();
            }
        }

        foreach(KeyValuePair<StatusEffectType, Volume> volume in 
                volumes)
        {
            volume.Value.weight = 0;
            volume.Value.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Only finds player variables after a fixed update.
    /// </summary>
    private void OnEnable() =>
        StartCoroutine(OnEnableCoroutine());

    private IEnumerator OnEnableCoroutine()
    {
        yield return new WaitForFixedUpdate();
        FindPlayer();
    }

    private void OnDisable() =>
        PlayerLost();

    /// <summary>
    /// Starts update post process coroutine.
    /// </summary>
    /// <param name="type">Status effect type.</param>
    /// <param name="information"></param>
    private void UpdatePostProcess(StatusEffectType type, 
        IStatusEffectInformation information)
    {
        // If theres a coroutine running, cancels all effects
        if (statusCoroutine != null) StopCoroutine(statusCoroutine);
        statusCoroutine = UpdatePostProcessCoroutine(type);
        StartCoroutine(statusCoroutine);
    }

    /// <summary>
    /// Adds post process smoothly.
    /// </summary>
    /// <param name="type">Status effect type.</param>
    /// <returns>Null.</returns>
    private IEnumerator UpdatePostProcessCoroutine(StatusEffectType type)
    {
        if (volumes[type].gameObject.activeSelf == false)
        {
            foreach (KeyValuePair<StatusEffectType, Volume> volume in
                volumes)
            {
                if (volume.Key != type)
                {
                    RemovePostProcess(type);
                }
            }

            foreach (ParticleSystem particle in particles[type])
                particle.Play();

            volumes[type].gameObject.SetActive(true);
            while (volumes[type].weight < 1)
            {
                volumes[type].weight += Time.deltaTime;
                yield return null;
            }
        }
    }

    /// <summary>
    /// Starts remove post process coroutine.
    /// </summary>
    /// <param name="type">Status effect type.</param>
    private void RemovePostProcess(StatusEffectType type) =>
        StartCoroutine(RemovePostProcessCoroutine(type));

    /// <summary>
    /// Removes post process smoothly.
    /// </summary>
    /// <param name="type">Status effect type.</param>
    /// <returns>Null.</returns>
    private IEnumerator RemovePostProcessCoroutine(StatusEffectType type)
    {
        if (volumes[type].gameObject.activeSelf)
        {
            foreach (ParticleSystem particle in particles[type])
                particle.Stop();

            while (volumes[type].weight > 0)
            {
                volumes[type].weight -= Time.deltaTime;
                yield return null;
            }

            while (EffectGetAliveParticles(particles[type]) > 0)
            {
                yield return null;
            }

            volumes[type].gameObject.SetActive(false);
        }
    }

    public void FindPlayer()
    {
        PlayerLost();

        playerStats = FindObjectOfType<PlayerStats>();
        if (playerStats != null)
        {
            if (playerStats.StatusEffectList != null)
            {
                playerStats.StatusEffectList.ValueChanged += UpdatePostProcess;
                playerStats.StatusEffectList.ValueChangedRemove += RemovePostProcess;
            }
        }
    }

    public void PlayerLost()
    {
        if (playerStats != null)
        {
            playerStats.StatusEffectList.ValueChanged -= UpdatePostProcess;
            playerStats.StatusEffectList.ValueChangedRemove -= RemovePostProcess;
        }
    }

    /// <summary>
    /// Gets alive particles of a particle system.
    /// </summary>
    /// <param name="particleSystem"></param>
    /// <returns></returns>
    private int EffectGetAliveParticles(ParticleSystem[] particleSystem)
    {
        int result = 0;
        foreach (ParticleSystem particles in particleSystem)
            result += particles.particleCount;

        return result;
    }
}
