using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptable object responsible for creating run passive abilities.
/// </summary>
[CreateAssetMenu(menuName = "Passive/Run Passive", fileName = "Passive")]
public class RunPassiveSO : AbstractPassiveSO<RunPassiveType>, IRunPassive
{
    [BoxGroup("Passive General Attributes")]
    [SerializeField] private RunPassiveType passiveType;

    [BoxGroup("Passive General Attributes")]
    [SerializeField] private List<UpgradeStats> upgrades;

    public override RunPassiveType PassiveType => passiveType;

    /// <summary>
    /// Updates player's stats with all upgrades contained on this passive.
    /// </summary>
    public void Execute(PlayerStats playerStats)
    {
        foreach (UpgradeStats upgrade in upgrades)
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

