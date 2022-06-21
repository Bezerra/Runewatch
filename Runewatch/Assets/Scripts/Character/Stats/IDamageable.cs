using UnityEngine;

/// <summary>
/// Interface implemented by damageable characters.
/// </summary>
public interface IDamageable
{
    /// <summary>
    /// Takes damage.
    /// </summary>
    /// <param name="damage">Damage to take.</param>
    /// <param name="element">Element of the damage.</param>
    /// <param name="damagePosition">Position of the damage.</param> 
    float TakeDamage(float damage, ElementType element, Vector3 damagePosition);

    /// <summary>
    /// Reduces armor + Health.
    /// </summary>
    /// <param name="damage">Damage to take.</param>
    /// <param name="criticalChance">Chance of critical hit.</param>
    /// <param name="criticalDamageModifier">Damage modifier on critical hits.</param>
    /// <param name="element">Element of the damage.</param>
    /// <param name="damagePosition">Position of the damage.</param> 
    float TakeDamage(float damage, float criticalChance, float criticalDamageModifier, 
        ElementType element, Vector3 damagePosition);

    /// <summary>
    /// Starts a coroutine to take damage overtime.
    /// If the enemy is already taking damage overtime, it will restart it.
    /// </summary>
    /// <param name="damage">Damage to take.</param>
    /// <param name="element">Element of the damage.</param>
    /// <param name="timeInterval">Takes damage every time interval. (ex: takes damage every 0.2 seconds, timeInterval = 0.2f).</param>
    /// <param name="maxTime">Takes damage for this time. (ex: this effect lasts for 5 seconds, maxTime = 5f).</param>
    void TakeDamageOvertime(float damage, ElementType element, float timeInterval, 
        float maxTime);
}
