using UnityEngine;

/// <summary>
/// Decision that checks if enemy is chasing the player.
/// </summary>
[CreateAssetMenu(menuName = "FSM/Decisions/Decision Chasing Player", fileName = "Decision Chasing Player")]
public class DecisionChasingPlayer : FSMDecision
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
