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
    [SerializeField] private AbilitySpellCard obtainedSpellCard;

    private void Awake()
    {
        playerSpells = FindObjectOfType<PlayerSpells>();
    }

    private void OnEnable()
    {
        if (NewObtainedSpell != null)
        {
            for (int i = 0; i < allCards.Length; i++)
            {
                allCards[i].SpellOnCard = playerSpells.CurrentSpells[i];
                allCards[i].UpdateInformation();
            }

            // Sets obtained spell to last card
            obtainedSpellCard.SpellOnCard = NewObtainedSpell;

            // Updates info and sets obtained spell variable of all cards.
            foreach (AbilitySpellCard card in allCards)
            {
                card.NewObtainedSpell = NewObtainedSpell;
                card.UpdateInformation();
            }

            // Updates info of obtained spell card
            obtainedSpellCard.UpdateInformation();
        }  
    }
}
