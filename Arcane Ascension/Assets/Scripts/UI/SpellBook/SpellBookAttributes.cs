using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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


    public void UpdateText()
    {
        maxHealth.text = playerStats.MaxHealth.ToString();
        damageResistance.text = (playerStats.DamageResistance * 100f) + " %";
        maxMana.text = playerStats.MaxMana.ToString();
        movementSpeed.text = 
            (playerValues.Speed * playerStats.MovementSpeedMultiplier).ToString();
        criticalBonusDamage.text = (playerStats.CriticalDamageModifier * 100f) + " %";
        criticalChance.text = (playerStats.CriticalChance * 100f) + " %";
        //ignisBonusDamage.text = playerStats.DamageElementMultiplier.ToString();
    }

    private void OnValidate()
    {
        UpdateText();
    }
}
