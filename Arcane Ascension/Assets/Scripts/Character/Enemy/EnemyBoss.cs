using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Class responsible for boss enemies.
/// </summary>
public class EnemyBoss : Enemy
{
    [SerializeField] private EnemyCharacterSO enragedCharacterSO;
    [Range(0f, 1f)] [SerializeField] private float healthToEnrage = 0.5f;

    [Header("Must not be higher than health to enrage")]
    [Range(0f, 1f)] [SerializeField] private float percentageToCastFirstMechanic = 0.5f;
    [Range(0f, 1f)] [SerializeField] private float percentageToCastSdcondMechanic = 0.2f;

    private float healthOnEnrageTransition;

    public bool ExecuteFirstMechanic { get; set; }
    public bool ExecutedFirstMechanic { get; set; }
    public bool ExecuteSecondMechanic { get; set; }
    public bool ExecutedSecondMechanic { get; set; }
    public IList<GameObject> ExtraMechanics { get; set; }

    protected override void Awake()
    {
        base.Awake();
        ExtraMechanics = new List<GameObject>();
    }

    protected override void Start()
    {
        CurrentlySelectedSpell = EnemyStats.EnemyAttributes.AllEnemySpells[0];
        Agent.speed = Values.Speed * EnemyStats.CommonAttributes.MovementSpeedMultiplier *
            EnemyStats.CommonAttributes.MovementStatusEffectMultiplier;
    }

    public void StartStateMachine()
    {
        StateMachine.StartNoDelay();
    }

    /// <summary>
    /// If a value is met, triggers enraged animation + logic.
    /// </summary>
    protected override void EventTakeDamage()
    {
        base.EventTakeDamage();

        // Removes extra resistance in case it bugs on SafeZone script
        if (ExecutedFirstMechanic || ExecutedSecondMechanic)
        {
            if (ExtraMechanics.Count > 0)
            {
                bool objectsStillActive = false;
                foreach (GameObject go in ExtraMechanics)
                {
                    if (go.activeSelf) objectsStillActive = true;
                }

                if (objectsStillActive == false)
                {
                    EnemyStats.CommonAttributes.DamageResistanceStatusEffectMultiplier = 0;
                    ExtraMechanics.Clear();
                }
            }
        }

        if (EnemyStats.Health / EnemyStats.MaxHealth <= percentageToCastFirstMechanic)
            ExecuteFirstMechanic = true;

        if (EnemyStats.Health / EnemyStats.MaxHealth <= percentageToCastSdcondMechanic)
            ExecuteSecondMechanic = true;

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
