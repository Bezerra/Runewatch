using UnityEngine;
using TMPro;

/// <summary>
/// Class responsible for handling information of an ability spell card.
/// </summary>
public class AbilitySpellCard : MonoBehaviour
{
    // Spell of this card
    public ISpell SpellOnCard { get; set; }

    // Components
    private TextMeshProUGUI textInCard;
    private PlayerSpells playerSpells;
    private AbilitiesCanvas abilitiesCanvas;

    private void Awake()
    {
        abilitiesCanvas = GetComponentInParent<AbilitiesCanvas>();
        playerSpells = FindObjectOfType<PlayerSpells>();
        textInCard = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        if (SpellOnCard != null)
        {
            textInCard.text = SpellOnCard.Name;
        }
    }

    public void AddSpell()
    {
        if (CheckAddSpellValidation())
        {
            playerSpells.AddSpell(SpellOnCard as SpellSO);
            abilitiesCanvas.DisableAll();
            Destroy(gameObject);
        }
    }

    private bool CheckAddSpellValidation()
    {
        return true;
    }
}
