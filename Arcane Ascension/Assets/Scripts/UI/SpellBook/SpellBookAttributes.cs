using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Class responsible for updating spellbook attributes text fields.
/// </summary>
public class SpellBookAttributes : MonoBehaviour
{
    [SerializeField] private PlayerStatsSO playerStats;
    [SerializeField] private PlayerValuesSO playerValues;

    [Header("Texts to update")]
    [SerializeField] private TextMeshProUGUI maxHealth;
    [SerializeField] private TextMeshProUGUI damageResistance;
    [SerializeField] private TextMeshProUGUI maxMana;
    [SerializeField] private TextMeshProUGUI manaRegeneration;
    [SerializeField] private TextMeshProUGUI movementSpeed;
    [SerializeField] private TextMeshProUGUI baseDamage;
    [SerializeField] private TextMeshProUGUI lifeSteal;
    [SerializeField] private TextMeshProUGUI criticalBonusDamage;
    [SerializeField] private TextMeshProUGUI criticalChance;
    [SerializeField] private TextMeshProUGUI ignisBonusDamage;
    [SerializeField] private TextMeshProUGUI aquaBonusDamage;
    [SerializeField] private TextMeshProUGUI terraBonusDamage;
    [SerializeField] private TextMeshProUGUI naturaBonusDamage;
    [SerializeField] private TextMeshProUGUI fulgurBonusDamage;
    [SerializeField] private TextMeshProUGUI luxBonusDamage;
    [SerializeField] private TextMeshProUGUI umbraBonusDamage;

    private IDictionary<AffectsType, TextMeshProUGUI> affectedText;

    [SerializeField] private Color normalTextColor;
    [SerializeField] private Color affectedTextColor;
    [SerializeField] private Color unnafectedTextColor;

    private void Awake()
    {
        affectedText = new Dictionary<AffectsType, TextMeshProUGUI>
        {
            {AffectsType.MaxHealth, maxHealth },
            {AffectsType.DamageResistance, damageResistance },
            {AffectsType.MaxMana, maxMana },
            {AffectsType.ManaRegeneration, manaRegeneration},
            {AffectsType.MovementSpeed, movementSpeed },
            {AffectsType.BaseDamage, baseDamage },
            {AffectsType.LifeSteal, lifeSteal },
            {AffectsType.CriticalBonusDamage, criticalBonusDamage },
            {AffectsType.CriticalChance, criticalChance },
            {AffectsType.IgnisDamage, ignisBonusDamage },
            {AffectsType.AquaDamage, aquaBonusDamage },
            {AffectsType.TerraDamage, terraBonusDamage },
            {AffectsType.NaturaDamage, naturaBonusDamage },
            {AffectsType.FulgurDamage, fulgurBonusDamage },
            {AffectsType.LuxDamage, luxBonusDamage },
            {AffectsType.UmbraDamage, umbraBonusDamage },
        };
    }

    /// <summary>
    /// Highlights text that has a relation with the selected ability. 
    /// </summary>
    /// <param name="abilityAffectsType">Stats that affects or is affected by this ability,</param>
    public void UpdateSelectedCardStatsColors(AffectsType abilityAffectsType)
    {
        foreach(KeyValuePair<AffectsType,TextMeshProUGUI> dicElement in affectedText)
        {
            if (dicElement.Key == abilityAffectsType) dicElement.Value.color = affectedTextColor;
            else dicElement.Value.color = unnafectedTextColor;
        }
    }

    public void UpdateText()
    {
        maxHealth.text = playerStats.MaxHealth.ToString("F0");

        damageResistance.text = 
            ((1 - playerStats.DamageResistance) * 100f).ToString("F1") + " %";

        maxMana.text = playerStats.MaxMana.ToString("F0");

        // MANA REGEN TIME IS SET TO 0.1F, THIS CALCULATION IS BASED ON THAT VALUE
        manaRegeneration.text = (playerStats.ManaRegenAmount * 10f).
            ToString("F1") + " / sec";

        movementSpeed.text = 
            (playerValues.Speed * playerStats.MovementSpeedMultiplier).
            ToString("F1");

        baseDamage.text = (playerStats.BaseDamageMultiplier * 100f).
            ToString("F1") + " %";

        lifeSteal.text = playerStats.LifeSteal.
            ToString("F1") + " %";

        criticalBonusDamage.text = 
            (playerStats.CriticalDamageModifier * 100f).ToString("F1") + " %";

        criticalChance.text = 
            (playerStats.CriticalChance * 100f).ToString("F1") + " %";

        ignisBonusDamage.text =
            (playerStats.DamageElementMultiplier[ElementType.Ignis] * 100f - 100).
            ToString("F1") + " %";

        aquaBonusDamage.text =
            (playerStats.DamageElementMultiplier[ElementType.Aqua] * 100f - 100).
            ToString("F1") + " %";

        terraBonusDamage.text =
            (playerStats.DamageElementMultiplier[ElementType.Terra] * 100f - 100).
            ToString("F1") + " %";

        naturaBonusDamage.text =
            (playerStats.DamageElementMultiplier[ElementType.Natura] * 100f - 100).
            ToString("F1") + " %";

        fulgurBonusDamage.text =
            (playerStats.DamageElementMultiplier[ElementType.Fulgur] * 100f - 100).
            ToString("F1") + " %";

        luxBonusDamage.text =
            (playerStats.DamageElementMultiplier[ElementType.Lux] * 100f - 100).
            ToString("F1") + " %";

        umbraBonusDamage.text =
            (playerStats.DamageElementMultiplier[ElementType.Umbra] * 100f - 100).
            ToString("F1") + " %";
    }
}
