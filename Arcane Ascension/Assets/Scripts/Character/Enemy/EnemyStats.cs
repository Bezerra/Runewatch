using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Class responsible for handling enemy stats.
/// </summary>
public class EnemyStats : Stats
{
    public EnemyStatsSO EnemyAttributes => character.CommonValues.CharacterStats as EnemyStatsSO;

    // Creates a list of room weights 
    public IList<int> AvailableSpellsWeight { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        // Creates a list of spell weights 
        AvailableSpellsWeight = new List<int>();
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
    public override void TakeDamage(float damage, ElementType element)
    {
        float damageToReceive =
            Mathf.Floor(
                damage * (ElementsDamage.CalculateDamage(element, CommonAttributes.Element)) *
                CommonAttributes.DamageResistance);
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
        }
        else
        {
            if (damageOvertimeCoroutine != null) StopCoroutine(damageOvertimeCoroutine);
            OnEventDeath(this);

            // Gets random drops and spawns them
            EnemyAttributes.Rates.GetDrop(transform.position + new Vector3(0, -transform.localPosition.y, 0));
            IEnumerator<(LootType, Vector3)> itemEnumerator = EnemyAttributes.Rates.DroppedLoot.GetEnumerator();
            while (itemEnumerator.MoveNext())
            {
                ItemLootPoolCreator.Pool.InstantiateFromPool(
                    itemEnumerator.Current.Item1.ToString(),
                    itemEnumerator.Current.Item2, Quaternion.identity);
            }

            // This will be applied with an animation event
            Destroy(GetComponentInParent<SelectionBase>().gameObject);
            //////////////////////////////////////////////////////////
        }
    }

    /// <summary>
    /// Reduces Health.
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
        float damageToReceive =
            Mathf.Floor(
                damage * (ElementsDamage.CalculateDamage(element, CommonAttributes.Element)) *
                CommonAttributes.DamageResistance);
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
        }
        else
        {
            if (damageOvertimeCoroutine != null) StopCoroutine(damageOvertimeCoroutine);
            OnEventDeath(this);

            // Gets random drops and spawns them
            EnemyAttributes.Rates.GetDrop(transform.position + new Vector3(0, -transform.localPosition.y, 0));
            IEnumerator<(LootType, Vector3)> itemEnumerator = EnemyAttributes.Rates.DroppedLoot.GetEnumerator();
            while (itemEnumerator.MoveNext())
            {
                ItemLootPoolCreator.Pool.InstantiateFromPool(
                    itemEnumerator.Current.Item1.ToString(),
                    itemEnumerator.Current.Item2, Quaternion.identity);
            }

            // This will be applied with an animation event
            Destroy(GetComponentInParent<SelectionBase>().gameObject);
            //////////////////////////////////////////////////////////
        }
    }
}
