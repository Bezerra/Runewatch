using UnityEngine;

/// <summary>
/// Action to roll.
/// </summary>
[CreateAssetMenu(menuName = "FSM/Actions/Action Roll", fileName = "Action Roll")]
sealed public class ActionRoll: FSMAction
{
    public override void Execute(StateController aiCharacter)
    {
        Roll(aiCharacter);
    }

    /// <summary>
    /// Every X seconds, AI character rolls randomly to the right or to the left.
    /// </summary>
    /// <param name="aiCharacter">AI Character.</param>
    private void Roll(StateController aiCharacter)
    {
        Vector3 movement = aiCharacter.EnemyScript.RollDirection == Direction.Right ?
            aiCharacter.EnemyScript.transform.right : -aiCharacter.EnemyScript.transform.right;

        if (Time.time - aiCharacter.EnemyScript.RollTime > aiCharacter.EnemyScript.RollDelay)
        {
            // Checks if it has ground on the movement direction
            if (Physics.OverlapSphere(aiCharacter.EnemyScript.transform.position + movement * 3, 1.5f).Length > 0)
            {
                // Moves enemy
                aiCharacter.EnemyScript.Controller.Move(
                    movement * aiCharacter.EnemyScript.Values.RollMovementQuantity * Time.deltaTime);

                // If rollTime has exceeded the limit
                if (Time.time - aiCharacter.EnemyScript.RollTime > 
                    aiCharacter.EnemyScript.RollDelay + aiCharacter.EnemyScript.Values.RollMovementTime)
                {
                    // Resets roll variables and breaks the loop
                    aiCharacter.EnemyScript.RollTime = Time.time;
                    aiCharacter.EnemyScript.RollDirection = 
                        Random.Range(0, 2) > 0 ? aiCharacter.EnemyScript.RollDirection = Direction.Right : 
                        aiCharacter.EnemyScript.RollDirection = Direction.Left;
                    aiCharacter.EnemyScript.RollDelay = Random.Range(
                        aiCharacter.EnemyScript.Values.RollDelay.x, aiCharacter.EnemyScript.Values.RollDelay.y);
                }
            }
            else // If there is no ground on the movement direction
            {
                // Switches to another direction
                aiCharacter.EnemyScript.RollDirection = 
                    aiCharacter.EnemyScript.RollDirection == Direction.Right ?
                    aiCharacter.EnemyScript.RollDirection = Direction.Left :
                    aiCharacter.EnemyScript.RollDirection = Direction.Right;

                // Resets roll time
                aiCharacter.EnemyScript.RollTime = Time.time;
            }
        }
    }

    /// <summary>
    /// Resets roll variables.
    /// </summary>
    /// <param name="aiCharacter">AI Character.</param>
    public override void OnEnter(StateController aiCharacter)
    {
        aiCharacter.EnemyScript.RollTime = 0;
        aiCharacter.EnemyScript.RollDirection = 
            Random.Range(0, 2) > 0 ? aiCharacter.EnemyScript.RollDirection = Direction.Right : 
            aiCharacter.EnemyScript.RollDirection = Direction.Left;
        aiCharacter.EnemyScript.RollDelay = Random.Range(
            aiCharacter.EnemyScript.Values.RollDelay.x, aiCharacter.EnemyScript.Values.RollDelay.y);
    }

    public override void OnExit(StateController aiCharacter)
    {
        // Left blank on purpose
    }
}
