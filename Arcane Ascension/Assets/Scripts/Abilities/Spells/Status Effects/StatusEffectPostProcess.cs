using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class StatusEffectPostProcess : MonoBehaviour, IFindPlayer
{
    [SerializeField] private Volume burn;
    [SerializeField] private Volume slow;
    [SerializeField] private Volume fortify;
    [SerializeField] private Volume corrupt;
    [SerializeField] private Volume wispsHealing;
    [SerializeField] private Volume haste;
    [SerializeField] private Volume sacrifice;
    [SerializeField] private Volume vulnerable;

    private Dictionary<StatusEffectType, Volume> volumes;
    private PlayerStats playerStats;

    private void Awake()
    {
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
        foreach(KeyValuePair<StatusEffectType, Volume> keyVal in 
                volumes)
        {
            keyVal.Value.weight = 0;
            keyVal.Value.gameObject.SetActive(false);
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
        IStatusEffectInformation information) =>
            StartCoroutine(UpdatePostProcessCoroutine(type));

    /// <summary>
    /// Adds post process smoothly.
    /// </summary>
    /// <param name="type">Status effect type.</param>
    /// <returns>Null.</returns>
    private IEnumerator UpdatePostProcessCoroutine(StatusEffectType type)
    {
        if (volumes[type].gameObject.activeSelf == false)
        {
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
        while (volumes[type].weight > 0)
        {
            volumes[type].weight -= Time.deltaTime;
            yield return null;
        }
        volumes[type].gameObject.SetActive(false);
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
}
