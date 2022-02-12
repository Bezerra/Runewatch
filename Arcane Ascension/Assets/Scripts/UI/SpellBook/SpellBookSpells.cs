using UnityEngine;

/// <summary>
/// Class responsible for controlling spellbook spell slots.
/// </summary>
public class SpellBookSpells : MonoBehaviour, IFindPlayer
{
    private SpellBookSpell[] spellBookSpells;
    private PlayerSpells playerSpells;
    private ActivateSpellBook activateSpellBook;

    private void Awake()
    {
        spellBookSpells = GetComponentsInChildren<SpellBookSpell>(true);
        activateSpellBook = GetComponentInParent<ActivateSpellBook>();
        FindPlayer();
    }

    /// <summary>
    /// Updates all spellbook spells Spell property and images.
    /// </summary>
    public void UpdateSpellSlots()
    {
        for (int i = 0; i < spellBookSpells.Length; i++)
        {
            if (playerSpells != null &&
                playerSpells.CurrentSpells != null &&
                playerSpells.CurrentSpells.Length > 0)
            {
                spellBookSpells[i].Spell = playerSpells.CurrentSpells[i];
            }
        }
    }

    /// <summary>
    /// Reorganizes player spells to be the same as the spellbook.
    /// </summary>
    public void ReorganizePlayerSpells()
    {
        spellBookSpells = GetComponentsInChildren<SpellBookSpell>();
        for (int i = 0; i < spellBookSpells.Length; i++)
        {
            if (playerSpells != null &&
                playerSpells.CurrentSpells != null &&
                playerSpells.CurrentSpells.Length > 0)
            {
                playerSpells.CurrentSpells[i] = spellBookSpells[i].Spell;
            }
        }

        for (int i = 0; i < playerSpells.CurrentSpells.Length; i++)
        {
            if (playerSpells != null &&
                playerSpells.CurrentSpells != null &&
                playerSpells.CurrentSpells.Length > 0)
            {
                if (playerSpells.CurrentSpells[i] == 
                    activateSpellBook.SelectedSpellOnBookOpen)
                {
                    playerSpells.CurrentSpellIndex = (byte)i;
                }
            }
        }
    }

    public void FindPlayer()
    {
        playerSpells = FindObjectOfType<PlayerSpells>();
    }

    public void PlayerLost()
    {
        // left blank on purpose
    }
}
