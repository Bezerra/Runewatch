using UnityEngine;

/// <summary>
/// Action to move to player's last known position.
/// </summary>
[CreateAssetMenu(menuName = "FSM/Actions/Action Move To Last Known Position", 
    fileName = "Action Move To Last Known Position")]
sealed public class ActionMoveLastKnownPosition : FSMAction
{
    public override void Execute(StateController<Enemy> ai)
    {
        // Left blank on purpose
    }

    public override void OnEnter(StateController<Enemy> ai)
    {
        ai.Controller.Agent.SetDestination(ai.Controller.PlayerLastKnownPosition);
    }

    public override void OnExit(StateController<Enemy> ai)
    {
        // Left blank on purpose
    }
}
