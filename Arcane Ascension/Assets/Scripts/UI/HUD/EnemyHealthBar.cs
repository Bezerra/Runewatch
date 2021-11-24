using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using ExtensionMethods;

/// <summary>
/// Class responsible for controling enemy health bar information.
/// </summary>
public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private GameObject healthBarGameObject;
    [SerializeField] private Image health;

    [Header("Health bar")]
    [SerializeField] private Color lowHealth;
    [SerializeField] private Color highHealth;

    [Header("Element")]
    [SerializeField] private Image elementIcon;

    [Header("Status Effects Slots")]
    [SerializeField] private Image[] statusEffectsSlots;
    private Dictionary<StatusEffectType, StatusEffectImage> statusEffectsSlotsInUse;

    // Components
    private Camera cam;
    private EnemyStats enemyStats;

    // Coroutines
    private IEnumerator updateHealthCoroutine;
    private YieldInstruction wffu;

    private void Awake()
    {
        cam = Camera.main;
        enemyStats = GetComponentInParent<EnemyStats>();
        wffu = new WaitForFixedUpdate();
        statusEffectsSlotsInUse = new Dictionary<StatusEffectType, StatusEffectImage>();
    }

    private void Start()
    {
        if (enemyStats?.EnemyAttributes.Icon != null)
            elementIcon.sprite = enemyStats.EnemyAttributes.Icon;

        healthBarGameObject.SetActive(false);
    }

    private void OnEnable()
    {
        StartCoroutine(OnEnableCoroutine());
    }

    private IEnumerator OnEnableCoroutine()
    {
        yield return wffu;
        enemyStats.EventTakeDamage += OnTakeDamage;
        enemyStats.StatusEffectList.ValueChanged += UpdateStatusEffectsEvent;
    }

    private void OnDisable()
    {
        enemyStats.EventTakeDamage -= OnTakeDamage;
        enemyStats.StatusEffectList.ValueChanged -= UpdateStatusEffectsEvent;
    }

    /// <summary>
    /// Starts a coroutine to update health.
    /// </summary>
    private void OnTakeDamage() =>
         this.StartCoroutineWithReset(ref updateHealthCoroutine, UpdateHealthCoroutine());

    /// <summary>
    /// Coroutine that updates health bar UI fill amount and color smoothly.
    /// </summary>
    /// <returns>WFFU.</returns>
    private IEnumerator UpdateHealthCoroutine()
    {
        do
        {
            if (enemyStats == null)
                break;

            health.fillAmount =
                Mathf.Lerp(
                    health.fillAmount,
                    enemyStats.Health / enemyStats.CommonAttributes.MaxHealth,
                    Time.fixedDeltaTime * 5f);

            health.color =
                health.color.Remap(
                    0.3f, 0.5f, lowHealth,
                    highHealth, health.fillAmount);

            yield return wffu;
        }
        while (enemyStats.Health.Similiar(health.fillAmount) == false);
    }

    /// <summary>
    /// Enables or disables health bar.
    /// </summary>
    /// <param name="condition">True to enable, false to disable.</param>
    public void EnableEnemyHealthBar(bool condition)
    {
        if (condition)
        {
            healthBarGameObject.SetActive(true);
            OnTakeDamage();
        }
        else
        {
            healthBarGameObject.SetActive(false);
        }
    }

    private void Update()
    {
        UpdateStatusEffects();
    }

    private void FixedUpdate() =>
        UpdateRotation();

    /// <summary>
    /// Updates rotation of the bar.
    /// </summary>
    private void UpdateRotation()
    {
        if (healthBarGameObject.activeSelf)
        {
            healthBarGameObject.transform.LookAt(cam.transform);
        }
    }

    private void UpdateStatusEffects()
    {
        if (statusEffectsSlotsInUse.Count > 0)
        {
            for (int i = 0; i < statusEffectsSlotsInUse.Count; i++)
            {
                statusEffectsSlotsInUse[statusEffectsSlotsInUse.ElementAt(i).Value.Type].Image.fillAmount = 1 -
                    (Time.time -
                    enemyStats.StatusEffectList.Items[statusEffectsSlotsInUse.ElementAt(i).Value.Type].TimeApplied) /
                    statusEffectsSlotsInUse[statusEffectsSlotsInUse.ElementAt(i).Key].Duration;

                if (statusEffectsSlotsInUse[statusEffectsSlotsInUse.ElementAt(i).Value.Type].Image.fillAmount <= 0)
                {
                    statusEffectsSlotsInUse[statusEffectsSlotsInUse.ElementAt(i).Value.Type].Image.gameObject.SetActive(false);
                    statusEffectsSlotsInUse.Remove(statusEffectsSlotsInUse.ElementAt(i).Value.Type);
                }
            }
        }
    }

    /// <summary>
    /// Updates status effects bar.
    /// </summary>
    /// <param name="type"></param>
    /// <param name=""></param>
    private void UpdateStatusEffectsEvent(StatusEffectType type, IStatusEffectInformation information)
    {
        for (int i = 0; i < statusEffectsSlots.Length; i++)
        {
            if (statusEffectsSlots[i].gameObject.activeSelf == false)
            {
                statusEffectsSlots[i].gameObject.SetActive(true);

                statusEffectsSlots[i].sprite = enemyStats.
                    StatusEffectList.Items[type].Icon;

                statusEffectsSlotsInUse.Add(type,
                    new StatusEffectImage(statusEffectsSlots[i], type, information.Duration));

                break;
            }
        }
    }
}
