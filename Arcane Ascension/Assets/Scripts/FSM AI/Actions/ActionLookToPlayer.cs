using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Action to chase player.
/// </summary>
[CreateAssetMenu(menuName = "FSM/Actions/Action Look To Player", fileName = "Action Look To Player")]
sealed public class ActionLookToPlayer : FSMAction
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
                ai.Controller.transform.LookAtYLerp(ai.Controller.CurrentTarget, ai.Controller.Values.RotationSpeed);
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
