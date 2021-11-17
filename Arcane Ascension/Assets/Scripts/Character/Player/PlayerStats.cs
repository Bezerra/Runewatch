using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : Stats, IMana, IArmor, ISaveable
{
    // Components
    private CharacterSaveDataController stpData;

    public PlayerStatsSO PlayerAttributes => character.CommonValues.CharacterStats as PlayerStatsSO;

    // Coroutines
    private IEnumerator loseManaCoroutine;
    private IEnumerator regenManaCoroutine;
    private YieldInstruction wft;

    // Components
    private PlayerCastSpell playerCastSpell;

    /// <summary>
    /// Property with currenty passives possessed by the player character.
    /// </summary>
    public IList<IRunPassive> CurrentPassives;

    /// <summary>
    /// Current mana of the character.
    /// </summary>
    public float Mana { get; private set; }

    /// <summary>
    /// Current armor of the character.
    /// </summary>
    public float Armor { get; private set; }

    /// <summary>
    /// Property with das charges.
    /// </summary>
    public int DashCharge { get; set; }

    /// <summary>
    /// Property to keep track of character max mana.
    /// </summary>
    public float MaxMana => PlayerAttributes.MaxMana;

    protected override void Awake()
    {
        base.Awake();
        CurrentPassives = new List<IRunPassive>();
        playerCastSpell = GetComponent<PlayerCastSpell>();
        stpData = FindObjectOfType<CharacterSaveDataController>();
    }

    protected override void Start()
    {
        UpdateStats(stpData.SaveData.Vitality, StatsType.Health);
        UpdateStats(stpData.SaveData.Insight, StatsType.Mana);
        UpdateStats(stpData.SaveData.Agility, StatsType.MovementSpeedMultiplier);
        UpdateStats(stpData.SaveData.Meditation, StatsType.ManaRegenAmount);

        base.Start();
        Mana = PlayerAttributes.MaxMana;
        Armor = PlayerAttributes.MaxArmor;

        // Starts regen Mana coroutine
        regenManaCoroutine = RegenManaCoroutine();
        wft = new WaitForSeconds(PlayerAttributes.ManaRegenTime);
        StartCoroutine(regenManaCoroutine);
    }

    private void OnEnable()
    {
        playerCastSpell.EventSpendMana += ReduceMana;
        playerCastSpell.EventSpendManaContinuous += StartLoseManaCoroutine;
        playerCastSpell.EventStopSpendManaContinuous += StopLoseManaCoroutine;
    }

    private void OnDisable()
    {
        playerCastSpell.EventSpendMana -= ReduceMana;
        playerCastSpell.EventSpendManaContinuous -= StartLoseManaCoroutine;
        playerCastSpell.EventStopSpendManaContinuous -= StopLoseManaCoroutine;
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

            if (Mana + PlayerAttributes.ManaRegenAmount < PlayerAttributes.MaxMana)
            {
                Mana += PlayerAttributes.ManaRegenAmount;
            }
            else
            {
                Mana = PlayerAttributes.MaxMana;
            }
        }
    }

    /// <summary>
    /// Starts a coroutine to burn mana.
    /// </summary>
    /// <param name="amountToLose">Amount to lose.</param>
    /// <param name="timeToWait">Time to wait before each loss.</param>
    private void StartLoseManaCoroutine(float amountToLose, float timeToWait)
    {
        if (loseManaCoroutine != null) StopCoroutine(loseManaCoroutine);
        loseManaCoroutine = LoseManaCoroutine(amountToLose, timeToWait);
        StartCoroutine(loseManaCoroutine);
    }

    /// <summary>
    /// Stops coroutine to burn mana.
    /// </summary>
    /// <param name="amountToLose">Amount to lose.</param>
    /// <param name="timeToWait">Time to wait before each loss.</param>
    private void StopLoseManaCoroutine()
    {
        if (loseManaCoroutine != null) StopCoroutine(loseManaCoroutine);
        loseManaCoroutine = null;
    }


    /// <summary>
    /// Loses Mana Amount every X seconds.
    /// </summary>
    /// <param name="amountToLose">Amount to lose.</param>
    /// <param name="timeToWait">Time to wait before each loss.</param>
    /// <returns>Wait for seconds.</returns>
    private IEnumerator LoseManaCoroutine(float amountToLose, float timeToWait)
    {
        YieldInstruction wfs = new WaitForSeconds(timeToWait);

        while (true)
        {
            yield return wfs;

            if (Mana - amountToLose > 0)
            {
                ReduceMana(amountToLose);
            }
            else
            {
                break;
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
    /// Reduces armor + Health.
    /// </summary>
    /// <param name="damage">Damage to take.</param>
    /// <param name="element">Element of the damage.</param>
    public override void TakeDamage(float damage, ElementType element)
    {
        float damageToReceive = Mathf.Floor(damage * (ElementsDamage.CalculateDamage(element, PlayerAttributes.Element)));
        OnEventTakeDamage(damageToReceive);

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

                FindObjectOfType<PlayerCastSpell>().AttackKeyRelease();

                // This will do some other stuff later
                Destroy(GetComponentInParent<SelectionBase>().gameObject);
            }
        }
    }

    /// <summary>
    /// Reduces armor + health.
    /// </summary>
    /// <param name="damage">Damage to take.</param>
    /// <param name="criticalChance">Chance of critical hit.</param>
    /// <param name="criticalDamageModifier">Damage modifier on critical hits.</param>
    /// <param name="element">Element of the damage.</param>
    public override void TakeDamage(float damage, float criticalChance, float criticalDamageModifier, ElementType element)
    {
        // Critical check
        // If random.NextDouble is less than critical chance, it will do double damage
        bool criticalHit = random.NextDouble() < criticalChance;
        damage = criticalHit ? damage *= 2 * criticalDamageModifier : damage *= 1;

        // Claculates final damage
        float damageToReceive = Mathf.Floor(damage * (ElementsDamage.CalculateDamage(element, PlayerAttributes.Element)));
        OnEventTakeDamage(damageToReceive);

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

                FindObjectOfType<PlayerCastSpell>().AttackKeyRelease();


                // This will do some other stuff later
                Destroy(GetComponentInParent<SelectionBase>().gameObject);
            }
        }
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
                if (Health + amountOfHeal < PlayerAttributes.MaxHealth)
                {
                    Health += amountOfHeal;
                }
                else
                {
                    Health = PlayerAttributes.MaxHealth;
                }
                break;

            case StatsType.Mana:
                if (Mana + amountOfHeal < PlayerAttributes.MaxMana)
                {
                    Mana += amountOfHeal;
                }
                else
                {
                    Mana = PlayerAttributes.MaxMana;
                }
                break;

            case StatsType.Armor:
                if (Armor + amountOfHeal < PlayerAttributes.MaxArmor)
                {
                    Armor += amountOfHeal;
                }
                else
                {
                    Armor = PlayerAttributes.MaxArmor;
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
        amountToIncrement *= 0.01f;
        switch (statsType)
        {
            case StatsType.Health:
                PlayerAttributes.MaxHealth += PlayerAttributes.MaxHealth * amountToIncrement;
                break;

            case StatsType.Mana:
                PlayerAttributes.MaxMana += PlayerAttributes.MaxMana * amountToIncrement;
                break;

            case StatsType.MovementSpeedMultiplier:
                PlayerAttributes.MovementSpeedMultiplier += amountToIncrement;
                FindObjectOfType<PlayerMovement>().UpdateSpeed();
                break;

            case StatsType.ManaRegenAmount:
                PlayerAttributes.ManaRegenAmount += amountToIncrement;
                break;

            case StatsType.ManaRegenTime: // INCREMENTS time to regen, user must add amount to increment with negative sign

                if (PlayerAttributes.ManaRegenTime + amountToIncrement < 0.01f)
                    PlayerAttributes.ManaRegenTime = 0.01f;
                else
                    PlayerAttributes.ManaRegenTime += amountToIncrement;

                StopCoroutine(regenManaCoroutine);
                wft = new WaitForSeconds(PlayerAttributes.ManaRegenTime);
                StartCoroutine(regenManaCoroutine);
                break;

            case StatsType.ManaRegenSteal:
                PlayerAttributes.ManaRegenSteal += amountToIncrement;
                break;

            case StatsType.Armor:
                PlayerAttributes.MaxArmor += amountToIncrement;
                break;

            case StatsType.Damage:
                PlayerAttributes.BaseDamageMultiplier += amountToIncrement;
                break;

            case StatsType.CriticalChance:
                PlayerAttributes.CriticalChance += amountToIncrement;
                break;

            case StatsType.CriticalDamageMultiplier:
                PlayerAttributes.CriticalDamageModifier += amountToIncrement;
                break;

            case StatsType.IgnisDamage:
                PlayerAttributes.DamageElementMultiplier[ElementType.Fire] += amountToIncrement;
                break;

            case StatsType.FulgurDamage:
                PlayerAttributes.DamageElementMultiplier[ElementType.Electric] += amountToIncrement;
                break;

            case StatsType.AquaDamage:
                PlayerAttributes.DamageElementMultiplier[ElementType.Water] += amountToIncrement;
                break;

            case StatsType.TerraDamage:
                PlayerAttributes.DamageElementMultiplier[ElementType.Earth] += amountToIncrement;
                break;

            case StatsType.NaturaDamage:
                PlayerAttributes.DamageElementMultiplier[ElementType.Nature] += amountToIncrement;
                break;

            case StatsType.LuxDamage:
                PlayerAttributes.DamageElementMultiplier[ElementType.Light] += amountToIncrement;
                break;

            case StatsType.UmbraDamage:
                PlayerAttributes.DamageElementMultiplier[ElementType.Dark] += amountToIncrement;
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

    /// <summary>
    /// Saves player stats.
    /// </summary>
    /// <param name="saveData">Saved data class.</param>
    /// <returns>Null.</returns>
    public void SaveCurrentData(RunSaveData saveData)
    {
        if (this != null) // Do not remove <
        {
            saveData.PlayerSavedData.Health = Health;
            saveData.PlayerSavedData.Armor = Armor;
            saveData.PlayerSavedData.Mana = Mana;
            saveData.PlayerSavedData.DashCharge = DashCharge;

            // Passives
            saveData.PlayerSavedData.CurrentPassives = new byte[CurrentPassives.Count];
            for (int i = 0; i < CurrentPassives.Count; i++)
            {
                saveData.PlayerSavedData.CurrentPassives[i] = CurrentPassives[i].ID;
            }
        }
    }

    /// <summary>
    /// Loads player stats.
    /// </summary>
    /// <param name="saveData">Saved data class.</param>
    /// <returns>Null.</returns>
    public IEnumerator LoadData(RunSaveData saveData)
    {
        yield return new WaitForFixedUpdate();

        // Stats
        if (this != null) // Do not remove <
        {
            // Passives
            AllRunPassives allPassives = FindObjectOfType<AllRunPassives>();
            CurrentPassives = new List<IRunPassive>();
            for (int i = 0; i < saveData.PlayerSavedData.CurrentPassives.Length; i++)
            {
                for (int j = 0; j < allPassives.PassiveList.Count; j++)
                {
                    if (saveData.PlayerSavedData.CurrentPassives[i] == allPassives.PassiveList[j].ID)
                    {
                        CurrentPassives.Add(allPassives.PassiveList[j]);
                    }
                }
            }
            foreach (IRunPassive passive in CurrentPassives)
            {
                passive.Execute(this);
            }
 
            // Loads stats
            SetStats(saveData.PlayerSavedData.Health, saveData.PlayerSavedData.Armor, saveData.PlayerSavedData.Mana);
            DashCharge = saveData.PlayerSavedData.DashCharge;
        }
    }

    // Regsitered on CheatsConsole.
    protected virtual void OnEventSpentMana(float manaToSpend) => EventSpentMana?.Invoke(manaToSpend);
    public Action<float> EventSpentMana;
}
