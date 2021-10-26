using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scritable object responsible for getting 3 random spells from all spells list.
/// </summary>
[CreateAssetMenu(menuName = "Events/Event Get Chest Spell",
    fileName = "Event Get Chest Spell")]
public class EventGetChestSpellsSO : EventAbstractSO
{
    private IList<SpellSO> allSpells;
    private PlayerSpells playerSpells;

    public override void Execute()
    {
        if (playerSpells == null)
            playerSpells = FindObjectOfType<PlayerSpells>();

        allSpells = new List<SpellSO>();
        List<SpellSO> allSpellsDefault = FindObjectOfType<AllSpells>().SpellList;
        foreach (SpellSO spell in allSpellsDefault)
            allSpells.Add(spell);


        // Array with 3 random spells
        ISpell[] results = GetSpell();
        Debug.Log("Got 3 spells");
    }

    private ISpell[] GetSpell()
    {
        ISpell[] resultSpells = new ISpell[3];

        // Starts by removing the spells that player already has from the list of all spells
        foreach (SpellSO spell in playerSpells.CurrentSpells)
            allSpells.Remove(spell);

        for (int i = 0; i < 3; i++)
        {
            // Leave it for now, can delete later
            while (true)
            {
                // Default spell will always take 1 slot
                // And if third spell is set
                if (allSpells.Count == 1)
                    break;

                // Starts on 1 because spell 0 is default spell
                int spellIndex = Random.Range(1, allSpells.Count);

                // Gets spell and removes it from all spells list
                resultSpells[i] = allSpells[spellIndex];
                allSpells.Remove(allSpells[spellIndex]);
                break;
            }
        }
        return resultSpells;
    }
}
