/// <summary>
/// Interface implemented by enemy animator with possible actions.
/// </summary>
public interface IEnemyAnimator 
{
    /// <summary>
    /// Triggers enemy attack.
    /// </summary>
    /// <param name="enemyAttackType">Type of the attack.</param>
    void TriggerAttack(EnemyAttackType enemyAttackType);
}
