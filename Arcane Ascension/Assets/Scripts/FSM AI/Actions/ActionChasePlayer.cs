using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Action to chase player.
/// </summary>
[CreateAssetMenu(menuName = "FSM/Actions/Action Chase Player", fileName = "Action Chase Player")]
sealed public class ActionChasePlayer : FSMAction
{
    [Range(1, 15)] [SerializeField] private float distanceFromTarget;

    private readonly float ROTATIONSPEED = 3;

    public override void Execute(StateController<Enemy> ai)
    {
        ChasePlayer(ai);
    }

    private void ChasePlayer(StateController<Enemy> ai)
    {
        if (ai.Controller.CurrentTarget != null)
        {
            ai.Controller.transform.LookAtYLerp(ai.Controller.CurrentTarget, ROTATIONSPEED);
        }

        // If the agent has reached its final destination
        if (ai.Controller.CurrentTarget != null)
        {
            if (Vector3.Distance(
                ai.Controller.transform.position, ai.Controller.CurrentTarget.position) < distanceFromTarget)
            {
                
            }
            else
            {
                ai.Controller.Agent.SetDestination(ai.Controller.transform.position + ai.Controller.transform.forward);
            }
        }
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
