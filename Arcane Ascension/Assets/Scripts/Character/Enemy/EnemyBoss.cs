using UnityEngine;

/// <summary>
/// Class responsible for boss enemies.
/// </summary>
public class EnemyBoss : Enemy
{
    [SerializeField] private EnemyCharacterSO enragedCharacterSO;
    [Range(0f, 1f)] [SerializeField] private float healthToEnrage = 0.5f;

    private float healthOnEnrageTransition;

    protected override void Start()
    {
        CurrentlySelectedSpell = EnemyStats.EnemyAttributes.AllEnemySpells[0];
        Agent.speed = Values.Speed * EnemyStats.CommonAttributes.MovementSpeedMultiplier *
            EnemyStats.CommonAttributes.MovementStatusEffectMultiplier;
    }

    public void StartStateMachine()
    {
        StateMachine.Start();
    }

    /// <summary>
    /// If a value is met, triggers enraged animation + logic.
    /// </summary>
    protected override void EventTakeDamage()
    {
        base.EventTakeDamage();

        if (CommonValues == enragedCharacterSO) return;
        if (EnemyStats.Health / EnemyStats.MaxHealth <= healthToEnrage)
        {
            // Gets  enemy's current health
            healthOnEnrageTransition = 0;
            healthOnEnrageTransition = EnemyStats.Health;

            CommonValues = enragedCharacterSO;

            // Resets variables for important classes
            EnemyStats.ResetAll();
            ResetAll();
            Animation.ResetAll();

            // Sets health to the previous state health
            EnemyStats.Health = healthOnEnrageTransition;

            // Triggers enraged animation
            Animation.TriggerEnragedAnimation();

            // Stops states logic
            StateMachine.Stop();

            // Next state will be trigered after engaged animation is over
            // on fireDemonEnragedAnimationBehaviour script
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
