using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    [SerializeField] private Color noManaSpellColor;
    [SerializeField] private Color spellColor;
    [SerializeField] private Color noSpellColor;

    // Fields to update
    [SerializeField] private Image crosshair;
    [SerializeField] private Image crosshairHit;
    [SerializeField] private List<Image> spellsUI;
    [SerializeField] private List<Image> spellsBackgroundUI;
    [SerializeField] private List<Image> spellsBorderUI;
    [SerializeField] private Image dash;
    [SerializeField] private TextMeshProUGUI dashCharge;
    [SerializeField] private Image health;
    [SerializeField] private Image armor;
    [SerializeField] private Image mana;
    [SerializeField] private TextMeshProUGUI gold;
    [SerializeField] private TextMeshProUGUI arcanePower;
    [SerializeField] private TextMeshProUGUI fpsCounterTMP;

    // WILL BE CONTROLED VIA OPTIONAS WHEN OPTIONS EXIST
    [SerializeField] private bool showFPS;

    private IList<EnemyStats> enemyStats;

    // Coroutines
    private IEnumerator hitCrosshairCoroutine;
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
    }

    private void OnEnable()
    {
        input.CastSpell += CastSpell;
        input.StopCastSpell += StopCastSpell;

        foreach(EnemyStats enemy in enemyStats)
        {
            enemy.EventTakeDamage += TriggerCrosshairHit;
            enemy.EventDeath += UnsubscribeEnemy;
        }
    }

    private void OnDisable()
    {
        input.CastSpell -= CastSpell;
        input.StopCastSpell -= StopCastSpell;

        foreach (EnemyStats enemy in enemyStats)
        {
            if (enemy != null)
            {
                enemy.EventTakeDamage -= TriggerCrosshairHit;
                enemy.EventDeath -= UnsubscribeEnemy;
            }
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

    private void TriggerCrosshairHit(float emptyVar)
    {
        if (hitCrosshairCoroutine != null) StopCoroutine(hitCrosshairCoroutine);
        hitCrosshairCoroutine = TriggerCrosshairHitCoroutine();
        StartCoroutine(hitCrosshairCoroutine);
    }

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
        health.fillAmount =
            playerStats.Health / playerStats.CommonAttributes.MaxHealth;
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
    }
}
