using UnityEngine;
using TMPro;

/// <summary>
/// Class responsible for handling information of an ability spell card.
/// </summary>
public class AbilitySpellCard : MonoBehaviour
{
    /// <summary>
    /// Property to know which spell this card contains.
    /// </summary>
    public ISpell SpellOnCard { get; set; }

    /// <summary>
    /// Property used to know which spell was obtained.
    /// </summary>
    public ISpell NewObtainedSpell { get; set; }

    // Components
    private TextMeshProUGUI textInCard;
    private PlayerSpells playerSpells;
    private PlayerInteraction playerInteraction;
    private AbilitiesCanvas abilitiesCanvas;

    // Full spells canvas
    [SerializeField] private GameObject fullSpellsCanvas;

    private void Awake()
    {
        abilitiesCanvas = GetComponentInParent<AbilitiesCanvas>();
        playerInteraction = FindObjectOfType<PlayerInteraction>();
        playerSpells = FindObjectOfType<PlayerSpells>();
        textInCard = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        UpdateInformation();
    }

    /// <summary>
    /// Updates card info.
    /// </summary>
    public void UpdateInformation()
    {
        if (SpellOnCard != null && textInCard != null)
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
            Destroy(playerInteraction.LastObjectInteracted.gameObject);
        }
        // Else if there are not slots, it will open a new canvas with 4 slots to select.
        else
        {
            // Activates new canvas and sets the currently selected spell on that canvas
            // THIS MUST BE BEFORE SETTING ACTIVE BECAUSE CANVAS WILL HAVE LOGIC ON ITS ENABLE
            fullSpellsCanvas.GetComponent<AbilityFullSpellChoice>().NewObtainedSpell = SpellOnCard; 
            fullSpellsCanvas.SetActive(true);
        }
    }

    /// <summary>
    /// This method is called with button press if it's the full spell menu canvas.
    /// </summary>
    /// <param name="slot"></param>
    public void AddSpellToSlot(int slot)
    {
        if (playerSpells.CurrentSpells[slot] != null)
        {
            // Destroys the spell scroll
            Destroy(playerInteraction.LastObjectInteracted.gameObject);

            // Drops a spell and updates player's spell list
            playerSpells.DropSpell(playerSpells.CurrentSpells[slot] as SpellSO);
            playerSpells.RemoveSpell(slot);
            playerSpells.AddSpell(NewObtainedSpell as SpellSO, slot);

            // Disables all abilities ui canvas
            abilitiesCanvas.DisableAll();
        }
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
                return true;
            }
        }
        return false;
    }
}
