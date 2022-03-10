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
    private readonly float[] HPDIFFICULTYMULTIPLIERS =
        new float[] { 1f, 1.5f, 2f, 3f };

    private readonly float[] DAMAGEDIFFICULTYMULTIPLIERS =
        new float[] { 1f, 1.2f, 1.5f, 2f };

    private readonly float[] APDIFFICULTYMULTIPLIERS =
        new float[] { 1f, 1.5f, 2f, 2.5f };

    private readonly float[] HPFLOORMULTIPLIERS =
        new float[] { 0.8f, 1f, 1f, 1.25f, 1.25f, 1.25f, 1.5f, 1.5f, 1.5f };

    private readonly float[] DAMAGEFLOORMULTIPLIERS =
        new float[] { 0.8f, 1f, 1f, 1.25f, 1.25f, 1.25f, 1.5f, 1.5f, 1.5f };

    private readonly float[] APFLOORMULTIPLIERS =
        new float[] { 1f, 1f, 1f, 1.25f, 1.25f, 1.25f, 1.5f, 1.5f, 1.5f };

    [BoxGroup("General Stats")]
    [Tooltip("Minimum and maximum random delay of attack")]
    [RangeMinMax(0, 20f)] [SerializeField] private Vector2 attackingDelay;

    // PODE FICAR POR AGORA CASO SE VOLTE AO ANTERIOR, APAGAR SE OS DROPS FICAREM NOS ROOMS
    //[BoxGroup("General Stats")]
    //[RangeMinMax(0, 1000)] [SerializeField] private Vector2 goldQuantity;

    [BoxGroup("General Stats")]
    [RangeMinMax(0, 1000)] [SerializeField] private Vector2 arcanePowerQuantity;
    private float arcanePowerMultiplierWithDifficulty;
    
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
        arcanePowerQuantity * arcanePowerMultiplierWithDifficulty;

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

        // GAME DIFFICULTY LOGIC
        RunSaveDataController runData = FindObjectOfType<RunSaveDataController>();
        arcanePowerMultiplierWithDifficulty = 1;

        switch (runData.SaveData.Difficulty)
        {
            case "Normal":
                MaxHealth *= HPDIFFICULTYMULTIPLIERS[0];
                BaseDamageMultiplier *= DAMAGEDIFFICULTYMULTIPLIERS[0];
                arcanePowerMultiplierWithDifficulty = APDIFFICULTYMULTIPLIERS[0];
                break;
            case "Medium":
                MaxHealth *= HPDIFFICULTYMULTIPLIERS[1];
                BaseDamageMultiplier *= DAMAGEDIFFICULTYMULTIPLIERS[1];
                arcanePowerMultiplierWithDifficulty = APDIFFICULTYMULTIPLIERS[1];
                break;
            case "Hard":
                MaxHealth *= HPDIFFICULTYMULTIPLIERS[2];
                BaseDamageMultiplier *= DAMAGEDIFFICULTYMULTIPLIERS[2];
                arcanePowerMultiplierWithDifficulty = APDIFFICULTYMULTIPLIERS[2];
                break;
            case "Extreme":
                MaxHealth *= HPDIFFICULTYMULTIPLIERS[3];
                BaseDamageMultiplier *= DAMAGEDIFFICULTYMULTIPLIERS[3];
                arcanePowerMultiplierWithDifficulty = APDIFFICULTYMULTIPLIERS[3];
                break;
        }

        // If it's a boss, ignores all this
        if (Type == CharacterType.Boss) return;

        // FLOOR DIFFICULTY LOGIC
        int currentFloor = runData.SaveData.DungeonSavedData.Floor - 1;

        MaxHealth *= HPFLOORMULTIPLIERS[currentFloor];
        BaseDamageMultiplier *= DAMAGEFLOORMULTIPLIERS[currentFloor];
        arcanePowerMultiplierWithDifficulty = APFLOORMULTIPLIERS[currentFloor];
    }
}
