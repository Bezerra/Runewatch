using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Decision that checks if the enemy has been in this state for x seconds.
/// </summary>
[CreateAssetMenu(menuName = "FSM/Decisions/Decision Max Time In State When Stopped", 
    fileName = "Decision Max Time In State When Stopped")]
sealed public class DecisionMaxTimeInStateWhenStopped : FSMDecision
{
    public override bool CheckDecision(StateController<Enemy> ai)
    {
        return RemainInState(ai);
    }

    /// <summary>
    /// Remains in this state for a random value depending on the enemy waiting time.
    /// </summary>
    /// <param name="ai">AI Controller.</param>
    /// <returns>True if it detects a player without a wall in front.</returns>
    private bool RemainInState(StateController<Enemy> ai)
    {
        if (ai.Controller.Agent.remainingDistance < 2f)
            ai.Controller.TimePointReached += Time.deltaTime;
        else
            ai.Controller.TimePointReached = 0;

        if (ai.Controller.TimePointReached > ai.Controller.CurrentWaitingTime)
            return true;

        return false;
    }

    public override void OnEnter(StateController<Enemy> ai)
    {
        ai.Controller.TimePointReached = 0;

        ai.Controller.CurrentWaitingTime = Random.Range(
            ai.Controller.Values.WaitingTime.x, ai.Controller.Values.WaitingTime.y);
    }

    public override void OnExit(StateController<Enemy> ai)
    {
        // Left blank on purpose
    }
}
