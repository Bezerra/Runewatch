using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Action to chase player.
/// </summary>
[CreateAssetMenu(menuName = "FSM/Actions/Action Chase Player", fileName = "Action Chase Player")]
sealed public class ActionChasePlayer : FSMAction
{
    [Range(1, 15)] [SerializeField] private float distanceFromTarget;

    private readonly float ROTATIONSPEED = 3;

    public override void Execute(StateController aiCharacter)
    {
        ChasePlayer(aiCharacter);
    }

    private void ChasePlayer(StateController aiCharacter)
    {
        if (aiCharacter.EnemyScript.CurrentTarget != null)
        {
            aiCharacter.EnemyScript.transform.LookAtYLerp(aiCharacter.EnemyScript.CurrentTarget, ROTATIONSPEED);
        }

        // If the agent has reached its final destination
        if (aiCharacter.EnemyScript.CurrentTarget != null)
        {
            if (Vector3.Distance(
                aiCharacter.EnemyScript.transform.position, aiCharacter.EnemyScript.CurrentTarget.position) < distanceFromTarget)
            {
                aiCharacter.EnemyScript.Agent.isStopped = true;
            }
            else
            {
                aiCharacter.EnemyScript.Agent.isStopped = false;
                aiCharacter.EnemyScript.Agent.SetDestination(aiCharacter.EnemyScript.CurrentTarget.transform.position);
            }
        }
    }

    public override void OnEnter(StateController aiCharacter)
    {
        // Left blank on purpose
    }

    public override void OnExit(StateController aiCharacter)
    {
        // Left blank on purpose
    }
}
