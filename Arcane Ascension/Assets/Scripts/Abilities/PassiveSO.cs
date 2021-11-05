using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable object responsible for creating passive abilities.
/// </summary>
[CreateAssetMenu(menuName = "Passive", fileName = "Passive")]
public class PassiveSO : ScriptableObject, IPassive
{
    [SerializeField] private string notes;

    [SerializeField] private List<UpgradeStats> upgrades;
    
    /// <summary>
    /// Updates player's stats.
    /// </summary>
    public void UpdateStats()
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();

        foreach(UpgradeStats upgrade in upgrades)
        {
            upgrade.Upgrade(playerStats);
        }
    }

    /// <summary>
    /// Private struct to upgrade choose stats upgrades.
    /// </summary>
    [Serializable]
    private struct UpgradeStats
    {
        [Range(-1f, 1f)] [SerializeField] private float amountToAdd;
        [SerializeField] private StatsType statsType;

        public void Upgrade(PlayerStats playerStats) =>
            playerStats.UpdateStats(amountToAdd, statsType);
    }
}
