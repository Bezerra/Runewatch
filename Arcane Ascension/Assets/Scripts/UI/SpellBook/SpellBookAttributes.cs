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

    private IDictionary<ElementType, TextMeshProUGUI> elementsText;

    private void Awake()
    {
        elementsText = new Dictionary<ElementType, TextMeshProUGUI>
        {
            {ElementType.Ignis, ignisBonusDamage },
            {ElementType.Aqua, aquaBonusDamage },
            {ElementType.Terra, terraBonusDamage },
            {ElementType.Natura, naturaBonusDamage },
            {ElementType.Fulgur, fulgurBonusDamage },
            {ElementType.Lux, luxBonusDamage },
            {ElementType.Umbra, umbraBonusDamage },
        };
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
            (playerStats.DamageElementMultiplier[ElementType.Ignis] * 100f).
            ToString("F1") + " %";

        aquaBonusDamage.text =
            (playerStats.DamageElementMultiplier[ElementType.Aqua] * 100f).
            ToString("F1") + " %";

        terraBonusDamage.text =
            (playerStats.DamageElementMultiplier[ElementType.Terra] * 100f).
            ToString("F1") + " %";

        naturaBonusDamage.text =
            (playerStats.DamageElementMultiplier[ElementType.Natura] * 100f).
            ToString("F1") + " %";

        fulgurBonusDamage.text =
            (playerStats.DamageElementMultiplier[ElementType.Fulgur] * 100f).
            ToString("F1") + " %";

        luxBonusDamage.text =
            (playerStats.DamageElementMultiplier[ElementType.Lux] * 100f).
            ToString("F1") + " %";

        umbraBonusDamage.text =
            (playerStats.DamageElementMultiplier[ElementType.Umbra] * 100f).
            ToString("F1") + " %";
    }
}
