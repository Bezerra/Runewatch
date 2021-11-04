using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Class responsible for selecting a spell slot when all slots are full.
/// </summary>
public class AbilityFullSpellChoice : MonoBehaviour
{
    // Components
    private PlayerSpells playerSpells;

    /// <summary>
    /// Property set on AbilitySpellChoice, to know which spell was selected.
    /// </summary>
    public ISpell NewObtainedSpell { get; set; }

    [SerializeField] private AbilitySpellCard[] allCards;

    private void Awake()
    {
        playerSpells = FindObjectOfType<PlayerSpells>();
    }

    private void OnEnable()
    {
        for (int i = 0; i < allCards.Length -1; i++)
        {
            allCards[i].SpellOnCard = playerSpells.CurrentSpells[i];
        }

        // Sets obtained spell to last card
        allCards[4].SpellOnCard = NewObtainedSpell;

        // Updates info and sets obtained spell variable of all cards.
        foreach (AbilitySpellCard card in allCards)
        {
            card.NewObtainedSpell = NewObtainedSpell;
            card.UpdateInformation();
        }
    }
}
