/// <summary>
/// Interface implemented by enemy animator with possible actions.
/// </summary>
public interface IEnemyAnimator 
{
    /// <summary>
    /// Triggers enemy attack.
    /// </summary>
    /// <param name="enemyAttackType">Type of the attack.</param>
    void TriggerAttack(EnemyAttackTypeAnimations enemyAttackType);

    /// <summary>
    /// Triggers enraged animation.
    /// </summary>
    void TriggerEnragedAnimation();

    /// <summary>
    /// Resets class.
    /// </summary>
    void ResetAll();
}
