using UnityEngine;

/// <summary>
/// Action to roll.
/// </summary>
[CreateAssetMenu(menuName = "FSM/Actions/Action Roll", fileName = "Action Roll")]
sealed public class ActionRoll: FSMAction
{
    public override void Execute(StateController<Enemy> ai)
    {
        Roll(ai);
    }

    /// <summary>
    /// Every X seconds, AI character rolls randomly to the right or to the left.
    /// </summary>
    /// <param name="ai">AI Character.</param>
    private void Roll(StateController<Enemy> ai)
    {
        if (ai.Controller.WalkingBackwards)
            return;

        Vector3 movement = ai.Controller.RollDirection == Direction.Right ?
            ai.Controller.transform.right : -ai.Controller.transform.right;

        if (Time.time - ai.Controller.RollTime > ai.Controller.RollDelay)
        {
            // Checks if it has ground on the movement direction
            if (Physics.OverlapSphere(ai.Controller.transform.position + movement * 3, 1.5f).Length > 0)
            {
                // Moves enemy
                ai.Controller.CharController.Move(
                    ai.Controller.Values.RollMovementQuantity * Time.deltaTime * movement);

                // If rollTime has exceeded the limit
                if (Time.time - ai.Controller.RollTime > 
                    ai.Controller.RollDelay + ai.Controller.Values.RollMovementTime)
                {
                    // Resets roll variables and breaks the loop
                    ai.Controller.RollTime = Time.time;
                    ai.Controller.RollDirection =
                    Random.Range(0, 2) > 0 ? ai.Controller.RollDirection = Direction.Right :
                        ai.Controller.RollDirection = Direction.Left;
                    ai.Controller.RollDelay = Random.Range(
                        ai.Controller.Values.RollDelay.x, ai.Controller.Values.RollDelay.y);
                }
            }
            else // If there is no ground on the movement direction
            {
                // Switches to another direction
                ai.Controller.RollDirection = 
                    ai.Controller.RollDirection == Direction.Right ?
                    ai.Controller.RollDirection = Direction.Left :
                    ai.Controller.RollDirection = Direction.Right;

                // Resets roll time
                ai.Controller.RollTime = Time.time;
            }
        }
    }

    /// <summary>
    /// Resets roll variables.
    /// </summary>
    /// <param name="ai">AI Character.</param>
    public override void OnEnter(StateController<Enemy> ai)
    {
        ai.Controller.RollTime = 0;
        ai.Controller.RollDirection =
            Random.Range(0, 2) > 0 ? ai.Controller.RollDirection = Direction.Right :
            ai.Controller.RollDirection = Direction.Left;
        ai.Controller.RollDelay = Random.Range(
            ai.Controller.Values.RollDelay.x, ai.Controller.Values.RollDelay.y);
    }

    public override void OnExit(StateController<Enemy> ai)
    {
        // Left blank on purpose
    }
}
