using UnityEngine;

/// <summary>
/// Class responsible for controlling spellbook spell slots.
/// </summary>
public class SpellBookSpells : MonoBehaviour, IFindPlayer
{
    private SpellBookSpell[] spellBookSpells;
    private PlayerSpells playerSpells;

    public DisableType DisableType => throw new System.NotImplementedException();

    private void Awake()
    {
        spellBookSpells = GetComponentsInChildren<SpellBookSpell>();
        FindPlayer();
    }

    private void Start()
    {
        UpdateSpellSlots();
    }

    public void UpdateSpellSlots()
    {
        for (int i = 0; i < spellBookSpells.Length; i++)
        {
            spellBookSpells[i].Spell = playerSpells.CurrentSpells[i];
            spellBookSpells[i].UpdateSpellSlotImage();
        }
    }

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
