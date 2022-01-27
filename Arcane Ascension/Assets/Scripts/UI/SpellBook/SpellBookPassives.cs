using UnityEngine;

/// <summary>
/// Class responsible for controlling spellbook passive slots.
/// </summary>
public class SpellBookPassives : MonoBehaviour, IFindPlayer
{
    private SpellBookPassive[] spellBookPassives;
    private PlayerStats playerStats;

    private void Awake()
    {
        spellBookPassives = GetComponentsInChildren<SpellBookPassive>(true);
        FindPlayer();
    }

    /// <summary>
    /// Updates all spellbook spells Spell property and images.
    /// </summary>
    public void UpdatePassiveSlots()
    {
        if (playerStats.CurrentPassives != null &&
            playerStats.CurrentPassives.Count > 0)
        {
            for (int i = 0; i < playerStats.CurrentPassives.Count; i++)
            {
                if (playerStats.CurrentPassives[i] != null)
                    spellBookPassives[i].Passive = playerStats.CurrentPassives[i];
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
