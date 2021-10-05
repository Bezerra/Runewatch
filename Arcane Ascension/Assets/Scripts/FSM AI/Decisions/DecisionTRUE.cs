using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Decisions/Decision TRUE", fileName = "Decision TRUE")]
public class DecisionTRUE : FSMDecision
{
    public override bool CheckDecision(StateController aiController)
    {
        return true;
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
