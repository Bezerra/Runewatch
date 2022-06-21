using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Action to look away from.
/// </summary>
[CreateAssetMenu(menuName = "FSM/Actions/Action Look Away From Player", fileName = "Action Look Away From Player")]
sealed public class ActionLookAwayFromPlayer : FSMAction
{
    public override void Execute(StateController<Enemy> ai)
    {
        RotateToPlayer(ai);
    }

    private void RotateToPlayer(StateController<Enemy> ai)
    {
        if (ai.Controller.CurrentTarget != null)
        {
            if (ai.Controller.RunningBackwards == false)
            {
                ai.Controller.transform.LookAtYLerp(
                    ai.Controller.transform.position + 
                    ai.Controller.CurrentTarget.Direction(ai.Controller.transform.position), 
                    ai.Controller.Values.RotationSpeed);
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
