using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptable object responsible for creating passive abilities.
/// </summary>
[InlineEditor]
[CreateAssetMenu(menuName = "Passive", fileName = "Passive")]
public class PassiveSO : ScriptableObject, IPassive
{
    [SerializeField] private new string name;
    [SerializeField] private string description;
    [Range(0, 255)] [SerializeField] private byte passiveID;

    [Range(1, 7)] [SerializeField] private int passiveTier;
    [SerializeField] private PassiveType passiveType;
    [SerializeField] private List<UpgradeStats> upgrades;

    public int PassiveTier => passiveTier;
    public PassiveType PassiveType => passiveType;
    public string Name => name;
    public string Description => description;
    public byte PassiveID => passiveID;

    /// <summary>
    /// Updates player's stats.
    /// </summary>
    public void Execute(PlayerStats playerStats)
    {
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

        public StatsType PassiveType => statsType;

        public void Upgrade(PlayerStats playerStats) =>
            playerStats.UpdateStats(amountToAdd, statsType);
    }
}
