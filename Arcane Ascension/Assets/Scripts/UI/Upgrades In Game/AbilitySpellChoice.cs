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
    private AbilitiesCanvas abilitiesCanvas;
    private PlayerInteraction playerInteraction;

    private void Awake()
    {
        abilitiesCanvas = GetComponentInParent<AbilitiesCanvas>();
        spellCards = GetComponentsInChildren<AbilitySpellCard>();
        playerInteraction = FindObjectOfType<PlayerInteraction>();
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

        bool deactivateCanvas = true;
        for (int i = 0; i < randomAbilities.SpellResult.Length; i++)
        {
            if (randomAbilities.SpellResult[i] != null)
                deactivateCanvas = false;
        }
        if (deactivateCanvas)
        {
            // Destroys the spell scroll
            Destroy(playerInteraction.LastObjectInteracted.gameObject);
            abilitiesCanvas.DisableAll();
        }
    }
}
