using System;
using System.Collections;
using System.Text;
using UnityEngine;

/// <summary>
/// Class responsible for controlling character stats.
/// </summary>
public class Stats : MonoBehaviour, IDamageable, IHealable, IHealth
{
    protected System.Random random;
    protected Character character;

    public StatsSO CommonAttributes => character.CommonValues.CharacterStats;

    /// <summary>
    /// Property that keeps trace of character's current health.
    /// </summary>
    public float Health { get; protected set; }

    /// <summary>
    /// Property only used to know max health with IHealable interface.
    /// </summary>
    public float MaxHealth => CommonAttributes.MaxHealth;

    protected IEnumerator damageOvertimeCoroutine;

    protected virtual void Awake()
    {
        random = new System.Random();
        character = GetComponent<Character>();
    }

    protected virtual void Start()
    {
        Health = CommonAttributes.MaxHealth;
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
    public virtual void TakeDamage(float damage, ElementType element)
    {
        float damageToReceive = Mathf.Floor(damage * (ElementsDamage.CalculateDamage(element, CommonAttributes.Element)));
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

        if (Health - damageToReceive > 0)
        {
            Health -= damageToReceive;
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

    /// <summary>
    /// Reduces Health.
    /// </summary>
    /// <param name="damage">Damage to take.</param>
    /// <param name="criticalChance">Chance of critical hit.</param>
    /// <param name="criticalDamageModifier">Damage modifier on critical hits.</param>
    /// <param name="element">Element of the damage.</param>
    public virtual void TakeDamage(float damage, float criticalChance, float criticalDamageModifier, ElementType element)
    {
        // Critical check
        // If random.NextDouble is less than critical chance, it will do double damage
        bool criticalHit = random.NextDouble() < criticalChance;
        damage = criticalHit ? damage *= 2 * criticalDamageModifier: damage *= 1;

        // Claculates final damage
        float damageToReceive = Mathf.Floor(damage * (ElementsDamage.CalculateDamage(element, CommonAttributes.Element)));
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

        if (Health - damageToReceive > 0)
        {
            Health -= damageToReceive;
        }
        else
        {
            if (damageOvertimeCoroutine != null) StopCoroutine(damageOvertimeCoroutine);
            Destroy(GetComponentInParent<SelectionBase>().gameObject);
        }
    }

    /// <summary>
    /// Heal Health, Mana or Armor.
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

    public override string ToString()
    {
        StringBuilder str = new StringBuilder();
        str.Append($"Health: {Health.ToString():2f} |" +
            $"Damage: {CommonAttributes.BaseDamageMultiplier.ToString():2f} |" + 
            $"Character Element: {CommonAttributes.Element}");
        return str.ToString();
    }

    // Events
    // Registered on CheatsConsole, EnemyScript.
    protected virtual void OnEventTakeDamage(float damageToReceive) => EventTakeDamage?.Invoke(damageToReceive);
    public Action<float> EventTakeDamage;
}
