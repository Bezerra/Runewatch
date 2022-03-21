using System;
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
    private IInput input;
    private PlayerMovement playerMovement;
    private FPSCounter fpsCounter;

    [Header("Spell slots")]
    [SerializeField] private Color noManaSpellColor;
    [SerializeField] private Color spellColor;
    [SerializeField] private Color noSpellColor;

    [Header("Status Effects Slots")]
    [SerializeField] private Image[] statusEffectsSlots;

    [Header("Health bar")]
    [SerializeField] private Color lowHealth;
    [SerializeField] private Color mediumHealth;
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
    [SerializeField] private GameObject minimap;
    [SerializeField] private RawImage timerBackground;
    [SerializeField] private TextMeshProUGUI timerTMP;
    [SerializeField] private GameObject fpsCounterBackground;
    [SerializeField] private TextMeshProUGUI fpsCounterTMP;
    [Header("Gem Colors")]
    [SerializeField] private ElementalGem[] gems;
    private GameObject activeGem;

    // Collections
    private IList<EnemyStats> enemyStats;
    private Dictionary<StatusEffectType, StatusEffectImage> statusEffectsSlotsInUse;
    private Dictionary<ElementType, GameObject> gemsImages;

    // Coroutines
    private IEnumerator hitCrosshairCoroutine;
    private IEnumerator updateHealthCoroutine;
    private IEnumerator updateManaCoroutine;
    private IEnumerator updateLeafShieldCoroutine;
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
        crosshairWaitForSeconds = new WaitForSeconds(0.3f);
        wffu = new WaitForFixedUpdate();
        statusEffectsSlotsInUse = new Dictionary<StatusEffectType, StatusEffectImage>();
        gemsImages = new Dictionary<ElementType, GameObject>();

        for (int i = 0; i < gems.Length; i++)
        {
            gemsImages.Add(gems[i].Element, gems[i].GO);
        }
    }

    private void OnEnable()
    {
        // Needs to be a coroutine because onEnable is running before player awake < wtf
        StartCoroutine(OnEnableCoroutine());

        ControlFPSCounter((int)PlayerPrefs.GetFloat(PPrefsOptions.FPSCounter.ToString(), 0)); //off
        ControlMinimap((int)PlayerPrefs.GetFloat(PPrefsOptions.Minimap.ToString(), 1)); //on
        ControlTimer((int)PlayerPrefs.GetFloat(PPrefsOptions.Timer.ToString(), 1)); //on
    }

    private IEnumerator OnEnableCoroutine()
    {
        yield return wffu;

        input.CastSpell += CastSpell;
        input.StopCastSpell += StopCastSpell;
        playerStats.EventHealthUpdate += OnTakeDamage;
        playerStats.EventManaUpdate += OnManaUpdate;
        playerStats.EventArmorUpdate += OnArmorUpdate;
        playerCurrency.EventCurrencyUpdate += OnCurrencyUpdate;
        playerStats.StatusEffectList.ValueChanged += UpdateStatusEffectsEvent;
        playerSpells.SelectedNewSpell += SelectNewSpell;

        foreach (EnemyStats enemy in enemyStats)
        {
            enemy.EventTakeDamage += TriggerCrosshairHit;
            enemy.EventDeath += UnsubscribeEnemy;
        }

        // Updates UI for empty spell slots
        for (int i = 0; i < playerSpells.CurrentSpells.Length; i++)
        {
            if (playerSpells.CurrentSpells[i] == null)
            {
                spellsBorderUI[i].enabled = false;
                spellsUI[i].enabled = false;
            }
        }
    }

    private void OnDisable()
    {
        input.CastSpell -= CastSpell;
        input.StopCastSpell -= StopCastSpell;
        playerStats.EventHealthUpdate -= OnTakeDamage;
        playerStats.EventManaUpdate -= OnManaUpdate;
        playerStats.EventArmorUpdate -= OnArmorUpdate;
        playerCurrency.EventCurrencyUpdate -= OnCurrencyUpdate;
        playerStats.StatusEffectList.ValueChanged -= UpdateStatusEffectsEvent;
        playerSpells.SelectedNewSpell -= SelectNewSpell;

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
        armor.fillAmount = 0;
        spellsUI[4].sprite = playerSpells.SecondarySpell.Icon;

        // Timer will be saved everytime ending run scene is loaded

        GameplayTime.LoadTimer();
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

            if (health.fillAmount <= 0.35f)
            {
                health.color =
                    health.color.Remap(
                        0.25f, 0.35f, lowHealth,
                        mediumHealth, health.fillAmount);
            }
            else
            {
                health.color =
                    health.color.Remap(
                        0.35f, 0.45f, mediumHealth,
                        highHealth, health.fillAmount);
            }

            yield return wffu;
        }
        while (playerStats.Health.Similiar(health.fillAmount) == false);
    }

    /// <summary>
    /// Starts a coroutine to update health.
    /// </summary>
    private void OnArmorUpdate() =>
         this.StartCoroutineWithReset(ref updateLeafShieldCoroutine, UpdateLeafShieldCoroutine());

    /// <summary>
    /// Coroutine that updates leaf shield bar UI fill amount.
    /// </summary>
    /// <returns>WFFU.</returns>
    private IEnumerator UpdateLeafShieldCoroutine()
    {
        do
        {
            if (playerStats == null)
                break;

            armor.fillAmount =
                Mathf.Lerp(
                    armor.fillAmount,
                    playerStats.Armor / playerStats.PlayerAttributes.MaxArmor,
                    Time.fixedDeltaTime * 2f);

            yield return wffu;
        }
        while (playerStats.Armor.Similiar(armor.fillAmount) == false);
        if (armor.fillAmount < 0.01) armor.fillAmount = 0;
    }

    /// <summary>
    /// Starts a coroutine to update health.
    /// </summary>
    private void OnManaUpdate() =>
         this.StartCoroutineWithReset(ref updateManaCoroutine, UpdateManaCoroutine());

    /// <summary>
    /// Coroutine that updates mana bar UI fill amount.
    /// </summary>
    /// <returns>WFFU.</returns>
    private IEnumerator UpdateManaCoroutine()
    {
        do
        {
            if (playerStats == null)
                break;

            mana.fillAmount =
                Mathf.Lerp(
                    mana.fillAmount,
                    playerStats.Mana / playerStats.PlayerAttributes.MaxMana,
                    Time.fixedDeltaTime * 10f);

            yield return wffu;
        }
        while (playerStats.Mana.Similiar(mana.fillAmount) == false);
    }

    private void OnCurrencyUpdate(float currencyGold, float currencyAP)
    {
        gold.text = "Gold : " + currencyGold;
        arcanePower.text = "Arcane P : " + currencyAP;
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
        if (playerSpells.ActiveSpell == null)
            return;

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
        UpdateSpells();
        UpdateDashCharges();
        UpdateFPS();
        UpdateStatusEffects();
        UpdateTimer();
    }

    /// <summary>
    /// Updates spells UI.
    /// </summary>
    private void UpdateSpells()
    {
        for (int i = 0; i < playerSpells.CurrentSpells.Length; i++)
        {
            if (playerSpells.CurrentSpells[i] != null)
            {
                spellsUI[i].fillAmount =
                    playerSpells.CurrentSpells[i].CooldownCounter / playerSpells.CurrentSpells[i].Cooldown;
                spellsBackgroundUI[i].fillAmount =
                    playerSpells.CurrentSpells[i].CooldownCounter / playerSpells.CurrentSpells[i].Cooldown;

                // No mana
                if (playerStats.Mana - playerSpells.CurrentSpells[i].ManaCost < 0)
                {
                    spellsUI[i].color = noManaSpellColor;
                }
                else
                {
                    spellsUI[i].color = spellColor;
                }
            }
        }
        
        spellsUI[4].fillAmount =
                    playerSpells.SecondarySpell.CooldownCounter / playerSpells.SecondarySpell.Cooldown;
    }

    /// <summary>
    /// Executed when the player selects a new spell.
    /// </summary>
    /// <param name="index"></param>
    private void SelectNewSpell(byte index)
    {
        for (int i = 0; i < playerSpells.CurrentSpells.Length; i++)
        {
            if (playerSpells.CurrentSpells[i] != null)
            {
                // Selected spell
                if (i == playerSpells.CurrentSpellIndex)
                {
                    spellsUI[i].enabled = true;
                    spellsUI[i].sprite = playerSpells.CurrentSpells[i].Icon;
                    spellsBorderUI[i].enabled = true;

                    // If there's an active gem
                    if (activeGem != null)
                    {
                        activeGem.SetActive(false);
                    }

                    activeGem = gemsImages[playerSpells.CurrentSpells[i].Element];
                    activeGem.SetActive(true);
                }
                else
                {
                    spellsUI[i].enabled = true;
                    spellsUI[i].sprite = playerSpells.CurrentSpells[i].Icon;
                    spellsBorderUI[i].enabled = false;
                }
            }
            else
            {
                spellsUI[i].fillAmount = 0;
                spellsUI[i].color = noSpellColor;
                spellsBorderUI[i].enabled = false;
                spellsUI[i].enabled = false;
            }
        }
    }

    /// <summary>
    /// Updates dash cahrges.
    /// </summary>
    private void UpdateDashCharges()
    {
        if (playerStats.DashCharge != playerStats.PlayerAttributes.MaxDashCharge)
        {
            dash.fillAmount = 1 - playerMovement.CurrentTimeToGetCharge / player.Values.TimeToGetDashCharge;
        }
        else
        {
            dash.fillAmount = 1;
        }
        dashCharge.text = playerStats.DashCharge.ToString();
    }

    /// <summary>
    /// Updates fps counter.
    /// </summary>
    private void UpdateFPS()
    {
        if (PlayerPrefs.GetFloat("FPSCounter", 0) == 1)
        {
            if (fpsCounter.FrameRate >= 59) fpsCounterTMP.color = Color.green;
            else if (fpsCounter.FrameRate >= 29) fpsCounterTMP.color = new Color(0.75f, 1, 0.75f, 1);
            else if (fpsCounter.FrameRate > 20) fpsCounterTMP.color = Color.yellow;
            else fpsCounterTMP.color = Color.red;
            fpsCounterTMP.text = fpsCounter.FrameRate.ToString() + " fps";
        }
    }

    /// <summary>
    /// Sets fps counter to on or off.
    /// </summary>
    /// <param name="value"></param>
    public void ControlFPSCounter(int value)
    {
        if (value == 1)
        {
            fpsCounterBackground.SetActive(true);
            fpsCounterTMP.enabled = true;
        }
        else
        {
            fpsCounterBackground.SetActive(false);
            fpsCounterTMP.enabled = false;
        }
    }

    /// <summary>
    /// Sets minimap to on or off.
    /// </summary>
    /// <param name="value"></param>
    public void ControlMinimap(int value)
    {
        if (value == 1)
        {
            minimap.SetActive(true);
        }
        else
        {
            minimap.SetActive(false);
        }
    }

    /// <summary>
    /// Sets timer to on or off.
    /// </summary>
    /// <param name="value"></param>
    public void ControlTimer(int value)
    {
        if (value == 1)
        {
            timerBackground.enabled = true;
            timerTMP.color = new Color(1, 1, 1, 1);
        }
        else
        {
            timerBackground.enabled = false;
            timerTMP.color = new Color(1, 1, 1, 0);
        }
    }

    /// <summary>
    /// Update status effects timers.
    /// </summary>
    private void UpdateStatusEffects()
    {
    if (statusEffectsSlotsInUse.Count > 0)
    {
        for (int i = 0; i < statusEffectsSlotsInUse.Count; i++)
        {
            statusEffectsSlotsInUse[statusEffectsSlotsInUse.ElementAt(i).Value.Type].Image.fillAmount = 1 -
                (Time.time -
                playerStats.StatusEffectList.Items[statusEffectsSlotsInUse.
                ElementAt(i).Value.Type].TimeApplied) /
                statusEffectsSlotsInUse[statusEffectsSlotsInUse.
                ElementAt(i).Key].Duration;

            if (statusEffectsSlotsInUse[statusEffectsSlotsInUse.
                ElementAt(i).Value.Type].Image.fillAmount <= 0)
            {
                statusEffectsSlotsInUse[statusEffectsSlotsInUse.
                    ElementAt(i).Value.Type].Image.gameObject.SetActive(false);
                statusEffectsSlotsInUse.Remove(statusEffectsSlotsInUse.ElementAt(i).Value.Type);
            }
        }
    }
    }

    /// <summary>
    /// Updates status effects bar with current status effects.
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
    /// Updates timer every update.
    /// </summary>
    private void UpdateTimer()
    {
        GameplayTime.UpdateTimer();
        timerTMP.text = GameplayTime.GameTimer.ToString(@"hh\:mm\:ss");
    }

    [Serializable]
    private class ElementalGem
    {
        [SerializeField] private ElementType element;
        [SerializeField] private GameObject gO;
        public ElementType Element => element;
        public GameObject GO => gO;
    }
}
