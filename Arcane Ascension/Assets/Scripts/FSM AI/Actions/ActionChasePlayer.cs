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
        if (aiCharacter.CurrentTarget != null)
        {
            aiCharacter.transform.LookAtYLerp(aiCharacter.CurrentTarget, ROTATIONSPEED);
        }

        // If the agent has reached its final destination
        if (aiCharacter.CurrentTarget != null)
        {
            if (Vector3.Distance(aiCharacter.transform.position, aiCharacter.CurrentTarget.position) < distanceFromTarget)
            {
                aiCharacter.Agent.isStopped = true;
            }
            else
            {
                aiCharacter.Agent.isStopped = false;
                aiCharacter.Agent.SetDestination(aiCharacter.CurrentTarget.transform.position);
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
