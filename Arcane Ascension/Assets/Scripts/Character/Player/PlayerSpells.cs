using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for controlling player spells and shot action.
/// </summary>
public class PlayerSpells : MonoBehaviour, ISaveable
{
    private PlayerInputCustom input;
    private Stats playerStats;
    private IList<SpellSO> allSpells;

    // Array with all available spells.
    public ISpell[] CurrentSpells { get; private set; }
    private byte currentSpellIndex;

    // Currently active spell from available spells
    public ISpell ActiveSpell => CurrentSpells[currentSpellIndex];

    private void Awake()
    {
        input = FindObjectOfType<PlayerInputCustom>();
        playerStats = GetComponent<Stats>();
        allSpells = FindObjectOfType<AllSpells>().SpellList;

        CurrentSpells = new SpellSO[4];
    }

    private void Start()
    {
        // TEMPORARY TESTS // Adds 4 spells to current spells
        for (int i = 0; i < CurrentSpells.Length; i++)
        {
            if (i < allSpells.Count)
            {
                if (allSpells[i] != null)
                    AddSpell(allSpells[i]);
            }
        }

        StartSpellCooldown();
    }

    // Registers events
    private void OnEnable()
    {
        input.SelectSpell += SelectSpell;
    }

    private void OnDisable()
    {
        input.SelectSpell -= SelectSpell;
    }

    /// <summary>
    /// Starts cooldown count for all spell coroutine.
    /// </summary>
    public void StartSpellCooldown()
    {
        for (int i = 0; i < CurrentSpells.Length; i++)
        {
            if (CurrentSpells[i] != null && this != null)
                StartCoroutine(StartSpellCooldownCoroutine(CurrentSpells[i]));
        }
    }

    /// <summary>
    /// Starts cooldown count for one spell coroutine.
    /// </summary>
    public void StartSpellCooldown(ISpell spell) =>
        StartCoroutine(StartSpellCooldownCoroutine(spell));

    /// <summary>
    /// Starts cooldown count for a spell. 
    /// </summary>
    /// <returns>Wait for fixed update.</returns>
    private IEnumerator StartSpellCooldownCoroutine(ISpell spell)
    {
        YieldInstruction wffu = new WaitForFixedUpdate();

        spell.CooldownCounter = 0;

        float currentTime = Time.time;
        while (Time.time < currentTime + spell.Cooldown)
        {
            // Sets cooldown from 0 to active spell cooldown.
            spell.CooldownCounter = (Time.time - currentTime);
            yield return wffu;
        }
        spell.CooldownCounter = spell.Cooldown;
    }

    /// <summary>
    /// Sets a spell as active spell.
    /// </summary>
    /// <param name="index">Index of the spell on array.</param>
    private void SelectSpell(byte index)
    {
        if (CurrentSpells[index] != null)
            currentSpellIndex = index;
    }

    /// <summary>
    /// Add spell to current spells.
    /// </summary>
    /// <param name="spellToAdd">Spell to add.</param>
    public void AddSpell(SpellSO spellToAdd)
    {
        if (CurrentSpells.Contains(spellToAdd) == false)
        {
            for (int i = 0; i < CurrentSpells.Length; i++)
            {
                if (CurrentSpells[i] == null)
                {
                    if (playerStats.Attributes.AvailableSpells.Contains(spellToAdd) == false)
                        playerStats.Attributes.AvailableSpells.Add(spellToAdd);
                    CurrentSpells[i] = spellToAdd;
                    StartSpellCooldown(CurrentSpells[i]);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Removes a spell from current spells.
    /// </summary>
    /// <param name="indexOfSpellToRemove">Spell to remove.</param>
    public void RemoveSpell(int indexOfSpellToRemove)
    {
        if (indexOfSpellToRemove > 0)
            CurrentSpells[indexOfSpellToRemove] = null;
        else
            throw new System.Exception("Can't remove default spell.");
    }

    /// <summary>
    /// Swaps a spell from a slot to another.
    /// </summary>
    /// <param name="spell1">Spell slot to swap.</param>
    /// <param name="spell2">Desired new slot for the selected spell.</param>
    public void SwapSpellSlot(int spell1, int spell2)
    {
        bool canSwap = true;
        foreach (ISpell spell in CurrentSpells)
            if (spell.CooldownCounter != spell.Cooldown)
                canSwap = false;

        if (canSwap)
        {
            if (spell1 > 0 && spell2 > 0)
            {
                ISpell temporarySlotSpell = CurrentSpells[spell2];
                CurrentSpells[spell2] = CurrentSpells[spell1];
                CurrentSpells[spell1] = temporarySlotSpell;
            }
            else
                throw new System.Exception("Can't swap default spell.");
        }
    }

    public void SaveCurrentData(SaveData saveData)
    {
        // Checks which spells the player current has
        byte[] currentSpells = new byte[4];
        for (int i = 0; i < CurrentSpells.Length; i++)
        {
            if (CurrentSpells[i] == null)
                currentSpells[i] = 0;
            else
                currentSpells[i] = CurrentSpells[i].SpellID;
        }

        // Saves spells and current selected spell
        saveData.PlayerSavedData.CurrentSpells = currentSpells;
        saveData.PlayerSavedData.CurrentSpellIndex = currentSpellIndex;
    }

    public IEnumerator LoadData(SaveData saveData)
    {
        yield return new WaitForFixedUpdate();

        // Loads current spell list
        byte[] savedSpells = saveData.PlayerSavedData.CurrentSpells;
        for (int i = 0; i < savedSpells.Length; i++)
        {
            if (savedSpells[i] != 0)
            {
                for (int j = 0; j < allSpells.Count; j++)
                {
                    if (savedSpells[i] == allSpells[j].SpellID)
                    {
                        CurrentSpells[i] = allSpells[j];
                        break;
                    }
                }
            }
            else
            {
                CurrentSpells[i] = null;
            }
        }

        // Loads current spell selected
        currentSpellIndex = saveData.PlayerSavedData.CurrentSpellIndex;

        StartSpellCooldown();
    }
}
