using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using ExtensionMethods;

/// <summary>
/// Scriptable object with stats for an enemy.
/// </summary>
[InlineEditor]
[CreateAssetMenu(menuName = "Character/Stats/Enemy Stats", fileName = "Enemy Stats")]
public class EnemyStatsSO : StatsSO
{
    [BoxGroup("General Stats")]
    [Tooltip("Minimum and maximum random delay of attack")]
    [RangeMinMax(1, 20f)] [SerializeField] private Vector2 attackingDelay;

    [BoxGroup("General Stats")]
    [RangeMinMax(0, 1000)] [SerializeField] private Vector2 goldQuantity;
    
    [BoxGroup("General Stats")]
    [RangeMinMax(0, 1000)] [SerializeField] private Vector2 arcanePowerQuantity;
    
    // List with all possible loots in enemy
    [BoxGroup("General Stats")]
    [Header("Healing Potion is only affected by CharacterSaveDataController Reaper variable. " +
        "Rate can be ignored.")]
    [SerializeField] private LootRates lootRates;
    
    [BoxGroup("Damage Stats")]
    [Header("Character list of spells")]
    [SerializeField] private List<EnemySpell> availableSpells;

    /// <summary>
    /// 1 is default value, the lesser this number, the fewer attacks
    /// the enemy will perform.
    /// </summary>
    public float AttackingSpeedReductionMultiplier { get; set; }

    /// <summary>
    /// Attacking delay with a multiplier applied.
    /// </summary>
    public Vector2 AttackingDelay => 
        attackingDelay * AttackingSpeedReductionMultiplier;

    public Vector2 GoldQuantity =>
        goldQuantity;

    public Vector2 ArcanePowerQuantity =>
        arcanePowerQuantity;

    public LootRates Rates => 
        lootRates;

    public List<EnemySpell> AllEnemySpells => 
        availableSpells;

    /// <summary>
    /// Initiates lootrates struct.
    /// </summary>
    public void Initialize()
    {
        AttackingSpeedReductionMultiplier = 1f;
    }

    /// <summary>
    /// Struct with loot pieces and a list with all loot the enemy dropped.
    /// </summary>
    [Serializable]
    public struct LootRates
    {
        [SerializeField] private List<LootPiece> lootPieces;
        public List<LootPiece> LootPieces => lootPieces;

        /// <summary>
        /// Class with a type of loot and its chance of being dropped.
        /// </summary>
        [Serializable]
        public class LootPiece
        {
            [SerializeField] private LootType lootType;
            [Range(0f, 100f)] [SerializeField] private float lootRate;

            public LootType LootType => lootType;
            public float LootRate { get => lootRate; set => lootRate = value; }
        }
    }
}
