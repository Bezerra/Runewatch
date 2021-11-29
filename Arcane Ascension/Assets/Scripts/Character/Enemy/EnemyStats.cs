using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

/// <summary>
/// Class responsible for handling enemy stats.
/// </summary>
public class EnemyStats : Stats
{
    public EnemyStatsSO EnemyAttributes => character.CommonValues.CharacterStats as EnemyStatsSO;

    // Creates a list of room weights 
    public IList<int> AvailableSpellsWeight { get; private set; }

    // Save data
    private CharacterSaveDataController stpData;

    protected override void Awake()
    {
        base.Awake();

        // Creates a list of spell weights 
        AvailableSpellsWeight = new List<int>();
        stpData = FindObjectOfType<CharacterSaveDataController>();
        EnemyAttributes.Initialize(stpData);
    }

    protected override void Start()
    {
        base.Start();
        
        for (int i = 0; i < EnemyAttributes.AllEnemySpells.Count; i++)
        {
            AvailableSpellsWeight.Add(EnemyAttributes.AllEnemySpells[i].SpellWeight);
        }
    }

    /// <summary>
    /// Reduces armor + Health.
    /// </summary>
    /// <param name="damage">Damage to take.</param>
    /// <param name="element">Element of the damage.</param>
    /// <param name="damagePosition">Position of the damage.</param>
    public override void TakeDamage(float damage, ElementType element, Vector3 damagePosition)
    {
        float damageToReceive =
            Mathf.Floor(
                damage * (ElementsDamage.CalculateDamage(element, CommonAttributes.Element)) *
                (CommonAttributes.DamageResistance + CommonAttributes.DamageResistanceStatusEffectMultiplier));

        // Prevents healing from negative damage
        if (damageToReceive < 0) damageToReceive = 0;

        OnEventTakeDamage(damageToReceive);

        // Spawn damage text
        GameObject damageHitText =
                    DamageHitPoolCreator.Pool.InstantiateFromPool("DamageHit", transform.position, Quaternion.identity);
        if (damageHitText.TryGetComponent<DamageHitText>(out DamageHitText outDamageHitText))
        {
            outDamageHitText.UpdateShownDamage(damageToReceive, false);
        }

        if (Health - damageToReceive > 0)
        {
            Health -= damageToReceive;
            OnEventTakeDamage();
            OnEventHealthUpdate();
        }
        else
        {
            if (damageOvertimeCoroutine != null) StopCoroutine(damageOvertimeCoroutine);
            Health = 0;
            OnEventTakeDamage();
            OnEventHealthUpdate();
            OnEventDeath(this);

            // Gets random drops and spawns them
            EnemyAttributes.Rates.GetDrop(transform.position + new Vector3(0, -transform.localPosition.y * 0.5f, 0));
            IEnumerator<(LootType, Vector3)> itemEnumerator = EnemyAttributes.Rates.DroppedLoot.GetEnumerator();
            while (itemEnumerator.MoveNext())
            {
                GameObject spawnedLoot = ItemLootPoolCreator.Pool.InstantiateFromPool(
                    itemEnumerator.Current.Item1.ToString(),
                    itemEnumerator.Current.Item2, Quaternion.identity);

                if(spawnedLoot.TryGetComponent(out ICurrency currency))
                {
                    if (currency.CurrencyType == CurrencyType.Gold) 
                        currency.Amount = EnemyAttributes.GoldQuantity;
                    else
                        currency.Amount = EnemyAttributes.ArcanePowerQuantity;
                }
            }

            // Death animation will be triggered in EnemyAnimations
            EnemyDeath();
        }
    }

    /// <summary>
    /// Reduces Health.
    /// </summary>
    /// <param name="damage">Damage to take.</param>
    /// <param name="criticalChance">Chance of critical hit.</param>
    /// <param name="criticalDamageModifier">Damage modifier on critical hits.</param>
    /// <param name="element">Element of the damage.</param>
    /// <param name="damagePosition">Position of the damage.</param> 
    public override void TakeDamage(float damage, float criticalChance, float criticalDamageModifier, 
        ElementType element, Vector3 damagePosition)
    {
        // Critical check
        // If random.NextDouble is less than critical chance, it will do double damage
        bool criticalHit = random.NextDouble() < criticalChance;
        damage = criticalHit ? damage *= 2 * criticalDamageModifier : damage *= 1;

        // Claculates final damage
        float damageToReceive =
            Mathf.Floor(
                damage * (ElementsDamage.CalculateDamage(element, CommonAttributes.Element)) *
                (CommonAttributes.DamageResistance + CommonAttributes.DamageResistanceStatusEffectMultiplier));

        // Prevents healing from negative damage
        if (damageToReceive < 0) damageToReceive = 0;

        OnEventTakeDamage(damageToReceive);

        // Spawn damage text
        GameObject damageHitText =
                DamageHitPoolCreator.Pool.InstantiateFromPool("DamageHit", transform.position, Quaternion.identity);
        if (damageHitText.TryGetComponent<DamageHitText>(out DamageHitText outDamageHitText))
        {
            outDamageHitText.UpdateShownDamage(damageToReceive, criticalHit);
        }

        if (Health - damageToReceive > 0)
        {
            Health -= damageToReceive;
            OnEventTakeDamage();
            OnEventHealthUpdate();
        }
        else
        {
            if (damageOvertimeCoroutine != null) StopCoroutine(damageOvertimeCoroutine);
            Health = 0;
            OnEventTakeDamage();
            OnEventHealthUpdate();
            OnEventDeath(this);

            // Gets random drops and spawns them
            EnemyAttributes.Rates.GetDrop(transform.position + new Vector3(0, -transform.localPosition.y * 0.5f, 0));
            IEnumerator<(LootType, Vector3)> itemEnumerator = EnemyAttributes.Rates.DroppedLoot.GetEnumerator();
            while (itemEnumerator.MoveNext())
            {
                GameObject spawnedLoot = ItemLootPoolCreator.Pool.InstantiateFromPool(
                    itemEnumerator.Current.Item1.ToString(),
                    itemEnumerator.Current.Item2, Quaternion.identity);

                // Currency is in a child of the prefab
                ICurrency lootCurrency = spawnedLoot.GetComponentInChildren<ICurrency>();
                if (lootCurrency != null)
                {
                    if (lootCurrency.CurrencyType == CurrencyType.Gold)
                    {
                        lootCurrency.AmountMultiplier = stpData.SaveData.Pickpocket;
                        lootCurrency.Amount = EnemyAttributes.GoldQuantity;
                    }
                    else
                        lootCurrency.Amount = EnemyAttributes.ArcanePowerQuantity;
                }
            }

            // Death animation will be triggered in EnemyAnimations
            EnemyDeath();
        }
    }

    /// <summary>
    /// Destroys enemy colliders.
    /// </summary>
    private void EnemyDeath()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        GetComponent<Enemy>().enabled = false;

        if (agent != null)
        {
            agent.isStopped = true;
            agent.radius = 0;
        }

        SelectionBase enemyRoot = GetComponentInParent<SelectionBase>();
        foreach (Collider colliders in enemyRoot.GetComponentsInChildren<Collider>())
            colliders.enabled = false;
    }   
}
