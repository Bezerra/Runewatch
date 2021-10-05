using UnityEngine;

/// <summary>
/// Action to roll.
/// </summary>
[CreateAssetMenu(menuName = "FSM/Actions/Action Roll", fileName = "Action Roll")]
sealed public class ActionRoll: FSMAction
{
    [Header("Roll")]
    [Tooltip("How much does the enemy move")]
    [Range(1, 25f)] [SerializeField] private float rollMovementQuantity;
    [Tooltip("How long does the roll last")]
    [Range(0.01f, 0.5f)] [SerializeField] private float rollMovementTime;
    [Tooltip("Roll every X seconds (X is defined by a random number between these values")]
    [RangeMinMax(3, 15f)] [SerializeField] private Vector2 rollDelay;

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
            aiCharacter.transform.right : -aiCharacter.transform.right;

        if (Time.time - aiCharacter.EnemyScript.RollTime > aiCharacter.EnemyScript.RollDelay)
        {
            // Checks if it has ground on the movement direction
            if (Physics.OverlapSphere(aiCharacter.transform.position + movement * 3, 1.5f).Length > 0)
            {
                // Moves enemy
                aiCharacter.EnemyScript.Controller.Move(
                    movement * rollMovementQuantity * Time.deltaTime);

                // If rollTime has exceeded the limit
                if (Time.time - aiCharacter.EnemyScript.RollTime > 
                    aiCharacter.EnemyScript.RollDelay + rollMovementTime)
                {
                    // Resets roll variables and breaks the loop
                    aiCharacter.EnemyScript.RollTime = Time.time;
                    aiCharacter.EnemyScript.RollDirection = 
                        Random.Range(0, 2) > 0 ? aiCharacter.EnemyScript.RollDirection = Direction.Right : 
                        aiCharacter.EnemyScript.RollDirection = Direction.Left;
                    aiCharacter.EnemyScript.RollDelay = Random.Range(
                        rollDelay.x, rollDelay.y);
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
            rollDelay.x, rollDelay.y);
    }

    public override void OnExit(StateController aiCharacter)
    {
        // Left blank on purpose
    }
}
