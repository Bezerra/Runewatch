using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Class responsible for handling player spells UI.
/// </summary>
public class PlayerUI : MonoBehaviour
{
    private PlayerSpells playerSpells;
    private PlayerStats playerStats;
    private IUseCurrency playerCurrency;
    private PlayerInputCustom input;

    [SerializeField] private Image crosshair;
    [SerializeField] private List<Image> spellsUI;
    [SerializeField] private Image health;
    [SerializeField] private Image armor;
    [SerializeField] private Image mana;
    [SerializeField] private TextMeshProUGUI gold;
    [SerializeField] private TextMeshProUGUI arcanePower;

    private void Awake()
    {
        input = FindObjectOfType<PlayerInputCustom>();
        playerSpells = GetComponentInParent<PlayerSpells>();
        playerStats = GetComponentInParent<PlayerStats>();
        playerCurrency = GetComponentInParent<IUseCurrency>();
    }

    private void OnEnable()
    {
        input.CastSpell += CastSpell;
        input.StopCastSpell += StopCastSpell;
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
        for (int i = 0; i < playerSpells.CurrentSpells.Length; i++)
        {
            if (playerSpells.CurrentSpells[i] != null)
            {
                spellsUI[i].sprite = playerSpells.CurrentSpells[i].Icon;
                spellsUI[i].fillAmount =
                    playerSpells.CurrentSpells[i].CooldownCounter / playerSpells.CurrentSpells[i].Cooldown;
            }
            else
            {
                spellsUI[i].fillAmount = 0;
            }
        }

        spellsUI[4].sprite = playerSpells.SecondarySpell.Icon;
        spellsUI[4].fillAmount =
                    playerSpells.SecondarySpell.CooldownCounter / playerSpells.SecondarySpell.Cooldown;

        health.fillAmount =
            playerStats.Health / playerStats.CommonAttributes.MaxHealth;

        armor.fillAmount =
            playerStats.Armor / playerStats.PlayerAttributes.MaxArmor;

        mana.fillAmount =
            playerStats.Mana / playerStats.PlayerAttributes.MaxMana;

        gold.text = "Gold : " + playerCurrency.Quantity.Item1;
        arcanePower.text = "Arcane P : " + playerCurrency.Quantity.Item2;
    }
}
