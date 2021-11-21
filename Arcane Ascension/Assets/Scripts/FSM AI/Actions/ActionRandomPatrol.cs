using UnityEngine;

/// <summary>
/// Action for random patrol.
/// </summary>
[CreateAssetMenu(menuName = "FSM/Actions/Action Random Patrol", fileName = "Action Random Patrol")]
sealed public class ActionRandomPatrol : FSMAction
{
    /// <summary>
    /// Runs on update.
    /// </summary>
    /// <param name="ai"></param>
    public override void Execute(StateController<Enemy> ai)
    {
        Patrol(ai);
    }

    /// <summary>
    /// Patrols an area with random values.
    /// </summary>
    /// <param name="ai">AI Character.</param>
    private void Patrol(StateController<Enemy> ai)
    {
        // If the agent has reached its final destination
        if (ai.Controller.Agent.remainingDistance <= 
            ai.Controller.Agent.stoppingDistance && 
            !ai.Controller.Agent.pathPending)
        {
            if (ai.Controller.PickingPatrolPosition)
            {
                UpdateVariablesValues(ai);
            }

            if (Time.time - ai.Controller.TimePointReached > ai.Controller.CurrentWaitingTime)
            {
                SetNewDestination(ai);
            }
        }
    }

    /// <summary>
    /// Updates movement variables;
    /// </summary>
    private void UpdateVariablesValues(StateController<Enemy> ai)
    {
        ai.Controller.CurrentDistance = Random.Range(
            ai.Controller.Values.Distance.x, ai.Controller.Values.Distance.y);

        ai.Controller.CurrentWaitingTime = Random.Range(
            ai.Controller.Values.WaitingTime.x, ai.Controller.Values.WaitingTime.y);

        ai.Controller.PickingPatrolPosition = false;
        ai.Controller.TimePointReached = Time.time;
    }

    /// <summary>
    /// Sets new destination with random values.
    /// </summary>
    /// <param name="ai">AI character.</param>
    private void SetNewDestination(StateController<Enemy> ai)
    {
        Vector3 finalDestination =
                    new Vector3(
                        Random.Range(-ai.Controller.CurrentDistance, ai.Controller.CurrentDistance),
                        0,
                        Random.Range(-ai.Controller.CurrentDistance, ai.Controller.CurrentDistance));

        ai.Controller.Agent.SetDestination(ai.Controller.transform.position + finalDestination);

        ai.Controller.PickingPatrolPosition = true;
    }

    public override void OnEnter(StateController<Enemy> ai)
    {
        ai.Controller.Agent.angularSpeed = 500f;
        ai.Controller.Agent.isStopped = false;
        UpdateVariablesValues(ai);
        SetNewDestination(ai);
    }

    public override void OnExit(StateController<Enemy> ai)
    {
        // Left blank on purpose
    }
}
