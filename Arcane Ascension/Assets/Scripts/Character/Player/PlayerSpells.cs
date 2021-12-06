using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Class responsible for controlling player spells and shot action.
/// </summary>
public class PlayerSpells : MonoBehaviour, ISaveable
{
    [Range(0, 4)][SerializeField] private int ADDINITIALSPELLSFORTESTS;

    // Components
    private IInput input;
    private PlayerHandEffect playerHandEffect;
    private PlayerStats playerStats;
    private IList<SpellSO> allSpells;
    private SpellSO defaultSpell;
    private PlayerCastSpell playerCastSpell;

    // Array with all available spells
    public ISpell[] CurrentSpells { get; private set; }

    // Currently selected spell index
    public byte CurrentSpellIndex { get; private set; }

    // Currently active spell from available spells
    public ISpell ActiveSpell => CurrentSpells[CurrentSpellIndex];
    public ISpell SecondarySpell => defaultSpell;

    // Coroutines
    private YieldInstruction wffu;

    // Spell select scroll
    private float currentTimeScrollChanged;
    private readonly float DELAYTOSCROLLCHANGE = 0.5f;

    private void Awake()
    {
        input = FindObjectOfType<PlayerInputCustom>();
        playerHandEffect = FindObjectOfType<PlayerHandEffect>();
        playerStats = GetComponent<PlayerStats>();
        allSpells = FindObjectOfType<AllSpells>().SpellList;
        defaultSpell = FindObjectOfType<AllSpells>().DefaultSpell;
        playerCastSpell = GetComponent<PlayerCastSpell>();

        CurrentSpells = new SpellSO[4];
        wffu = new WaitForFixedUpdate();
    }

    private void Start()
    {
        if (ADDINITIALSPELLSFORTESTS > 0)
        {
            //// TEMPORARY TESTS // Adds 4 spells to current spells
            for (int i = 0; i < ADDINITIALSPELLSFORTESTS; i++)
            {
                if (i <= CurrentSpells.Length)
                {
                    // THIS IS TEMP, WHILE IN GAME PLAYER WILL ONLY CAST THE SPELLS ON HIS AVAILABLE SPELLS, NOT THIS ONE
            
                    AddSpell(allSpells[i]);
                }
            }
        }





        if (CurrentSpells[0] != null)
        {
            SelectSpell(0, true);

            StartSpellCooldown();
        }

        StartSpellCooldown(SecondarySpell);
    }

    // Registers events
    private void OnEnable()
    {
        input.SelectSpell += SelectSpell;
        playerCastSpell.EventStartCooldown += StartSpellCooldown;
        input.PreviousNextSpell += SelectNextAndPreviousSpell;
    }

    private void OnDisable()
    {
        input.SelectSpell -= SelectSpell;
        playerCastSpell.EventStartCooldown -= StartSpellCooldown;
        input.PreviousNextSpell -= SelectNextAndPreviousSpell;
    }

    /// <summary>
    /// Selects spells with scroll
    /// </summary>
    /// <param name="axis"></param>
    private void SelectNextAndPreviousSpell(float axis, bool withDelay)
    {
        if (withDelay)
        {
            if (Time.time - currentTimeScrollChanged > DELAYTOSCROLLCHANGE)
            {
                SelectNextAndPreviousSpellLogic(axis);
            }
        }
        else
        {
            SelectNextAndPreviousSpellLogic(axis);
        }
    }

    /// <summary>
    /// Selects previous and next spell with a number.
    /// </summary>
    /// <param name="axis">Positive or negative number.</param>
    private void SelectNextAndPreviousSpellLogic(float axis)
    {
        int finalIndex = CurrentSpellIndex;

        if (axis < 0)
        {
            if (finalIndex + 1 < CurrentSpells.Length)
            {
                finalIndex++;
            }
            else
            {
                finalIndex = 0;
            }
        }
        if (axis > 0)
        {
            if (finalIndex - 1 >= 0)
            {
                finalIndex--;
            }
            else
            {
                finalIndex = CurrentSpells.Length - 1;
            }
        }
        currentTimeScrollChanged = Time.time;
        SelectSpell((byte)finalIndex);
    }

    /// <summary>
    /// Starts cooldown count for all spell coroutine.
    /// </summary>
    private void StartSpellCooldown()
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
    private void StartSpellCooldown(ISpell spell) =>
        StartCoroutine(StartSpellCooldownCoroutine(spell));

    /// <summary>
    /// Starts cooldown count for a spell. 
    /// </summary>
    /// <returns>Wait for fixed update.</returns>
    private IEnumerator StartSpellCooldownCoroutine(ISpell spell)
    {
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
        if (playerCastSpell.CurrentlyCasting == false)
        {
            // If the player selects an active spell different than the one currently selected
            if (CurrentSpells[index] != null)
            {
                if (index != CurrentSpellIndex)
                {
                    if (CurrentSpells[index] != null)
                        CurrentSpellIndex = index;

                    playerHandEffect.UpdatePlayerHandEffect(ActiveSpell);
                }
            }
        }
    }

    /// <summary>
    /// Sets a spell as active spell on game start.
    /// </summary>
    /// <param name="index">Index of the spell on array.</param>
    private void SelectSpell(byte index, bool initialSelection)
    {
        if (CurrentSpells[index] != null)
        {
            CurrentSpellIndex = index;

            playerHandEffect.UpdatePlayerHandEffect(ActiveSpell);

            StartSpellCooldown();
        }     
    }

    /// <summary>
    /// Checks if spell cooldown is over.
    /// </summary>
    /// <param name="spell">Spell to check.</param>
    /// <returns>Returns true if cooldown is over.</returns>
    public bool CooldownOver(ISpell spell)
    {
        if (CurrentSpells.Length > 0)
        {
            if (spell.CooldownCounter == spell.Cooldown)
                return true;
        }
        return false;
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
                    if (playerStats.PlayerAttributes.AvailableSpells.Contains(spellToAdd) == false)
                        playerStats.PlayerAttributes.AvailableSpells.Add(spellToAdd);
                    CurrentSpells[i] = spellToAdd;
                    StartSpellCooldown(CurrentSpells[i]);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Add spell to current spells.
    /// </summary>
    /// <param name="spellToAdd">Spell to add.</param>
    /// <param name="slotNumber">Slot number to add the spell to.</param> 
    public void AddSpell(SpellSO spellToAdd, int slotNumber)
    {
        if (CurrentSpells.Contains(spellToAdd) == false)
        {
            for (int i = 0; i < CurrentSpells.Length; i++)
            {
                if (CurrentSpells[i] == null && i == slotNumber)
                {
                    if (playerStats.PlayerAttributes.AvailableSpells.Contains(spellToAdd) == false)
                        playerStats.PlayerAttributes.AvailableSpells.Add(spellToAdd);
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
        if (CurrentSpells[indexOfSpellToRemove] != null)
            CurrentSpells[indexOfSpellToRemove] = null;
    }

    /// <summary>
    /// Creates a new spell scroll with the dropped spell.
    /// </summary>
    /// <param name="spellToDrop">Spell to drop.</param>
    public void DropSpell(SpellSO spellToDrop)
    {
        GameObject spellDropped =
            ItemLootPoolCreator.Pool.InstantiateFromPool(LootType.KnownSpell.ToString(),
            transform.position + transform.forward, Quaternion.identity);

        spellDropped.transform.RotateTo(transform.position);

        // Updates dropped spell information with the dropped spell
        if (spellDropped.TryGetComponent(out IDroppedSpell droppedSpell))
            droppedSpell.DroppedSpell = spellToDrop;
        if (spellDropped.TryGetComponent(out IInteractableWithCanvas interectableCanvas))
            interectableCanvas.UpdateInformation(spellToDrop.Name);
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
            if (spell1 >= 0 && spell1 < 4 &&
                spell2 >= 0 && spell2 < 4)
            {
                ISpell temporarySlotSpell = CurrentSpells[spell2];
                CurrentSpells[spell2] = CurrentSpells[spell1];
                CurrentSpells[spell1] = temporarySlotSpell;
            }
        }
    }

    public void SaveCurrentData(RunSaveData saveData)
    {
        // Checks which spells the player current has
        byte[] currentSpells = new byte[4];
        for (int i = 0; i < CurrentSpells.Length; i++)
        {
            if (CurrentSpells[i] == null)
                currentSpells[i] = 0;
            else
                currentSpells[i] = CurrentSpells[i].ID;
        }

        // Saves spells and current selected spell
        saveData.PlayerSavedData.CurrentSpells = currentSpells;
        saveData.PlayerSavedData.CurrentSpellIndex = CurrentSpellIndex;
    }

    public IEnumerator LoadData(RunSaveData saveData)
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
                    if (savedSpells[i] == allSpells[j].ID)
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
        CurrentSpellIndex = saveData.PlayerSavedData.CurrentSpellIndex;

        StartSpellCooldown();
    }
}
