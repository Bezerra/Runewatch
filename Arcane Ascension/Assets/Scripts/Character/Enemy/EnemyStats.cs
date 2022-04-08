using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using ExtensionMethods;

/// <summary>
/// Class responsible for handling enemy stats.
/// </summary>
public class EnemyStats : Stats
{
    public EnemyStatsSO EnemyAttributes => character.CommonValues.CharacterStats as EnemyStatsSO;

    // Creates a list of room weights 
    public IList<int> AvailableSpellsWeight { get; private set; }

    private IList<(LootType, Vector3)> droppedLoot;

    // Save data
    private CharacterSaveDataController stpData;

    // Death variables
    private bool dead;
    private SelectionBase enemyRoot;
    private Collider[] rootColliders;
    private NavMeshAgent agent;
    private Enemy enemy;
    private MinimapIcon minimapIcon;

    protected override void Awake()
    {
        base.Awake();
        
        // Creates a list of spell weights 
        AvailableSpellsWeight = new List<int>();
        stpData = FindObjectOfType<CharacterSaveDataController>();
        droppedLoot = new List<(LootType, Vector3)>();
        enemy = GetComponent<Enemy>();
        enemyRoot = GetComponentInParent<SelectionBase>();
        rootColliders = enemyRoot.GetComponentsInChildren<Collider>();
        agent = GetComponent<NavMeshAgent>();
        minimapIcon = GetComponentInChildren<MinimapIcon>();
    }

    protected override void Start()
    {
        base.Start();
        
        for (int i = 0; i < EnemyAttributes.AllEnemySpells.Count; i++)
        {
            AvailableSpellsWeight.Add(EnemyAttributes.AllEnemySpells[i].SpellWeight);
        }
    }

    private void OnEnable()
    {
        EnemyAttributes.Initialize();
    }

    /// <summary>
    /// Reduces armor + Health.
    /// </summary>
    /// <param name="damage">Damage to take.</param>
    /// <param name="element">Element of the damage.</param>
    /// <param name="damagePosition">Position of the damage.</param>
    public override float TakeDamage(float damage, ElementType element, Vector3 damagePosition)
    {
        float damageToReceive =
            Mathf.Floor(
                damage * (ElementsDamage.CalculateDamage(element, CommonAttributes.Element)) *
                (CommonAttributes.DamageResistance + CommonAttributes.DamageResistanceStatusEffectMultiplier));

        // Achievement
        achievementLogic.TriggerAchievement(AchievementType.MostDamageDone, (int)damageToReceive);
        achievementLogic.TriggerAchievement(AchievementType.DamageDone, (int)damageToReceive);

        // Prevents healing from negative damage
        if (damageToReceive < 0) damageToReceive = 0;

        OnEventTakeDamage(damageToReceive);

        // Spawn damage text
        if (PlayerPrefs.GetFloat(PPrefsOptions.DamageDealt.ToString(), 1) == 1)
        {
            GameObject damageHitText =
                    DamageHitPoolCreator.Pool.InstantiateFromPool(
                        "DamageHit", transform.position, Quaternion.identity);

            if (damageHitText.TryGetComponent(out DamageHitText outDamageHitText))
            {
                outDamageHitText.UpdateShownDamage(damageToReceive, false);
            }
        }   

        if (Health - damageToReceive > 0)
        {
            Health -= damageToReceive;
            OnEventTakeDamage();
            OnEventHealthUpdate();
        }
        else
        {
            // Death animation will be triggered in EnemyAnimations
            // This if only happens once, for sure
            if (dead == false)
            {
                if (damageOvertimeCoroutine != null) StopCoroutine(damageOvertimeCoroutine);
                Health = 0;
                OnEventTakeDamage();
                OnEventHealthUpdate();
                OnEventDeath(this);

                EnemyDeath(ref dead);

                // Gets random drops and spawns them
                GetDrop(transform.position + new Vector3(0, -transform.localPosition.y * 0.5f, 0));
                IEnumerator<(LootType, Vector3)> itemEnumerator = droppedLoot.GetEnumerator();
                while (itemEnumerator.MoveNext())
                {
                    GameObject spawnedLoot = 
                        ItemLootPoolCreator.Pool.InstantiateFromPool(
                        itemEnumerator.Current.Item1.ToString(),
                        itemEnumerator.Current.Item2, Quaternion.identity);

                    if (spawnedLoot.TryGetComponentInChildrenFirstGen(out ICurrency currency))
                    {
                        if (currency.CurrencyType == CurrencyType.ArcanePower)
                            currency.Amount = EnemyAttributes.ArcanePowerQuantity;
                    }
                }

                this.enabled = false;
            }
        }

        return damageToReceive;
    }

    /// <summary>
    /// Reduces Health.
    /// </summary>
    /// <param name="damage">Damage to take.</param>
    /// <param name="criticalChance">Chance of critical hit.</param>
    /// <param name="criticalDamageModifier">Damage modifier on critical hits.</param>
    /// <param name="element">Element of the damage.</param>
    /// <param name="damagePosition">Position of the damage.</param> 
    public override float TakeDamage(float damage, float criticalChance, float criticalDamageModifier, 
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

        // Achievement
        achievementLogic.TriggerAchievement(AchievementType.MostDamageDone, (int)damageToReceive);
        achievementLogic.TriggerAchievement(AchievementType.DamageDone, (int)damageToReceive);

        // Prevents healing from negative damage
        if (damageToReceive < 0) damageToReceive = 0;

        OnEventTakeDamage(damageToReceive);

        // Spawn damage text
        if (PlayerPrefs.GetFloat(PPrefsOptions.DamageDealt.ToString(), 1) == 1)
        {
            GameObject damageHitText =
                DamageHitPoolCreator.Pool.InstantiateFromPool(
                    "DamageHit", transform.position, Quaternion.identity);

            if (damageHitText.TryGetComponent(out DamageHitText outDamageHitText))
            {
                outDamageHitText.UpdateShownDamage(damageToReceive, criticalHit);
            }
        }

        if (Health - damageToReceive > 0)
        {
            Health -= damageToReceive;
            OnEventTakeDamage();
            OnEventHealthUpdate();
        }
        else
        {
            // Death animation will be triggered in EnemyAnimations
            // This if only happens once, for sure
            if (dead == false)
            {
                if (damageOvertimeCoroutine != null) StopCoroutine(damageOvertimeCoroutine);
                Health = 0;
                OnEventTakeDamage();
                OnEventHealthUpdate();
                OnEventDeath(this);

                EnemyDeath(ref dead);

                // Gets random drops and spawns them
                GetDrop(transform.position + new Vector3(0, -transform.localPosition.y * 0.5f, 0));
                IEnumerator<(LootType, Vector3)> itemEnumerator = droppedLoot.GetEnumerator();
                while (itemEnumerator.MoveNext())
                {
                    GameObject spawnedLoot = 
                        ItemLootPoolCreator.Pool.InstantiateFromPool(
                        itemEnumerator.Current.Item1.ToString(),
                        itemEnumerator.Current.Item2, Quaternion.identity);

                    if (spawnedLoot.TryGetComponentInChildrenFirstGen(out ICurrency currency))
                    {
                        if (currency.CurrencyType == CurrencyType.ArcanePower)
                            currency.Amount = EnemyAttributes.ArcanePowerQuantity;
                    }
                }

                this.enabled = false;
            }
        }

        return damageToReceive;
    }

    /// <summary>
    /// Gets a drop and sets random position with a received position.
    /// </summary>
    /// <param name="position">Position to set the item.</param>
    protected void GetDrop(Vector3 position)
    {
        for (int i = 0; i < EnemyAttributes.Rates.LootPieces.Count; i++)
        {
            // If it's a healing potion, its rate will be automatically set
            if (EnemyAttributes.Rates.LootPieces[i].LootType == LootType.PotionHealing)
            {
                EnemyAttributes.Rates.LootPieces[i].LootRate = stpData.SaveData.Reaper;
            }

            if (EnemyAttributes.Rates.LootPieces[i].LootRate.PercentageCheck(random))
            {
                Vector3 newPosition = position + new Vector3(
                    UnityEngine.Random.Range(-1f, 1f), 0,
                    UnityEngine.Random.Range(-1f, 1f));

                droppedLoot.Add((EnemyAttributes.Rates.LootPieces[i].LootType, newPosition));
            }
        }
    }

    /// <summary>
    /// Destroys enemy colliders.
    /// </summary>
    private void EnemyDeath(ref bool dead)
    {
        foreach (Collider colliders in rootColliders)
            colliders.enabled = false;

        enemy.enabled = false;

        if (minimapIcon != null)
            Destroy(minimapIcon.transform.parent.gameObject);

        if (agent != null)
        {
            agent.isStopped = true;
            agent.radius = 0;
        }
        dead = true;
    }   

    /// <summary>
    /// Resets the class.
    /// </summary>
    public void ResetAll()
    {
        Awake();
        OnEnable();
        Start();
    }
}
