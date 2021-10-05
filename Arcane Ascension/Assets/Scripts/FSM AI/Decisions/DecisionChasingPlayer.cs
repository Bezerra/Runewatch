using UnityEngine;

/// <summary>
/// Decision that checks if enemy is chasing the player.
/// </summary>
[CreateAssetMenu(menuName = "FSM/Decisions/Decision Chasing Player", fileName = "Decision Chasing Player")]
public class DecisionChasingPlayer : FSMDecision
{
    public override bool CheckDecision(StateController aiController)
    {
        bool hasTarget = aiController.CurrentTarget != null ? true : false;
        return hasTarget;
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
