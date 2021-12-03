using UnityEngine;

/// <summary>
/// Class responsible for selecting a spell slot when all slots are full and the picked spell was a known spell.
/// </summary>
public class AbilityFullSpellChoiceKnownSpell : MonoBehaviour
{
    // Components
    private PlayerInputCustom input;
    private PlayerSpells playerSpells;

    // Scriptable object with dropped spells values
    [SerializeField] private RandomAbilitiesToChooseSO droppedSpellResult;

    // Cards on the UI
    [SerializeField] private AbilitySpellCard[] allCards;
    [SerializeField] private AbilitySpellCard obtainedSpellCard;

    private void Awake()
    {
        input = FindObjectOfType<PlayerInputCustom>();
        playerSpells = FindObjectOfType<PlayerSpells>();
    }

    private void OnEnable()
    {
        // Prevents bugs, don't add if null (TRUST)
        playerSpells = FindObjectOfType<PlayerSpells>();

        if (droppedSpellResult.DroppedSpell != null &&
            playerSpells.CurrentSpells != null)
        {
            // Updates current cards with player's spells
            for (int i = 0; i < allCards.Length; i++)
            {
                allCards[i].SpellOnCard = playerSpells.CurrentSpells[i];
                allCards[i].UpdateInformation();
            }

            // Sets obtained spell to last card
            obtainedSpellCard.SpellOnCard = droppedSpellResult.DroppedSpell;

            // Updates info and sets obtained spell variable of all cards.
            foreach (AbilitySpellCard card in allCards)
            {
                card.NewObtainedSpell = droppedSpellResult.DroppedSpell;
                card.UpdateInformation();
            }

            obtainedSpellCard.UpdateInformation();
        }
    }

    public void BackToGame()
    {
        input.SwitchActionMapToGameplay();
        Time.timeScale = 1;
    }
}
