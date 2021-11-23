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
        // If the agent has reached its final destination
        if (ai.StateTimeElapsed > 1)
        {
            if (ai.Controller.CurrentTarget != null)
            {
                if (Vector3.Distance(
                    ai.Controller.transform.position, ai.Controller.CurrentTarget.position) <
                    ai.Controller.DistanceToKeepFromTarget * 2)
                {
                    if (Mathf.Floor(ai.StateTimeElapsed) % 10 == 0)
                    {
                        ai.Controller.DirectionIfBackBlocked = Random.Range(0, 2) == 0 ?
                            Direction.Right : Direction.Left;
                    }

                    ai.Controller.RunningBackwards = true;
                    ai.Controller.Agent.speed = ai.Controller.Values.Speed *
                        ai.Controller.AllValues.CharacterStats.MovementSpeedMultiplier * 1.25f;
                    float rotationSign = ai.Controller.DirectionIfBackBlocked == Direction.Right ?
                        1 : -1;

                    Ray rayToFront = new Ray(
                        ai.Controller.transform.position - ai.Controller.transform.forward, 
                        ai.Controller.transform.forward);

                    if (Physics.Raycast(rayToFront, 6, Layers.WallsFloor))
                    {
                        ai.Controller.transform.Rotate(
                            Vector3.up, Time.deltaTime * ai.Controller.Values.RotationSpeed * 50f * rotationSign);

                        Ray rayToSide;
                        if (ai.Controller.DirectionIfBackBlocked == Direction.Right)
                            rayToSide = new Ray(ai.Controller.transform.position + ai.Controller.transform.right,
                                -ai.Controller.transform.right);
                        else
                            rayToSide = new Ray(ai.Controller.transform.position - ai.Controller.transform.right,
                                ai.Controller.transform.right);

                        if (Physics.Raycast(rayToSide, 4, Layers.WallsFloor))
                        {
                            Debug.Log("hit");
                            ai.Controller.transform.Rotate(
                                Vector3.up, Time.deltaTime * ai.Controller.Values.RotationSpeed * 50f * rotationSign);
                        }
                    }

                    ai.Controller.Agent.SetDestination(ai.Controller.transform.position +
                            ai.Controller.transform.forward);
                }
                else
                {
                    ai.Controller.RunningBackwards = false;
                    ai.Controller.Agent.speed = ai.Controller.Values.Speed *
                        ai.Controller.AllValues.CharacterStats.MovementSpeedMultiplier; ;
                }
            }
        }
    }

    public override void OnEnter(StateController<Enemy> ai)
    {
        ai.Controller.RunningBackwards = false;

        ai.Controller.DistanceToKeepFromTarget = Random.Range(
            ai.Controller.Values.DistanceWhileRunningBackwards.x, 
            ai.Controller.Values.DistanceWhileRunningBackwards.y);

        ai.Controller.DirectionIfBackBlocked = Random.Range(0, 2) == 0 ?
            Direction.Right : Direction.Left;

        if (ai.Controller.CurrentTarget == null)
            ai.Controller.CurrentTarget = ai.Controller.PlayerScript.Eyes.transform;

        ai.Controller.Agent.SetDestination(ai.Controller.transform.position +
            ai.Controller.CurrentTarget.Direction(ai.Controller.transform.position) *
            ai.Controller.DistanceToKeepFromTarget);
    }

    public override void OnExit(StateController<Enemy> ai)
    {
        ai.Controller.RunningBackwards = false;
        ai.Controller.Agent.speed = ai.Controller.Values.Speed *
            ai.Controller.AllValues.CharacterStats.MovementSpeedMultiplier; ;
    }
}
