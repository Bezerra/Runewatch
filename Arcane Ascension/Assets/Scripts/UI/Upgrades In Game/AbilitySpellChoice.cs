using UnityEngine;

/// <summary>
/// Class responsible for three random abilities canvas and logic.
/// </summary>
public class AbilitySpellChoice : MonoBehaviour
{
    // Scriptable object with random abilities
    [SerializeField] private RandomAbilitiesToChooseSO randomAbilities;

    // Panels with 3 spells
    private AbilitySpellCard[] spellCards;

    // Components
    private PlayerInputCustom input;

    private void Awake()
    {
        spellCards = GetComponentsInChildren<AbilitySpellCard>();
        input = FindObjectOfType<PlayerInputCustom>();
    }

    private void OnEnable()
    {
        // Updates spell cards with random spells obtained
        for (int i = 0; i < spellCards.Length; i++)
        {
            if (randomAbilities.SpellResult[i] != null)
            {
                spellCards[i].SpellOnCard = randomAbilities.SpellResult[i];
            }
        }
    }

    public void BackToGame()
    {
        input.SwitchActionMapToGameplay();
        Time.timeScale = 1;
    }
}
