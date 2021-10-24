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
    public override void Execute(StateController aiCharacter)
    {
        Patrol(aiCharacter);
    }

    /// <summary>
    /// Patrols an area with random values.
    /// </summary>
    /// <param name="aiCharacter">AI Character.</param>
    private void Patrol(StateController aiCharacter)
    {
        // If the agent has reached its final destination
        if (aiCharacter.Agent.remainingDistance <= aiCharacter.Agent.stoppingDistance && !aiCharacter.Agent.pathPending)
        {
            if (aiCharacter.EnemyScript.ReachedPoint)
            {
                UpdateVariablesValues(aiCharacter);
            }

            if (Time.time - aiCharacter.EnemyScript.TimePointReached > aiCharacter.EnemyScript.WaitingTime)
            {
                Vector3 finalDestination =
                    new Vector3(
                        Random.Range(-aiCharacter.EnemyScript.Distance, aiCharacter.EnemyScript.Distance), 
                        0, 
                        Random.Range(-aiCharacter.EnemyScript.Distance, aiCharacter.EnemyScript.Distance));

                aiCharacter.Agent.SetDestination(aiCharacter.transform.position + finalDestination);

                aiCharacter.EnemyScript.ReachedPoint = true;
            }
        }
    }

    /// <summary>
    /// Updates movement variables;
    /// </summary>
    private void UpdateVariablesValues(StateController aiCharacter)
    {
        aiCharacter.EnemyScript.Distance = Random.Range(
            aiCharacter.EnemyScript.Values.Distance.x, aiCharacter.EnemyScript.Values.Distance.y);

        aiCharacter.EnemyScript.WaitingTime = Random.Range(
            aiCharacter.EnemyScript.Values.WaitingTime.x, aiCharacter.EnemyScript.Values.WaitingTime.y);

        aiCharacter.EnemyScript.ReachedPoint = false;
        aiCharacter.EnemyScript.TimePointReached = Time.time;
    }

    public override void OnEnter(StateController aiCharacter)
    {
        aiCharacter.Agent.isStopped = false;
        UpdateVariablesValues(aiCharacter);
    }

    public override void OnExit(StateController aiCharacter)
    {
        // Left blank on purpose
    }
}
