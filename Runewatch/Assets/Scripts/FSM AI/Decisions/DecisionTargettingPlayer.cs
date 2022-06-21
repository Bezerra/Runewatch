using UnityEngine;

/// <summary>
/// Decision that checks if enemy is chasing the player.
/// </summary>
[CreateAssetMenu(menuName = "FSM/Decisions/Decision Targeting Player", 
    fileName = "Decision Targeting Player")]
public class DecisionTargettingPlayer : FSMDecision
{
    public override bool CheckDecision(StateController<Enemy> ai)
    {
        bool hasTarget = ai.Controller.CurrentTarget != null;

        return hasTarget;
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
