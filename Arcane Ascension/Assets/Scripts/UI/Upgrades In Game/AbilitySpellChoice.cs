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

    private void Awake()
    {
        spellCards = GetComponentsInChildren<AbilitySpellCard>();
    }

    private void OnEnable()
    {
        for (int i = 0; i < spellCards.Length; i++)
        {
            if (randomAbilities.SpellResult[i] != null)
            {
                spellCards[i].SpellOnCard = randomAbilities.SpellResult[i];
            }
        }
    }

    public void TEMPBACKTOGAME()
    {
        FindObjectOfType<PlayerInputCustom>().SwitchActionMapToGameplay();
        Time.timeScale = 1;
    }
}
