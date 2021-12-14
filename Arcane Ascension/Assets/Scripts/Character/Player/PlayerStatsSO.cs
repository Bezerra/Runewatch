using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptable object with stats for a player.
/// </summary>
[InlineEditor]
[CreateAssetMenu(menuName = "Character/Stats/Player Stats", fileName = "Player Stats")]
public class PlayerStatsSO : StatsSO
{
    [BoxGroup("General Stats")]
    [Range(0, 2000)] [SerializeField] private float defaultArmor;
    [BoxGroup("Mana Stats")]
    [Range(0, 2000)] [SerializeField] private float defaultMana;
    [BoxGroup("Mana Stats")]
    [Range(0.1f, 10)] [SerializeField] private float defaultManaRegenAmount;
    [BoxGroup("Mana Stats")]
    [Range(0.01f, 2)] [SerializeField] private float defaultManaRegenTime;
    [BoxGroup("Mana Stats")]
    [Range(0.1f, 20f)] [SerializeField] private float defaultManaRegenSteal;
    [BoxGroup("General Stats")]
    [Range(1, 2)] [SerializeField] private int defaultDashCharge;
    [BoxGroup("General Stats")]
    [Range(1, 2f)] [SerializeField] private float healthPotionsPercentageMultiplier;
    [BoxGroup("Damage Stats")]
    [Header("Character list of spells")]
    [SerializeField] private List<SpellSO> availableSpells;

    public float MaxMana { get; set; }
    public float ManaRegenAmount { get; set; }
    public float ManaRegenTime { get; set; }
    public float ManaRegenSteal { get; set; }
    public float MaxArmor { get; set; }
    public int MaxDashCharge { get; set; }
    public float HealthPotionsPercentageMultiplier { get; set; }
    public List<SpellSO> AvailableSpells => availableSpells;

    public override void Initialize()
    {
        base.Initialize();
        MaxMana = defaultMana;
        MaxArmor = defaultArmor;
        ManaRegenAmount = defaultManaRegenAmount;
        ManaRegenTime = defaultManaRegenTime;
        ManaRegenSteal = defaultManaRegenSteal;
        MaxDashCharge = defaultDashCharge;
        HealthPotionsPercentageMultiplier = healthPotionsPercentageMultiplier;
    }
}
