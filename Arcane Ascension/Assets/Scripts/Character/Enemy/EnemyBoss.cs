using UnityEngine;

/// <summary>
/// Class responsible for boss enemies.
/// </summary>
public class EnemyBoss : Enemy
{
    [SerializeField] private EnemyCharacterSO enragedCharacterSO;
    [Range(0f, 1f)] [SerializeField] private float healthToEnrage = 0.5f;

    /// <summary>
    /// If a value is met, triggers enraged animation + logic.
    /// </summary>
    protected override void EventTakeDamage()
    {
        base.EventTakeDamage();

        if (CommonValues == enragedCharacterSO) return;
        if (EnemyStats.Health / EnemyStats.MaxHealth <= healthToEnrage)
        {
            CommonValues = enragedCharacterSO;
            EnemyStats.ResetAll();
            EnemyStats.Heal(EnemyStats.MaxHealth, StatsType.Health);
            ResetAll();
            Animation.ResetAll();
            Animation.TriggerEnragedAnimation();
            StateMachine.Stop();
        }
    }

    /// <summary>
    /// Allows state machine to run.
    /// </summary>
    public void ActivateEnragedState()
    {
        StateMachine.StartNoDelay();
    }
}
