using UnityEngine;

/// <summary>
/// Class responsible for selecting a spell slot when all slots are full and the picked spell was a known spell.
/// </summary>
public class AbilityFullSpellChoiceKnownSpell : MonoBehaviour
{
    // Components
    private PlayerSpells playerSpells;

    [SerializeField] private RandomAbilitiesToChooseSO droppedSpellResult;

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
        allCards[4].SpellOnCard = droppedSpellResult.SpellResult[0];

        // Updates info and sets obtained spell variable of all cards.
        foreach (AbilitySpellCard card in allCards)
        {
            card.NewObtainedSpell = droppedSpellResult.SpellResult[0];
            card.UpdateInformation();
        }
    }
}
