using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Action to run from player.
/// </summary>
[CreateAssetMenu(menuName = "FSM/Actions/Action Run From Player", 
fileName = "Action Run From Player")]
sealed public class ActionRunFromPlayer : FSMAction
{
    public override void Execute(StateController<Enemy> ai)
    {
        RunFromPlayer(ai);
    }

    private void RunFromPlayer(StateController<Enemy> ai)
    {
        if (ai.Controller.CurrentTarget != null)
        {
            Ray forwardRay =
                new Ray(ai.Controller.transform.position,
                    ai.Controller.CurrentTarget.Direction(ai.Controller.transform.position));

            if (Physics.Raycast(forwardRay, 1.5f, Layers.WallsFloor))
            {
                // Left blank on purpose
            }
            else
            {
                ai.Controller.Agent.SetDestination(ai.Controller.transform.position +
                    ai.Controller.CurrentTarget.Direction(ai.Controller.transform.position));
            }
        }
    }

    public override void OnEnter(StateController<Enemy> ai)
    {
        ai.Controller.Agent.angularSpeed = 400;

        ai.Controller.RunningBackwards = true;

        if (ai.Controller.CurrentTarget == null)
            ai.Controller.CurrentTarget = ai.Controller.PlayerScript.Eyes.transform;
    }

    public override void OnExit(StateController<Enemy> ai)
    {
        ai.Controller.RunningBackwards = false;
        ai.Controller.Agent.speed = ai.Controller.Values.Speed *
            ai.Controller.EnemyStats.CommonAttributes.MovementSpeedMultiplier *
            ai.Controller.EnemyStats.CommonAttributes.MovementStatusEffectMultiplier;
    }
}
