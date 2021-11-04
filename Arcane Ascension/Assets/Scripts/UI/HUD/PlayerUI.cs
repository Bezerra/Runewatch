using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class responsible for handling player spells UI.
/// </summary>
public class PlayerUI : MonoBehaviour
{
    private PlayerSpells playerSpells;
    private Stats playerStats;

    [SerializeField] private List<Image> spellsUI;
    [SerializeField] private Image health;
    [SerializeField] private Image armor;
    [SerializeField] private Image mana;

    private void Awake()
    {
        playerSpells = GetComponentInParent<PlayerSpells>();
        playerStats = GetComponentInParent<Stats>();
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
                spellsUI[i].fillAmount =
                    playerSpells.CurrentSpells[i].CooldownCounter / playerSpells.CurrentSpells[i].Cooldown;
            }
            else
            {
                spellsUI[i].fillAmount = 0;
            }
        }

        spellsUI[4].fillAmount =
                    playerSpells.SecondarySpell.CooldownCounter / playerSpells.SecondarySpell.Cooldown;

        health.fillAmount =
            playerStats.Health / playerStats.Attributes.MaxHealth;

        armor.fillAmount =
            playerStats.Armor / playerStats.Attributes.MaxArmor;

        mana.fillAmount =
            playerStats.Mana / playerStats.Attributes.MaxMana;
        
    }
}
