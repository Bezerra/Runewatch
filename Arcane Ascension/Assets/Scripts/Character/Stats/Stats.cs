using System.Collections;
using UnityEngine;
using System.Text;
using System;

/// <summary>
/// Class responsible for controlling character stats.
/// </summary>
public class Stats : MonoBehaviour, IDamageable, IHealable, IHealth, IMana, IArmor
{
    private System.Random random;
    private Character character;
    public StatsSO Attributes => character.CommonValues.CharacterStats;

    public float Health { get; private set; }
    public float Mana { get; private set; }
    public float Armor { get; private set; }

    private IEnumerator damageOvertimeCoroutine;
    private IEnumerator regenManaCoroutine;
    private YieldInstruction wft;

    private void Awake()
    {
        random = new System.Random();
        character = GetComponent<Character>();
    }

    private void Start()
    {
        Health = Attributes.MaxHealth;
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
    /// Takes damage overtime.
    /// </summary>
    /// <param name="damage">Damage to take.</param>
    /// <param name="element">Element of the damage.</param>
    /// <param name="timeInterval">Takes damage every time interval. (ex: takes damage every 0.2 seconds, timeInterval = 0.2f).</param>
    /// <param name="maxTime">Takes damage for this time. (ex: this effect lasts for 5 seconds, maxTime = 5f).</param>
    /// <returns>Null.</returns>
    private IEnumerator DamageOvertime(float damage, ElementType element, float timeInterval, float maxTime)
    {
        YieldInstruction wfs = new WaitForSeconds(timeInterval);

        float timeStarted = Time.time;
        float currentTime = Time.time;
        while (currentTime < timeStarted + maxTime)
        {
            TakeDamage(damage, element);
            currentTime = Time.time;
            yield return wfs;
        }
    }

    /// <summary>
    /// Starts a coroutine to take damage overtime.
    /// </summary>
    /// <param name="damage">Damage to take.</param>
    /// <param name="element">Element of the damage.</param>
    /// <param name="timeInterval">Takes damage every time interval. (ex: takes damage every 0.2 seconds, timeInterval = 0.2f).</param>
    /// <param name="maxTime">Takes damage for this time. (ex: this effect lasts for 5 seconds, maxTime = 5f).</param>
    public void TakeDamageOvertime(float damage, ElementType element, float timeInterval, float maxTime)
    {
        if (damageOvertimeCoroutine != null) StopCoroutine(damageOvertimeCoroutine);
        damageOvertimeCoroutine = DamageOvertime(damage, element, timeInterval, maxTime);
        StartCoroutine(damageOvertimeCoroutine);
    }

    /// <summary>
    /// Reduces armor + Health.
    /// </summary>
    /// <param name="damage">Damage to take.</param>
    /// <param name="element">Element of the damage.</param>
    public void TakeDamage(float damage, ElementType element)
    {
        float damageToReceive = Mathf.Floor(damage * (ElementsDamage.CalculateDamage(element, Attributes.Element)));

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
                // die
                print("Character died");
                if (damageOvertimeCoroutine != null) StopCoroutine(damageOvertimeCoroutine);
                Destroy(gameObject);
            }
        }

        // Temp
        if (Health > 0 || Armor > 0) Debug.Log("Health: " + Health + " || Armor: " + Armor);
    }

    /// <summary>
    /// Reduces armor + Health.
    /// </summary>
    /// <param name="damage">Damage to take.</param>
    /// <param name="criticalChance">Chance of critical hit.</param>
    /// <param name="element">Element of the damage.</param>
    public void TakeDamage(float damage, float criticalChance, ElementType element)
    {
        Debug.Log(damage);
        double ah = random.NextDouble();
        // Critical check
        // If random.NextDouble is less than critical chance, it will do double damage
        damage = ah < criticalChance ? damage *= 2 : damage *= 1;

        Debug.Log("double " + ah);
        Debug.Log("critical chance " + criticalChance);
        Debug.Log(damage);

        float damageToReceive = Mathf.Floor(damage * (ElementsDamage.CalculateDamage(element, Attributes.Element)));

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
                // die
                print("Character died");
                if (damageOvertimeCoroutine != null) StopCoroutine(damageOvertimeCoroutine);
                Destroy(gameObject);
            }
        }

        // Temp
        if (Health > 0 || Armor > 0) Debug.Log("Health: " + Health + " || Armor: " + Armor);
    }

    /// <summary>
    /// Reduces mana.
    /// </summary>
    /// <param name="amount">Amount to reduce.</param>
    public void ReduceMana(float amount)
    {
        if (Mana - amount > 0) Mana -= amount;
        else Mana = 0;
    }

    /// <summary>
    /// Heal Health, Mana or Armor.
    /// </summary>
    /// <param name="amountOfHeal">Amount of heal.</param>
    /// <param name="healType">Type of heal (Health, Mana or Armor).</param>
    public void Heal(float amountOfHeal, StatsType healType)
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
        switch(statsType)
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

            case StatsType.ManaRegenTime: // INCREMENTS time to regen, user must add amoun to increment with negative sign

                if (Attributes.ManaRegenTime + amountToIncrement < 0.01f)
                    Attributes.ManaRegenTime = 0.01f;
                else
                    Attributes.ManaRegenTime += amountToIncrement;

                wft = new WaitForSeconds(Attributes.ManaRegenTime);
                break;

            case StatsType.Armor:
                Attributes.MaxArmor += amountToIncrement;
                break;

            case StatsType.Damage:
                Attributes.BaseDamageMultiplier += amountToIncrement;
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

    public override string ToString()
    {
        StringBuilder str = new StringBuilder();
        str.Append($"Health: {Health.ToString():2f} |" + $"Armor: {Armor.ToString():2f} |" +
            $"Damage: {Attributes.BaseDamageMultiplier.ToString():2f} |" + $"Character Element: {Attributes.Element}");
        return str.ToString();
    }
}
