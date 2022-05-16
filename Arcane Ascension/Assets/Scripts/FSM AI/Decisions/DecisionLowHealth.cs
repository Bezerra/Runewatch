using UnityEngine;

/// <summary>
/// Decision that checks if enemy is low health.
/// </summary>
[CreateAssetMenu(menuName = "FSM/Decisions/Decision Low Health", 
    fileName = "Decision Low Health")]
public class DecisionLowHealth : FSMDecision
{
    private readonly float HEALTHPERCENTAGETORUN = 30f;

    public override bool CheckDecision(StateController<Enemy> ai)
    {
        if ((ai.Controller.EnemyStats.Health * 100) / ai.Controller.EnemyStats.MaxHealth < 
            HEALTHPERCENTAGETORUN)
        {
            return true;
        }
        return false;
    }

    public override void OnEnter(StateController<Enemy> ai)
    {
        // Left blank on purpose
    }

    public override void OnExit(StateController<Enemy> ai)
    {
        // Left blank on purpose
    }
}
