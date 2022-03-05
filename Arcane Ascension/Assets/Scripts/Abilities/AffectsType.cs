/// <summary>
/// Enum used by any ability to define which type of stats the ability affects OR
/// is affected by (This enum is basically used to highlight spellbook text
/// when a lign of text has a relation with the ability selected).
/// </summary>
public enum AffectsType
{
    None,
    MaxHealth,
    DamageResistance,
    MaxMana,
    ManaRegeneration,
    MovementSpeed,
    BaseDamage,
    LifeSteal,
    CriticalBonusDamage,
    CriticalChance,
    IgnisDamage,
    AquaDamage,
    TerraDamage,
    NaturaDamage,
    FulgurDamage,
    LuxDamage,
    UmbraDamage,
}
