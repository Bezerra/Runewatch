using System;
using System.Collections;
using System.Text;
using UnityEngine;

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
    public void TakeDamage(float damage, float criticalChance, ElementType element)
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

    public override string ToString()
    {
        StringBuilder str = new StringBuilder();
        str.Append($"Health: {Health.ToString():2f} |" + $"Armor: {Armor.ToString():2f} |" +
            $"Damage: {Attributes.BaseDamageMultiplier.ToString():2f} |" + $"Character Element: {Attributes.Element}");
        return str.ToString();
    }

    // Events
    // Registered on CheatsConsole, EnemyScript.
    protected virtual void OnEventTakeDamage(float damageToReceive) => EventTakeDamage?.Invoke(damageToReceive);
    public Action<float> EventTakeDamage;
    // Regsitered on CheatsConsole.
    protected virtual void OnEventSpentMana(float manaToSpend) => EventSpentMana?.Invoke(manaToSpend);
    public Action<float> EventSpentMana;
}
