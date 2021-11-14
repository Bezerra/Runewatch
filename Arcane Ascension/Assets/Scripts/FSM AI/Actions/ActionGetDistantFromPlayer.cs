using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Gets away from player trying to keep a minimum distance depending on the current equiped
/// spell range. If the enemy is moving towards a wall on its back, it starts moving to a
/// random side.
/// </summary>
[CreateAssetMenu(menuName = "FSM/Actions/Action Get Distant From Player", 
fileName = "Action Get Distant From Player")]
sealed public class ActionGetDistantFromPlayer : FSMAction
{
    public override void Execute(StateController<Enemy> ai)
    {
        GetDistantFromPlayer(ai);
    }

    /// <summary>
    /// Gets away from player trying to keep a minimum distance depending on the current equiped
    /// spell range. If the enemy is moving towards a wall on its back, it starts moving to a
    /// random side.
    /// </summary>
    /// <param name="ai">Enemy AI.</param>
    private void GetDistantFromPlayer(StateController<Enemy> ai)
    {
        // If the enemy is inside a current attack that needs him to stop, ignores the rest of the method
        if (ai.Controller.IsAttackingWithStoppingTime)
        {
            return;
        }

        // If the agent has reached its final destination
        if (ai.Controller.CurrentTarget != null)
        {
            if (Vector3.Distance(
                ai.Controller.transform.position, ai.Controller.CurrentTarget.position) <
                ai.Controller.CurrentlySelectedSpell.Range * 0.75f)
            {
                ai.Controller.WalkingBackwards = true;

                // If back is blocked
                Ray rayToback = 
                    new Ray(ai.Controller.transform.position + ai.Controller.transform.forward, -ai.Controller.transform.forward);
                if (Physics.Raycast(rayToback, 3, Layers.WallsFloor))
                {
                    // Creates a ray to a random side
                    Ray rayToSide;
                    if (ai.Controller.DirectionIfBackBlocked == Direction.Right)
                        rayToSide = new Ray(ai.Controller.transform.position + ai.Controller.transform.right,
                            -ai.Controller.transform.right);
                    else
                        rayToSide = new Ray(ai.Controller.transform.position - ai.Controller.transform.right,
                            ai.Controller.transform.right);

                    // Checks a ray to a random side
                    // and sets a new direction to move
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
                        // If there's nothing blocking the random side, moves the enemy towards that side
                        // else it will move to the opposite side
                        if (ai.Controller.DirectionIfBackBlocked == Direction.Right)
                            ai.Controller.Agent.SetDestination(ai.Controller.transform.position + ai.Controller.transform.right);
                        else
                            ai.Controller.Agent.SetDestination(ai.Controller.transform.position - ai.Controller.transform.right);
                    }
                }
                else
                {
                    // If there's nothing blocking the enemy back, moves backwards normally
                    ai.Controller.Agent.SetDestination(ai.Controller.transform.position + 
                        ai.Controller.CurrentTarget.Direction(ai.Controller.transform.position));
                }

                // Keeps updating variables
                ai.Controller.Agent.angularSpeed = 0;
                ai.Controller.TimeStoppedWalkingBackwards = Time.time;
            }
            else
            {
                // If the enemy is at a valid distance
                // updates variables accordingly
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

        // Random side direction in case the back path is blocked
        ai.Controller.DirectionIfBackBlocked = Random.Range(0, 2) == 0 ?
            Direction.Right : Direction.Left;
    }

    public override void OnExit(StateController<Enemy> ai)
    {
        ai.Controller.WalkingBackwards = false;
        ai.Controller.Agent.angularSpeed = ai.Controller.Values.AgentAngularSpeed;
    }
}
