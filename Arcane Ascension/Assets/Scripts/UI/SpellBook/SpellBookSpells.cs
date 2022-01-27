using UnityEngine;

/// <summary>
/// Class responsible for controlling spellbook spell slots.
/// </summary>
public class SpellBookSpells : MonoBehaviour, IFindPlayer
{
    private SpellBookSpell[] spellBookSpells;
    private PlayerSpells playerSpells;

    private void Awake()
    {
        spellBookSpells = GetComponentsInChildren<SpellBookSpell>(true);
        FindPlayer();
    }

    /// <summary>
    /// Updates all spellbook spells Spell property and images.
    /// </summary>
    public void UpdateSpellSlots()
    {
        for (int i = 0; i < spellBookSpells.Length; i++)
        {
            spellBookSpells[i].Spell = playerSpells.CurrentSpells[i];
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
            playerSpells.CurrentSpells[i] = spellBookSpells[i].Spell;
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
