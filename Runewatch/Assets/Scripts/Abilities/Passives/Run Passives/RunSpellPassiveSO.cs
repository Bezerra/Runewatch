using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptable object responsible for creating run passive abilities.
/// </summary>
[CreateAssetMenu(menuName = "Passive/Run Passive", fileName = "Passive")]
public class RunSpellPassiveSO : AbstractPassiveSO<RunStatPassiveType>,IRunSpellPassive
{
    [BoxGroup("Passive General Attributes")]
    [SerializeField] private RunStatPassiveType passiveType;

    [BoxGroup("Passive General Attributes")]
    [SerializeField] private List<UpgradeStats> upgrades;

    public override RunStatPassiveType PassiveType => passiveType;

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

    public void Execute(PlayerSpells playerSpells)
    {
        if (playerSpells != null)
        {
            foreach (SpellSO spell in playerSpells.CurrentSpells)
            {
                //spell.playerDamage = new Vector2 (spell.playerDamage.x * smth,spell.playerDamage.y * smth)
            }
        }
    }

    /// <summary>
    /// Private struct to upgrade choose stats upgrades.
    /// </summary>
    [Serializable]
    private struct UpgradeStats
    {
        [Range(0, 15)] [SerializeField] private int amountToAdd;
        [SerializeField] private StatsType statsType;

        public void Upgrade(PlayerStats playerStats) =>
            playerStats.UpdateStats(amountToAdd, statsType);
    }
}

