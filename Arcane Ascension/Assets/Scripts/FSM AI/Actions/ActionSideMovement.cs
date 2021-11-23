using UnityEngine;

/// <summary>
/// Action to side move.
/// </summary>
[CreateAssetMenu(menuName = "FSM/Actions/Action SideMovement", fileName = "Action SideMovement")]
sealed public class ActionSideMovement: FSMAction
{
    public override void Execute(StateController<Enemy> ai)
    {
        //Roll(ai);
    }

    /// <summary>
    /// Every X seconds, AI character rolls randomly to the right or to the left.
    /// </summary>
    /// <param name="ai">AI Character.</param>
    private void Roll(StateController<Enemy> ai)
    {
        if (ai.Controller.RunningBackwards || 
            ai.Controller.IsAttackingWithStoppingTime)
        {
            return;
        }

        // Moves to the side for X seconds
        if (Time.time - ai.Controller.SideMovingTime < ai.Controller.SideMovementMaxTime)
        {
            Vector3 movement = ai.Controller.SideMovementDirection == Direction.Right ?
                ai.Controller.transform.right : -ai.Controller.transform.right;

            ai.Controller.Agent.SetDestination(
                ai.Controller.Agent.destination + movement);

            return;
        }

        // If delay has passed, it will reset the variables
        if (Time.time - ai.Controller.SideMovingTime >
            ai.Controller.SideMovementMaxTime + ai.Controller.SideMovementDelay)
        {
            ai.Controller.SideMovementDirection =
                Random.Range(0, 2) > 0 ? ai.Controller.SideMovementDirection = Direction.Right :
                ai.Controller.SideMovementDirection = Direction.Left;

            ai.Controller.SideMovementMaxTime = 
                Random.Range(ai.Controller.Values.SideMovementMaxTime.x, 
                ai.Controller.Values.SideMovementMaxTime.y);

            ai.Controller.SideMovingTime = Time.time;
        }
    }

    /// <summary>
    /// Resets roll variables.
    /// </summary>
    /// <param name="ai">AI Character.</param>
    public override void OnEnter(StateController<Enemy> ai)
    {
        ai.Controller.SideMovingTime = 0;
        ai.Controller.SideMovementDirection =
            Random.Range(0, 2) > 0 ? ai.Controller.SideMovementDirection = Direction.Right :
            ai.Controller.SideMovementDirection = Direction.Left;
        ai.Controller.SideMovementDelay = Random.Range(
            ai.Controller.Values.SideMovementDelay.x, ai.Controller.Values.SideMovementDelay.y);
        ai.Controller.SideMovementMaxTime = Random.Range(
            ai.Controller.Values.SideMovementMaxTime.x, ai.Controller.Values.SideMovementMaxTime.y);
    }

    public override void OnExit(StateController<Enemy> ai)
    {
        // Left blank on purpose
    }
}
