using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Decision that checks if the enemy has been in this state for x seconds.
/// </summary>
[CreateAssetMenu(menuName = "FSM/Decisions/Decision Max Time In State When Stopped", 
    fileName = "Decision Max Time In State When Stopped")]
sealed public class DecisionMaxTimeInStateWhenStopped : FSMDecision
{
    [SerializeField] private float timeToStayInState;

    public override bool CheckDecision(StateController<Enemy> ai)
    {
        return RemainInState(ai);
    }

    /// <summary>
    /// Looks for player.
    /// </summary>
    /// <param name="ai">AI Controller.</param>
    /// <returns>True if it detects a player without a wall in front.</returns>
    private bool RemainInState(StateController<Enemy> ai)
    {
        if (ai.Controller.Agent.remainingDistance < 2f)
            ai.Controller.TimeWhenReachedFinalPosition += Time.deltaTime;
        else
            ai.Controller.TimeWhenReachedFinalPosition = 0;

        if (ai.Controller.TimeWhenReachedFinalPosition > timeToStayInState)
            return true;

        return false;
    }

    public override void OnEnter(StateController<Enemy> ai)
    {
        ai.Controller.TimeWhenReachedFinalPosition = 0;
    }

    public override void OnExit(StateController<Enemy> ai)
    {
        // Left blank on purpose
    }
}
