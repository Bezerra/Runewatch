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
    /// <param name="aiCharacter"></param>
    public override void Execute(StateController<Enemy> aiCharacter)
    {
        Patrol(aiCharacter);
    }

    /// <summary>
    /// Patrols an area with random values.
    /// </summary>
    /// <param name="aiCharacter">AI Character.</param>
    private void Patrol(StateController<Enemy> aiCharacter)
    {
        // If the agent has reached its final destination
        if (aiCharacter.Controller.Agent.remainingDistance <= 
            aiCharacter.Controller.Agent.stoppingDistance && 
            !aiCharacter.Controller.Agent.pathPending)
        {
            if (aiCharacter.Controller.ReachedPoint)
            {
                UpdateVariablesValues(aiCharacter);
            }

            if (Time.time - aiCharacter.Controller.TimePointReached > aiCharacter.Controller.WaitingTime)
            {
                Vector3 finalDestination =
                    new Vector3(
                        Random.Range(-aiCharacter.Controller.Distance, aiCharacter.Controller.Distance), 
                        0, 
                        Random.Range(-aiCharacter.Controller.Distance, aiCharacter.Controller.Distance));

                aiCharacter.Controller.Agent.SetDestination(aiCharacter.Controller.transform.position + finalDestination);

                aiCharacter.Controller.ReachedPoint = true;
            }
        }
    }

    /// <summary>
    /// Updates movement variables;
    /// </summary>
    private void UpdateVariablesValues(StateController<Enemy> aiCharacter)
    {
        aiCharacter.Controller.Distance = Random.Range(
            aiCharacter.Controller.Values.Distance.x, aiCharacter.Controller.Values.Distance.y);

        aiCharacter.Controller.WaitingTime = Random.Range(
            aiCharacter.Controller.Values.WaitingTime.x, aiCharacter.Controller.Values.WaitingTime.y);

        aiCharacter.Controller.ReachedPoint = false;
        aiCharacter.Controller.TimePointReached = Time.time;
    }

    public override void OnEnter(StateController<Enemy> aiCharacter)
    {
        aiCharacter.Controller.Agent.isStopped = false;
        UpdateVariablesValues(aiCharacter);
    }

    public override void OnExit(StateController<Enemy> aiCharacter)
    {
        // Left blank on purpose
    }
}
