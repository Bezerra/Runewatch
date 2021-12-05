using System;
using System.Collections;
using System.Text;
using UnityEngine;

/// <summary>
/// Class responsible for controlling character stats.
/// </summary>
public abstract class Stats : MonoBehaviour, IDamageable, IHealable, IHealth
{
    protected System.Random random;
    protected Character character;

    public StatsSO CommonAttributes => character.CommonValues.CharacterStats;

    /// <summary>
    /// Property that keeps trace of character's current health.
    /// </summary>
    public float Health { get; set; }

    /// <summary>
    /// Property only used to know max health with IHealable interface.
    /// </summary>
    public float MaxHealth => CommonAttributes.MaxHealth;

    public ExtendedDictionary<StatusEffectType, IStatusEffectInformation> StatusEffectList 
        { get; private set; }

    protected IEnumerator damageOvertimeCoroutine;

    protected virtual void Awake()
    {
        random = new System.Random();
        character = GetComponent<Character>();
        StatusEffectList = new ExtendedDictionary<StatusEffectType, IStatusEffectInformation>();
    }

    protected virtual void Start()
    {
        Heal(CommonAttributes.MaxHealth, StatsType.Health);
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
            TakeDamage(damage, element, Vector3.zero);
            currentTime = Time.time;
            yield return wfs;
        }
    }

    /// <summary>
    /// Starts a coroutine to take damage overtime.
    /// </summary>
    /// <param name="damage">Damage to take.</param>
    /// <param name="element">Element of the damage.</param>
    /// <param name="timeInterval">Takes damage every time interval. 
    /// (ex: takes damage every 0.2 seconds, timeInterval = 0.2f).</param>
    /// <param name="maxTime">Takes damage for this time. 
    /// (ex: this effect lasts for 5 seconds, maxTime = 5f).</param>
    public void TakeDamageOvertime(float damage, ElementType element, float timeInterval, float maxTime)
    {
        if (damageOvertimeCoroutine != null) StopCoroutine(damageOvertimeCoroutine);
        damageOvertimeCoroutine = DamageOvertime(damage, element, timeInterval, maxTime);
        StartCoroutine(damageOvertimeCoroutine);
    }

    /// <summary>
    /// Reduces Health.
    /// </summary
    /// <param name="damage">Damage to take.</param>
    /// <param name="element">Element of the damage.</param>
    /// <param name="damagePosition">Position of the damage.</param> 
    public abstract void TakeDamage(float damage, 
        ElementType element, Vector3 damagePosition);

    /// <summary>
    /// Reduces Health.
    /// </summary>
    /// <param name="damage">Damage to take.</param>
    /// <param name="criticalChance">Chance of critical hit.</param>
    /// <param name="criticalDamageModifier">Damage modifier on critical hits.</param>
    /// <param name="element">Element of the damage.</param>
    /// <param name="damagePosition">Position of the damage.</param>
    public abstract void TakeDamage(
        float damage, float criticalChance, float criticalDamageModifier, 
        ElementType element, Vector3 damagePosition);

    /// <summary>
    /// Heal Health.
    /// </summary>
    /// <param name="amountOfHeal">Amount of heal.</param>
    /// <param name="healType">Type of heal (Health, Mana or Armor).</param>
    public virtual void Heal(float amountOfHeal, StatsType healType)
    {
        switch (healType)
        {
            case StatsType.Health:
                if (Health + amountOfHeal < CommonAttributes.MaxHealth)
                {
                    Health += amountOfHeal;
                }
                else
                {
                    Health = CommonAttributes.MaxHealth;
                }
                break;

            default:
                throw new System.Exception("Invalid heal type on Stats.Heal method on " + gameObject.name + '.');
        }
    }

    /// <summary>
    /// Updates speed variable.
    /// </summary>
    public virtual void UpdateSpeed() =>
        OnSpeedUpdate(character.CommonValues.CharacterValues.Speed *
        CommonAttributes.MovementSpeedMultiplier *
        CommonAttributes.MovementStatusEffectMultiplier);

    public override string ToString()
    {
        StringBuilder str = new StringBuilder();
        str.Append($"Health: {Health.ToString():2f} |" +
            $"Damage: {CommonAttributes.BaseDamageMultiplier.ToString():2f} |" + 
            $"Character Element: {CommonAttributes.Element}");
        return str.ToString();
    }

    // Events
    // Subscribed on CheatsConsole, EnemyScript.
    protected virtual void OnEventTakeDamage(float damageToReceive) => EventTakeDamageNumber?.Invoke(damageToReceive);
    public Action<float> EventTakeDamageNumber;

    // Subscribed on playerDamageReceiverUI
    protected virtual void OnEventTakeDamage(Vector3 damagePosition) => 
        EventTakeDamagePosition?.Invoke(damagePosition);
    public Action<Vector3> EventTakeDamagePosition;

    protected virtual void OnEventTakeDamage() => EventTakeDamage?.Invoke();
    public Action EventTakeDamage;

    // Subscribed on player ui
    protected virtual void OnEventHealthUpdate() => EventHealthUpdate?.Invoke();
    public Action EventHealthUpdate;

    // Subscribed on playerUi
    protected virtual void OnEventDeath(Stats stats) => EventDeath?.Invoke(stats);
    public Action<Stats> EventDeath;

    // Subscribed on classes that use Speed
    protected virtual void OnSpeedUpdate(float speed) => EventSpeedUpdate?.Invoke(speed);
    public Action<float> EventSpeedUpdate;

    // Subscribed on HUD classes
    protected virtual void OnStatusEffectListUpdated(StatusEffectType type,
        IStatusEffectInformation information) => EventStatusEffectListUpdated?.Invoke(type, information);
    public Action<StatusEffectType, IStatusEffectInformation> EventStatusEffectListUpdated;
}
