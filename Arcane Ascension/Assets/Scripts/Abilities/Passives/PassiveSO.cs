#if UNITY_EDITOR
using UnityEditor;
#endif

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
    [BoxGroup("General")]
    [HorizontalGroup("General/Split", 72)]
    [HideLabel, PreviewField(72)] [SerializeField] protected Sprite icon;

    [VerticalGroup("General/Split/Middle", 1), LabelWidth(60)]
    [InlineButton("ChangeFileName", "Update File Name")]
    [SerializeField] private new string name = "New passive";

    [VerticalGroup("General/Split/Middle", 1), LabelWidth(60)]
    [Tooltip("Single ID for every spell. No spells should have the same ID")]
    [Range(0, 255)] [SerializeField] private byte passiveID;

    [VerticalGroup("General/Split/Middle", 1), LabelWidth(60)]
    [Tooltip("Spell tier")]
    [Range(1, 7)] [SerializeField] private int passiveTier;

    [VerticalGroup("General/Split/Right", 2)]
    [HideLabel, TextArea(4, 6), SerializeField] private string description;

    [BoxGroup("Spell General Attributes")]
    [SerializeField] private PassiveType passiveType;

    [BoxGroup("Spell General Attributes")]
    [SerializeField] private List<UpgradeStats> upgrades;

    public int Tier => passiveTier;
    public PassiveType PassiveType => passiveType;
    public string Name => name;
    public string Description => description;
    public byte ID => passiveID;
    public Sprite Icon => icon;

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

#if UNITY_EDITOR
    protected void ChangeFileName()
    {
        string assetPath = AssetDatabase.GetAssetPath(this.GetInstanceID());
        AssetDatabase.RenameAsset(assetPath, name);
        AssetDatabase.SaveAssets();
    }
#endif
}
