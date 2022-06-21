using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Decisions/Decision TRUE", fileName = "Decision TRUE")]
public class DecisionTRUE : FSMDecision
{
    public override bool CheckDecision(StateController<Enemy> ai)
    {
        return true;
    }

    public override void OnEnter(StateController<Enemy> ai)
    {
        // Left blank on purpose
    }

    public override void OnExit(StateController<Enemy> ai)
    {
        // Left blank on purpose
    }
}
