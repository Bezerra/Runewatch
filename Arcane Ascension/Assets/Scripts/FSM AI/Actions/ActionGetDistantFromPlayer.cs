using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Action to get a minimum distance from player.
/// </summary>
[CreateAssetMenu(menuName = "FSM/Actions/Action Get Distant From Player", 
fileName = "Action Get Distant From Player")]
sealed public class ActionGetDistantFromPlayer : FSMAction
{
    public override void Execute(StateController<Enemy> ai)
    {
        ChasePlayer(ai);
    }

    private void ChasePlayer(StateController<Enemy> ai)
    {
        // If the agent has reached its final destination
        if (ai.Controller.CurrentTarget != null)
        {
            if (Vector3.Distance(
                ai.Controller.transform.position, ai.Controller.CurrentTarget.position) < 
                ai.Controller.DistanceToKeepFromTarget * 0.5f)
            {
                ai.Controller.WalkingBackwards = true;
                ai.Controller.Agent.SetDestination(ai.Controller.transform.position + ai.Controller.CurrentTarget.Direction(ai.Controller.transform.position));
            }
            else
            {
                ai.Controller.WalkingBackwards = false;
            }
        }
    }

    public override void OnEnter(StateController<Enemy> ai)
    {
        ai.Controller.WalkingBackwards = false;

        ai.Controller.DistanceToKeepFromTarget = Random.Range(
            ai.Controller.Values.DistanceToKeepFromTarget.x, ai.Controller.Values.DistanceToKeepFromTarget.y);
    }

    public override void OnExit(StateController<Enemy> ai)
    {
        // Left blank on purpose
    }
}
