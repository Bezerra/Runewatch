using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for controlling spellbook passive slots.
/// </summary>
public class SpellBookPassives : MonoBehaviour, IFindPlayer
{
    private List<SpellBookPassive> spellBookPassives;
    private PlayerStats playerStats;

    private void Awake()
    {
        spellBookPassives = new List<SpellBookPassive>();
        foreach(SpellBookPassive spellbookPassive in 
            GetComponentsInChildren<SpellBookPassive>(true))
        {
            spellBookPassives.Add(spellbookPassive);
        }
        FindPlayer();
    }

    /// <summary>
    /// Updates all spellbook spells Spell property and images.
    /// </summary>
    public void UpdatePassiveSlots()
    {
        if (playerStats != null &&
            playerStats.CurrentPassives != null &&
            playerStats.CurrentPassives.Count > 0)
        {
            for (int i = 0; i < playerStats.CurrentPassives.Count; i++)
            {
                if (playerStats.CurrentPassives[i] != null)
                    spellBookPassives[i].Passive = playerStats.CurrentPassives[i];
            }

            // Sorts by name
            spellBookPassives.Sort();

            // Orders as first sibling (list is in reverse order, so it will set
            // the last elements as first sibling first
            for (int i = spellBookPassives.Count - 1; i >= 0; i--)
            {
                spellBookPassives[i].transform.SetAsFirstSibling();
            }
        }
    }

    public void FindPlayer()
    {
        playerStats = FindObjectOfType<PlayerStats>();
    }

    public void PlayerLost()
    {
        // left blank on purpose
    }
}
