using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ExtensionMethods;

/// <summary>
/// Class responsible for handling player spells UI.
/// </summary>
public class PlayerUI : MonoBehaviour
{
    // Components
    private Player player;
    private PlayerSpells playerSpells;
    private PlayerStats playerStats;
    private IUseCurrency playerCurrency;
    private PlayerInputCustom input;
    private PlayerMovement playerMovement;
    private FPSCounter fpsCounter;
    private IList<EnemyStats> enemyStats;

    [Header("Spell slots")]
    [SerializeField] private Color noManaSpellColor;
    [SerializeField] private Color spellColor;
    [SerializeField] private Color noSpellColor;

    [Header("Status Effects Slots")]
    [SerializeField] private Image[] statusEffectsSlots;
    private Dictionary<StatusEffectType, StatusEffectImage> statusEffectsSlotsInUse;

    [Header("Health bar")]
    [SerializeField] private Color lowHealth;
    [SerializeField] private Color highHealth;

    // Fields to update
    [Header("Crosshair UI")]
    [SerializeField] private Image crosshair;
    [SerializeField] private Image crosshairHit;
    [Header("Spells UI")]
    [SerializeField] private List<Image> spellsUI;
    [SerializeField] private List<Image> spellsBackgroundUI;
    [SerializeField] private List<Image> spellsBorderUI;
    [Header("Dash UI")]
    [SerializeField] private Image dash;
    [SerializeField] private TextMeshProUGUI dashCharge;
    [Header("Stats UI")]
    [SerializeField] private Image health;
    [SerializeField] private Image armor;
    [SerializeField] private Image mana;
    [Header("Currency UI")]
    [SerializeField] private TextMeshProUGUI gold;
    [SerializeField] private TextMeshProUGUI arcanePower;
    [Header("Misc")]
    [SerializeField] private TextMeshProUGUI fpsCounterTMP;
    [SerializeField] private bool showFPS;
    // WILL BE CONTROLED VIA OPTIONS WHEN OPTIONS EXIST

    

    // Coroutines
    private IEnumerator hitCrosshairCoroutine;
    private IEnumerator updateHealthCoroutine;
    private float crosshairHitAlpha;
    private YieldInstruction crosshairWaitForSeconds;
    private YieldInstruction wffu;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        input = FindObjectOfType<PlayerInputCustom>();
        playerMovement = GetComponentInParent<PlayerMovement>();
        playerSpells = GetComponentInParent<PlayerSpells>();
        playerStats = GetComponentInParent<PlayerStats>();
        playerCurrency = GetComponentInParent<IUseCurrency>();
        fpsCounter = GetComponent<FPSCounter>();
        enemyStats = FindObjectsOfType<EnemyStats>();
        crosshairWaitForSeconds = new WaitForSeconds(1);
        wffu = new WaitForFixedUpdate();
        statusEffectsSlotsInUse = new Dictionary<StatusEffectType, StatusEffectImage>();
    }

    private void OnEnable()
    {
        // Needs to be a coroutine because onEnable is running before player awake
        StartCoroutine(OnEnableCoroutine());
    }

    private IEnumerator OnEnableCoroutine()
    {
        yield return new WaitForFixedUpdate();

        input.CastSpell += CastSpell;
        input.StopCastSpell += StopCastSpell;
        playerStats.EventTakeDamage += OnTakeDamage;

        playerStats.StatusEffectList.ValueChanged += UpdateStatusEffectsEvent;

        foreach (EnemyStats enemy in enemyStats)
        {
            enemy.EventTakeDamage += TriggerCrosshairHit;
            enemy.EventDeath += UnsubscribeEnemy;
        }
    }

    private void OnDisable()
    {
        input.CastSpell -= CastSpell;
        input.StopCastSpell -= StopCastSpell;
        playerStats.EventTakeDamage -= OnTakeDamage;
        playerStats.StatusEffectList.ValueChanged -= UpdateStatusEffectsEvent;

        foreach (EnemyStats enemy in enemyStats)
        {
            if (enemy != null)
            {
                enemy.EventTakeDamage -= TriggerCrosshairHit;
                enemy.EventDeath -= UnsubscribeEnemy;
            }
        }
    }

    private void Start()
    {
        OnTakeDamage();
    }

    public void SubscribeToEnemiesDamage()
    {
        if (enemyStats.Count > 0)
        {
            foreach (EnemyStats enemy in enemyStats)
            {
                if (enemy != null)
                {
                    enemy.EventTakeDamage -= TriggerCrosshairHit;
                    enemy.EventDeath -= UnsubscribeEnemy;
                }
            }
        }
        enemyStats = FindObjectsOfType<EnemyStats>();
        foreach (EnemyStats enemy in enemyStats)
        {
            enemy.EventTakeDamage += TriggerCrosshairHit;
            enemy.EventDeath += UnsubscribeEnemy;
        }
    }

    private void UnsubscribeEnemy(Stats deadEnemy)
    {
        for (int i = 0; i < enemyStats.Count; i++)
        {
            if (enemyStats[i] != null)
            {
                if (enemyStats[i] == deadEnemy)
                {
                    enemyStats[i].EventTakeDamage -= TriggerCrosshairHit;
                    enemyStats[i].EventDeath -= UnsubscribeEnemy;
                    break;
                }
            }
        }
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
            if (playerStats == null)
                break;

            health.fillAmount =
                Mathf.Lerp(
                    health.fillAmount,
                    playerStats.Health / playerStats.CommonAttributes.MaxHealth, 
                    Time.fixedDeltaTime * 5f);

            health.color =
                health.color.Remap(
                    0.3f, 0.5f, lowHealth,
                    highHealth, health.fillAmount);

            yield return wffu;
        }
        while (playerStats.Health.Similiar(health.fillAmount) == false);
    }

    /// <summary>
    /// Starts crosshair hit coroutine.
    /// </summary>
    private void TriggerCrosshairHit() =>
        this.StartCoroutineWithReset(ref hitCrosshairCoroutine, TriggerCrosshairHitCoroutine());

    /// <summary>
    /// Shows crosshair hit UI and smoothly fades it away.
    /// </summary>
    /// <returns>WFFU.</returns>
    private IEnumerator TriggerCrosshairHitCoroutine()
    {
        crosshairHitAlpha = 1;
        crosshairHit.color = new Color(1, 1, 1, crosshairHitAlpha);
        yield return crosshairWaitForSeconds;
        while (crosshairHitAlpha > 0)
        {
            crosshairHit.color = new Color(1, 1, 1, crosshairHitAlpha);
            crosshairHitAlpha -= Time.fixedDeltaTime;
            yield return wffu;
        }
    }

    /// <summary>
    /// Disables crosshair.
    /// </summary>
    private void CastSpell()
    {
        if (playerSpells.ActiveSpell.CastType == SpellCastType.OneShotCastWithRelease &&
            playerSpells.CooldownOver(playerSpells.ActiveSpell) &&
            playerSpells.CooldownOver(playerSpells.SecondarySpell))
            crosshair.enabled = false;
    }

    /// <summary>
    /// Enables crosshair.
    /// </summary>
    private void StopCastSpell()
    {
        if (crosshair.enabled == false)
            crosshair.enabled = true;
    }


    /// <summary>
    /// Updates the cooldown UI for all current spells and mana.
    /// </summary>
    private void Update()
    {
        // Updates spells
        for (int i = 0; i < playerSpells.CurrentSpells.Length; i++)
        {
            if (playerSpells.CurrentSpells[i] != null)
            {
                spellsUI[i].sprite = playerSpells.CurrentSpells[i].Icon;
                spellsUI[i].fillAmount =
                    playerSpells.CurrentSpells[i].CooldownCounter / playerSpells.CurrentSpells[i].Cooldown;
                spellsBackgroundUI[i].fillAmount =
                    playerSpells.CurrentSpells[i].CooldownCounter / playerSpells.CurrentSpells[i].Cooldown;

                if (playerStats.Mana - playerSpells.CurrentSpells[i].ManaCost < 0)
                {
                    spellsUI[i].color = noManaSpellColor;
                }
                else
                {
                    spellsUI[i].color = spellColor;
                }

                if (i == playerSpells.CurrentSpellIndex)
                {
                    spellsBorderUI[i].enabled = true;
                }
                else
                {
                    spellsBorderUI[i].enabled = false;
                }
            }
            else
            {
                spellsUI[i].fillAmount = 0;
                spellsUI[i].color = noSpellColor;
            }
        }
        spellsUI[4].sprite = playerSpells.SecondarySpell.Icon;
        spellsUI[4].fillAmount =
                    playerSpells.SecondarySpell.CooldownCounter / playerSpells.SecondarySpell.Cooldown;

        // Updates dash
        if (playerStats.DashCharge != playerStats.PlayerAttributes.MaxDashCharge)
        {
            dash.fillAmount = 1 - playerMovement.CurrentTimeToGetCharge / player.Values.TimeToGetDashCharge;
        } 
        else
        {
            dash.fillAmount = 1;
        }
        dashCharge.text = "x" + playerStats.DashCharge.ToString();

        // Updates HP/shield/mana bars
        
        armor.fillAmount =
            playerStats.Armor / playerStats.PlayerAttributes.MaxArmor;
        mana.fillAmount =
            playerStats.Mana / playerStats.PlayerAttributes.MaxMana;

        // Updates loot
        gold.text = "Gold : " + playerCurrency.Quantity.Item1;
        arcanePower.text = "Arcane P : " + playerCurrency.Quantity.Item2;

        if (showFPS)
        {
            if (fpsCounter.FrameRate >= 59) fpsCounterTMP.color = Color.green;
            else if (fpsCounter.FrameRate >= 29) fpsCounterTMP.color = new Color(0.75f, 1, 0.75f, 1);
            else if (fpsCounter.FrameRate > 20) fpsCounterTMP.color = Color.yellow;
            else fpsCounterTMP.color = Color.red;
            fpsCounterTMP.text = fpsCounter.FrameRate.ToString() + " fps";
        }

        UpdateStatusEffects();
    }


    private void UpdateStatusEffects()
    {
        if (statusEffectsSlotsInUse.Count > 0)
        {
            for (int i = 0; i < statusEffectsSlotsInUse.Count; i++)
            {
                statusEffectsSlotsInUse[statusEffectsSlotsInUse.ElementAt(i).Value.Type].Image.fillAmount = 1 -
                    (Time.time -
                    playerStats.StatusEffectList.Items[statusEffectsSlotsInUse.ElementAt(i).Value.Type].TimeApplied) /
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

                statusEffectsSlots[i].sprite = playerStats.
                    StatusEffectList.Items[type].Icon;

                statusEffectsSlotsInUse.Add(type, 
                    new StatusEffectImage(statusEffectsSlots[i], type, information.Duration));

                break;
            }
        }
    }

    /// <summary>
    /// Struct for an image of a status effect.
    /// </summary>
    private struct StatusEffectImage
    {
        public Image Image { get; }
        public StatusEffectType Type { get; }
        public float Duration { get; }
        public StatusEffectImage(Image image, StatusEffectType type, float duration)
        {
            Image = image;
            Type = type;
            Duration = duration;
        }
    }
}
