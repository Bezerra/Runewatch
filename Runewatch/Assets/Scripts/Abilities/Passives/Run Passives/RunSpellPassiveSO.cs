using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptable object responsible for creating run passive abilities.
/// </summary>
[CreateAssetMenu(menuName = "Passive/Run Passive", fileName = "Passive")]
public class RunSpellPassiveSO : AbstractPassiveSO<RunSpellPassiveType>,IRunSpellPassive
{
    [BoxGroup("Passive General Attributes")]
    [SerializeField] private RunSpellPassiveType passiveType;

    [BoxGroup("Passive General Attributes")]
    [SerializeField] private List<UpgradeStats> upgrades;

    public override RunSpellPassiveType PassiveType => passiveType;

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

