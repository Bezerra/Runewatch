public interface IDamageable
{
    /// <summary>
    /// Takes damage.
    /// </summary>
    /// <param name="damage">Damage to take.</param>
    /// <param name="element">Element of the damage.</param>
    void TakeDamage(float damage, ElementType element);

    /// <summary>
    /// Starts a coroutine to take damage overtime.
    /// </summary>
    /// <param name="damage">Damage to take.</param>
    /// <param name="element">Element of the damage.</param>
    /// <param name="timeInterval">Takes damage every time interval. (ex: takes damage every 0.2 seconds, timeInterval = 0.2f).</param>
    /// <param name="maxTime">Takes damage for this time. (ex: this effect lasts for 5 seconds, maxTime = 5f).</param>
    void TakeDamageOvertime(float damage, ElementType element, float timeInterval, float maxTime);
}
