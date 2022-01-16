using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptable object with stats for an enemy.
/// </summary>
[InlineEditor]
[CreateAssetMenu(menuName = "Character/Stats/Enemy Stats", fileName = "Enemy Stats")]
public class EnemyStatsSO : StatsSO
{
    [BoxGroup("General Stats")]
    [Tooltip("Minimum and maximum random delay of attack")]
    [RangeMinMax(0, 20f)] [SerializeField] private Vector2 attackingDelay;

    // PODE FICAR POR AGORA CASO SE VOLTE AO ANTERIOR, APAGAR SE OS DROPS FICAREM NOS ROOMS
    //[BoxGroup("General Stats")]
    //[RangeMinMax(0, 1000)] [SerializeField] private Vector2 goldQuantity;

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

    public Vector2 ArcanePowerQuantity =>
        arcanePowerQuantity;

    public LootRates Rates => 
        lootRates;

    public List<EnemySpell> AllEnemySpells => 
        availableSpells;

    /// <summary>
    /// Initiates lootrates struct.
    /// </summary>
    public override void Initialize()
    {
        base.Initialize();
        AttackingSpeedReductionMultiplier = 1f;
    }
}
