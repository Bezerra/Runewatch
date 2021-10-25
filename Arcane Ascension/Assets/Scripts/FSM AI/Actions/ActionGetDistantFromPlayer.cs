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
        GetDistantFromPlayer(ai);
    }

    private void GetDistantFromPlayer(StateController<Enemy> ai)
    {
        // If the agent has reached its final destination
        if (ai.Controller.CurrentTarget != null)
        {
            if (Vector3.Distance(
                ai.Controller.transform.position, ai.Controller.CurrentTarget.position) < 
                ai.Controller.DistanceToKeepFromTarget * 0.75f)
            {
                ai.Controller.WalkingBackwards = true;

                Ray rayToback = new Ray(ai.Controller.transform.position + ai.Controller.transform.forward, -ai.Controller.transform.forward);
                if (Physics.Raycast(rayToback, 3, Layers.WallsFloor))
                {
                    Ray rayToSide;
                    if (ai.Controller.DirectionIfBackBlocked == Direction.Right)
                        rayToSide = new Ray(ai.Controller.transform.position + ai.Controller.transform.right,
                            -ai.Controller.transform.right);
                    else
                        rayToSide = new Ray(ai.Controller.transform.position - ai.Controller.transform.right,
                            ai.Controller.transform.right);

                    if (Physics.Raycast(rayToSide, 4, Layers.WallsFloor))
                    {
                        if (ai.Controller.DirectionIfBackBlocked == Direction.Right)
                        {
                            ai.Controller.DirectionIfBackBlocked = Direction.Left;
                        }
                        else
                            ai.Controller.DirectionIfBackBlocked = Direction.Right;
                    }
                    else
                    {
                        if (ai.Controller.DirectionIfBackBlocked == Direction.Right)
                            ai.Controller.Agent.SetDestination(ai.Controller.transform.position + ai.Controller.transform.right);
                        else
                            ai.Controller.Agent.SetDestination(ai.Controller.transform.position - ai.Controller.transform.right);
                    }
                }
                else
                {
                    ai.Controller.Agent.SetDestination(ai.Controller.transform.position + 
                        ai.Controller.CurrentTarget.Direction(ai.Controller.transform.position));
                }

                ai.Controller.Agent.angularSpeed = 0;
                ai.Controller.TimeStoppedWalkingBackwards = Time.time;
            }
            else
            {
                if (Time.time - ai.Controller.TimeStoppedWalkingBackwards > 1)
                    ai.Controller.Agent.angularSpeed = ai.Controller.Values.AgentAngularSpeed;

                ai.Controller.WalkingBackwards = false;
            }
        }
    }

    public override void OnEnter(StateController<Enemy> ai)
    {
        ai.Controller.WalkingBackwards = false;
        ai.Controller.TimeStoppedWalkingBackwards = 0;

        ai.Controller.DistanceToKeepFromTarget = Random.Range(
            ai.Controller.Values.DistanceToKeepFromTarget.x, ai.Controller.Values.DistanceToKeepFromTarget.y);

        ai.Controller.DirectionIfBackBlocked = Random.Range(0, 2) == 0 ?
            Direction.Right : Direction.Left;
    }

    public override void OnExit(StateController<Enemy> ai)
    {
        ai.Controller.WalkingBackwards = false;
        ai.Controller.Agent.angularSpeed = ai.Controller.Values.AgentAngularSpeed;
    }
}
