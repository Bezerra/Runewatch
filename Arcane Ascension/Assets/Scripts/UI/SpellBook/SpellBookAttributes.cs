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
    [SerializeField] private TextMeshProUGUI[] maxHealth;
    [SerializeField] private TextMeshProUGUI[] damageResistance;
    [SerializeField] private TextMeshProUGUI[] maxMana;
    [SerializeField] private TextMeshProUGUI[] manaRegeneration;
    [SerializeField] private TextMeshProUGUI[] movementSpeed;
    [SerializeField] private TextMeshProUGUI[] baseDamage;
    [SerializeField] private TextMeshProUGUI[] lifeSteal;
    [SerializeField] private TextMeshProUGUI[] criticalBonusDamage;
    [SerializeField] private TextMeshProUGUI[] criticalChance;
    [SerializeField] private TextMeshProUGUI[] ignisBonusDamage;
    [SerializeField] private TextMeshProUGUI[] aquaBonusDamage;
    [SerializeField] private TextMeshProUGUI[] terraBonusDamage;
    [SerializeField] private TextMeshProUGUI[] naturaBonusDamage;
    [SerializeField] private TextMeshProUGUI[] fulgurBonusDamage;
    [SerializeField] private TextMeshProUGUI[] luxBonusDamage;
    [SerializeField] private TextMeshProUGUI[] umbraBonusDamage;

    private IDictionary<AffectsType, TextMeshProUGUI> affectedText;
    private IDictionary<TextMeshProUGUI, TextMeshProUGUI> parentText;

    [SerializeField] private Color normalTextColor;
    [SerializeField] private Color affectedTextColor;
    [SerializeField] private Color unnafectedTextColor;

    private void Awake()
    {
        affectedText = new Dictionary<AffectsType, TextMeshProUGUI>
        {
            {AffectsType.MaxHealth, maxHealth[0] },
            {AffectsType.DamageResistance, damageResistance[0] },
            {AffectsType.MaxMana, maxMana[0] },
            {AffectsType.ManaRegeneration, manaRegeneration[0] },
            {AffectsType.MovementSpeed, movementSpeed[0] },
            {AffectsType.BaseDamage, baseDamage[0] },
            {AffectsType.LifeSteal, lifeSteal[0] },
            {AffectsType.CriticalBonusDamage, criticalBonusDamage[0] },
            {AffectsType.CriticalChance, criticalChance[0] },
            {AffectsType.IgnisDamage, ignisBonusDamage[0] },
            {AffectsType.AquaDamage, aquaBonusDamage[0] },
            {AffectsType.TerraDamage, terraBonusDamage[0] },
            {AffectsType.NaturaDamage, naturaBonusDamage[0] },
            {AffectsType.FulgurDamage, fulgurBonusDamage[0] },
            {AffectsType.LuxDamage, luxBonusDamage[0] },
            {AffectsType.UmbraDamage, umbraBonusDamage[0] },
        };

        parentText = new Dictionary<TextMeshProUGUI, TextMeshProUGUI>
        {
            { maxHealth[0], maxHealth[1] },
            { damageResistance[0], damageResistance[1] },
            { maxMana[0], maxMana[1] },
            { manaRegeneration[0],manaRegeneration [1]},
            { movementSpeed[0], movementSpeed[1] },
            { baseDamage[0], baseDamage[1] },
            { lifeSteal[0], lifeSteal[1] },
            { criticalBonusDamage[0], criticalBonusDamage[1] },
            { criticalChance[0], criticalChance[1] },
            { ignisBonusDamage[0], ignisBonusDamage[1] },
            { aquaBonusDamage[0], aquaBonusDamage[1] },
            { terraBonusDamage[0], terraBonusDamage[1] },
            { naturaBonusDamage[0], naturaBonusDamage[1] },
            { fulgurBonusDamage[0], fulgurBonusDamage[1] },
            { luxBonusDamage[0], luxBonusDamage[1] },
            { umbraBonusDamage[0], umbraBonusDamage[1] },
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
            if (dicElement.Key == abilityAffectsType)
            {
                dicElement.Value.color = affectedTextColor;
                parentText[dicElement.Value].color = affectedTextColor;
            }
            else
            {
                dicElement.Value.color = unnafectedTextColor;
                parentText[dicElement.Value].color = unnafectedTextColor;
            }
        }
    }

    /// <summary>
    /// Resets text color.
    /// </summary>
    public void ResetTextColor()
    {
        foreach (KeyValuePair<AffectsType, TextMeshProUGUI> dicElement in affectedText)
        {
            dicElement.Value.color = normalTextColor;
            parentText[dicElement.Value].color = normalTextColor;
        }
    }

    public void UpdateText()
    {
        maxHealth[0].text = playerStats.MaxHealth.ToString("F0");

        damageResistance[0].text = 
            ((1 - playerStats.DamageResistance) * 100f).ToString("F1") + " %";

        maxMana[0].text = playerStats.MaxMana.ToString("F0");

        // MANA REGEN TIME IS SET TO 0.1F, THIS CALCULATION IS BASED ON THAT VALUE
        manaRegeneration[0].text = (playerStats.ManaRegenAmount * 10f).
            ToString("F1") + " / sec";

        movementSpeed[0].text = 
            (playerValues.Speed * playerStats.MovementSpeedMultiplier).
            ToString("F1");

        baseDamage[0].text = (playerStats.BaseDamageMultiplier * 100f).
            ToString("F1") + " %";

        lifeSteal[0].text = playerStats.LifeSteal.
            ToString("F1") + " %";

        criticalBonusDamage[0].text = 
            (playerStats.CriticalDamageModifier * 100f).ToString("F1") + " %";

        criticalChance[0].text = 
            (playerStats.CriticalChance * 100f).ToString("F1") + " %";

        ignisBonusDamage[0].text =
            (playerStats.DamageElementMultiplier[ElementType.Ignis] * 100f - 100).
            ToString("F1") + " %";

        aquaBonusDamage[0].text =
            (playerStats.DamageElementMultiplier[ElementType.Aqua] * 100f - 100).
            ToString("F1") + " %";

        terraBonusDamage[0].text =
            (playerStats.DamageElementMultiplier[ElementType.Terra] * 100f - 100).
            ToString("F1") + " %";

        naturaBonusDamage[0].text =
            (playerStats.DamageElementMultiplier[ElementType.Natura] * 100f - 100).
            ToString("F1") + " %";

        fulgurBonusDamage[0].text =
            (playerStats.DamageElementMultiplier[ElementType.Fulgur] * 100f - 100).
            ToString("F1") + " %";

        luxBonusDamage[0].text =
            (playerStats.DamageElementMultiplier[ElementType.Lux] * 100f - 100).
            ToString("F1") + " %";

        umbraBonusDamage[0].text =
            (playerStats.DamageElementMultiplier[ElementType.Umbra] * 100f - 100).
            ToString("F1") + " %";
    }
}
