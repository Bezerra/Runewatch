using UnityEngine;
using TMPro;

/// <summary>
/// Class responsible for handling information of an ability spell card.
/// </summary>
public class AbilitySpellCard : MonoBehaviour
{
    // Spell of this card
    public ISpell SpellOnCard { get; set; }
    public ISpell NewObtainedSpell { get; set; }

    // Components
    private TextMeshProUGUI textInCard;
    private PlayerSpells playerSpells;
    private AbilitiesCanvas abilitiesCanvas;

    // Full spells variables
    [SerializeField] private GameObject fullSpellsCanvas;

    private void Awake()
    {
        abilitiesCanvas = GetComponentInParent<AbilitiesCanvas>();
        playerSpells = FindObjectOfType<PlayerSpells>();
        textInCard = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        UpdateInformation();
    }

    public void UpdateInformation()
    {
        if (SpellOnCard != null)
        {
            textInCard.text = SpellOnCard.Name;
        }
    }

    /// <summary>
    /// This method is called with button press if the player is selection spells and
    /// his current spell slots are not full yet.
    /// </summary>
    public void TryAddSpell()
    {
        // If there are slots, it adds the spell.
        if (CheckAddSpellValidation())
        {
            playerSpells.AddSpell(SpellOnCard as SpellSO);
            abilitiesCanvas.DisableAll();
        }
        // ELse if there are not slots, it will open a new canvas with 4 slots to select.
        else
        {
            // Activates new canvas and sets the currently selected spell on that canvas
            gameObject.SetActive(false);
            fullSpellsCanvas.GetComponent<AbilityFullSpellChoice>().SelectedSpell = SpellOnCard; // THIS BEFORE SETTING ACTIVE
            fullSpellsCanvas.SetActive(true);
        }
    }

    /// <summary>
    /// This method is called with button press if it's the full spell menu canvas.
    /// </summary>
    /// <param name="slot"></param>
    public void AddSpellToSlot(int slot)
    {
        playerSpells.AddSpell(NewObtainedSpell as SpellSO, slot);
        abilitiesCanvas.DisableAll();
    }

    /// <summary>
    /// Checks if it's possible to add a spell.
    /// </summary>
    /// <returns>Returns true if player spell slots are not full.</returns>
    private bool CheckAddSpellValidation()
    {
        foreach (ISpell spell in playerSpells.CurrentSpells)
        {
            if (spell == null)
            {
                return false;
            }
        }
        return true;
    }
}
