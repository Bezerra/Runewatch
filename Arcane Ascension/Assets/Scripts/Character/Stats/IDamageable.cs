public interface IDamageable
{
    /// <summary>
    /// Takes damage.
    /// </summary>
    /// <param name="damage">Damage to take.</param>
    /// <param name="element">Element of the damage.</param>
    void TakeDamage(float damage, ElementType element);

    /// <summary>
    /// Reduces armor + Health.
    /// </summary>
    /// <param name="damage">Damage to take.</param>
    /// <param name="criticalChance">Chance of critical hit.</param>
    /// <param name="element">Element of the damage.</param>
    void TakeDamage(float damage, float criticalChance, ElementType element);

    /// <summary>
    /// Starts a coroutine to take damage overtime.
    /// If the enemy is already taking damage overtime, it will restart it.
    /// </summary>
    /// <param name="damage">Damage to take.</param>
    /// <param name="element">Element of the damage.</param>
    /// <param name="timeInterval">Takes damage every time interval. (ex: takes damage every 0.2 seconds, timeInterval = 0.2f).</param>
    /// <param name="maxTime">Takes damage for this time. (ex: this effect lasts for 5 seconds, maxTime = 5f).</param>
    void TakeDamageOvertime(float damage, ElementType element, float timeInterval, float maxTime);
}
