using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : Stats, IMana, IArmor
{
    public override StatsSO Attributes => character.CommonValues.CharacterStats;

    private IEnumerator regenManaCoroutine;
    private YieldInstruction wft;

    public float Mana { get; private set; }
    public float Armor { get; private set; }

    protected override void Start()
    {
        base.Start();
        Mana = Attributes.MaxMana;
        Armor = Attributes.MaxArmor;

        // Starts regen Mana coroutine
        regenManaCoroutine = RegenManaCoroutine();
        wft = new WaitForSeconds(Attributes.ManaRegenTime);
        StartCoroutine(regenManaCoroutine);
    }

    /// <summary>
    /// Regens Mana Regent Amount every Mana Regen Time seconds.
    /// </summary>
    /// <returns>Wait for seconds.</returns>
    private IEnumerator RegenManaCoroutine()
    {
        while (true)
        {
            yield return wft;

            if (Mana + Attributes.ManaRegenAmount < Attributes.MaxMana)
            {
                Mana += Attributes.ManaRegenAmount;
            }
            else
            {
                Mana = Attributes.MaxMana;
            }
        }
    }

    /// <summary>
    /// Reduces armor + Health.
    /// </summary>
    /// <param name="damage">Damage to take.</param>
    /// <param name="element">Element of the damage.</param>
    public override void TakeDamage(float damage, ElementType element)
    {
        float damageToReceive = Mathf.Floor(damage * (ElementsDamage.CalculateDamage(element, Attributes.Element)));
        OnEventTakeDamage(damageToReceive);

        // Spawn damage text
        if (character.CommonValues.CharacterValues.Type != CharacterType.Player)
        {
            GameObject damageHitText =
                        DamageHitPoolCreator.Pool.InstantiateFromPool("DamageHit", transform.position, Quaternion.identity);
            if (damageHitText.TryGetComponent<DamageHitText>(out DamageHitText outDamageHitText))
            {
                outDamageHitText.UpdateShownDamage(damageToReceive, false);
            }
        }

        if (Armor - damageToReceive > 0)
        {
            Armor -= damageToReceive;
        }
        else // If Armor - damage is < 0
        {
            // Calculates the damage after damaging the Armor
            float restOfTheDamage = damageToReceive - Armor;

            // Damages Armor
            Armor = 0;

            if (Health - restOfTheDamage > 0)
            {
                Health -= restOfTheDamage;
            }
            else
            {
                if (damageOvertimeCoroutine != null) StopCoroutine(damageOvertimeCoroutine);

                if (character.CommonValues.CharacterValues.Type == CharacterType.Player)
                {
                    FindObjectOfType<CameraPostProcessOff>().PostProcessOn();
                    FindObjectOfType<PlayerCastSpell>().AttackKeyRelease();
                }

                Destroy(GetComponentInParent<SelectionBase>().gameObject);
            }
        }
    }

    /// <summary>
    /// Reduces armor + Health.
    /// </summary>
    /// <param name="damage">Damage to take.</param>
    /// <param name="criticalChance">Chance of critical hit.</param>
    /// <param name="element">Element of the damage.</param>
    public override void TakeDamage(float damage, float criticalChance, ElementType element)
    {
        // Critical check
        // If random.NextDouble is less than critical chance, it will do double damage
        bool criticalHit = random.NextDouble() < criticalChance;
        damage = criticalHit ? damage *= 2 : damage *= 1;

        // Claculates final damage
        float damageToReceive = Mathf.Floor(damage * (ElementsDamage.CalculateDamage(element, Attributes.Element)));
        OnEventTakeDamage(damageToReceive);

        // Spawn damage text
        if (character.CommonValues.CharacterValues.Type != CharacterType.Player)
        {
            GameObject damageHitText =
                        DamageHitPoolCreator.Pool.InstantiateFromPool("DamageHit", transform.position, Quaternion.identity);
            if (damageHitText.TryGetComponent<DamageHitText>(out DamageHitText outDamageHitText))
            {
                outDamageHitText.UpdateShownDamage(damageToReceive, criticalHit);
            }
        }

        if (Armor - damageToReceive > 0)
        {
            Armor -= damageToReceive;
        }
        else // If Armor - damage is < 0
        {
            // Calculates the damage after damaging the Armor
            float restOfTheDamage = damageToReceive - Armor;

            // Damages Armor
            Armor = 0;

            if (Health - restOfTheDamage > 0)
            {
                Health -= restOfTheDamage;
            }
            else
            {
                if (damageOvertimeCoroutine != null) StopCoroutine(damageOvertimeCoroutine);

                FindObjectOfType<CameraPostProcessOff>().PostProcessOn();
                FindObjectOfType<PlayerCastSpell>().AttackKeyRelease();

                Destroy(GetComponentInParent<SelectionBase>().gameObject);
            }
        }
    }

    /// <summary>
    /// Reduces mana.
    /// </summary>
    /// <param name="amount">Amount to reduce.</param>
    public void ReduceMana(float amount)
    {
        if (Mana - amount > 0) Mana -= amount;
        else Mana = 0;

        OnEventSpentMana(amount);
    }

    /// <summary>
    /// Heal Health, Mana or Armor.
    /// </summary>
    /// <param name="amountOfHeal">Amount of heal.</param>
    /// <param name="healType">Type of heal (Health, Mana or Armor).</param>
    public override void Heal(float amountOfHeal, StatsType healType)
    {
        switch (healType)
        {
            case StatsType.Health:
                if (Health + amountOfHeal < Attributes.MaxHealth)
                {
                    Health += amountOfHeal;
                }
                else
                {
                    Health = Attributes.MaxHealth;
                }
                break;

            case StatsType.Mana:
                if (Mana + amountOfHeal < Attributes.MaxMana)
                {
                    Mana += amountOfHeal;
                }
                else
                {
                    Mana = Attributes.MaxMana;
                }
                break;

            case StatsType.Armor:
                if (Armor + amountOfHeal < Attributes.MaxArmor)
                {
                    Armor += amountOfHeal;
                }
                else
                {
                    Armor = Attributes.MaxArmor;
                }
                break;

            default:
                throw new System.Exception("Invalid heal type on Stats.Heal method on " + gameObject.name + '.');
        }
    }

    /// <summary>
    /// Updates Stats.
    /// </summary>
    /// <param name="amountToIncrement">Amount to increment.</param>
    /// <param name="statsType">Type of stats to upgrade.</param>
    public void UpdateStats(float amountToIncrement, StatsType statsType)
    {
        switch (statsType)
        {
            case StatsType.Health:
                Attributes.MaxHealth += amountToIncrement;
                break;

            case StatsType.Mana:
                Attributes.MaxMana += amountToIncrement;
                break;

            case StatsType.ManaRegenAmount:
                Attributes.ManaRegenAmount += amountToIncrement;
                break;

            case StatsType.ManaRegenTime: // INCREMENTS time to regen, user must add amount to increment with negative sign

                if (Attributes.ManaRegenTime + amountToIncrement < 0.01f)
                    Attributes.ManaRegenTime = 0.01f;
                else
                    Attributes.ManaRegenTime += amountToIncrement;

                StopCoroutine(regenManaCoroutine);
                wft = new WaitForSeconds(Attributes.ManaRegenTime);
                StartCoroutine(regenManaCoroutine);
                break;

            case StatsType.ManaRegenSteal:
                Attributes.ManaRegenSteal += amountToIncrement;
                break;

            case StatsType.Armor:
                Attributes.MaxArmor += amountToIncrement;
                break;

            case StatsType.Damage:
                Attributes.BaseDamageMultiplier += amountToIncrement;
                break;

            case StatsType.CriticalChance:
                Attributes.CriticalChance += amountToIncrement;
                break;
        }
    }

    /// <summary>
    /// Sets the 3 base stats.
    /// </summary>
    /// <param name="health">Health value.</param>
    /// <param name="armor">Armor value.</param>
    /// <param name="mana">Mana value.</param>
    public void SetStats(float health, float armor, float mana)
    {
        Health = health;
        Armor = armor;
        Mana = mana;
    }

    // Regsitered on CheatsConsole.
    protected virtual void OnEventSpentMana(float manaToSpend) => EventSpentMana?.Invoke(manaToSpend);
    public Action<float> EventSpentMana;
}
