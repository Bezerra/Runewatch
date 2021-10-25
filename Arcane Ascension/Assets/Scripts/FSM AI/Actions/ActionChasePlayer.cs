using UnityEngine;

/// <summary>
/// Action to chase player.
/// </summary>
[CreateAssetMenu(menuName = "FSM/Actions/Action Chase Player", fileName = "Action Chase Player")]
sealed public class ActionChasePlayer : FSMAction
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
                ai.Controller.transform.position, ai.Controller.CurrentTarget.position) > ai.Controller.DistanceToKeepFromTarget)
            {
                if (ai.Controller.WalkingBackwards == false)
                {
                    ai.Controller.Agent.SetDestination(ai.Controller.transform.position + ai.Controller.transform.forward);
                }
            }
        }
    }

    public override void OnEnter(StateController<Enemy> ai)
    {
        ai.Controller.DistanceToKeepFromTarget = Random.Range(
            ai.Controller.Values.DistanceToKeepFromTarget.x, ai.Controller.Values.DistanceToKeepFromTarget.y);
    }

    public override void OnExit(StateController<Enemy> ai)
    {
        // Left blank on purpose
    }
}
