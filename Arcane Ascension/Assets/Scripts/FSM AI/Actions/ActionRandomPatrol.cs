using UnityEngine;

/// <summary>
/// Action for random patrol.
/// </summary>
[CreateAssetMenu(menuName = "FSM/Actions/Action Random Patrol", fileName = "Action Random Patrol")]
sealed public class ActionRandomPatrol : FSMAction
{
    [RangeMinMax(0.5f, 10f)][SerializeField] private Vector2 waitingTimeRange;
    [RangeMinMax(1f, 20f)] [SerializeField] private Vector2 distanceRange;

    private float waitingTime;
    private float distance;
    private float timePointReached;
    private bool reachedPoint;

    private void OnEnable()
    {
        reachedPoint = true;
    }

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
            if (reachedPoint)
            {
                distance = Random.Range(distanceRange.x, distanceRange.y);
                waitingTime = Random.Range(waitingTimeRange.x, waitingTimeRange.y);
                reachedPoint = false;
                timePointReached = Time.time;
            }

            if (Time.time - timePointReached > waitingTime)
            {
                Vector3 finalDestination =
                    new Vector3(Random.Range(-distance, distance), 0, Random.Range(-distance, distance));

                aiCharacter.Agent.SetDestination(aiCharacter.transform.position + finalDestination);

                reachedPoint = true;
            }
        }
    }

    public override void OnEnter(StateController aiCharacter)
    {
        aiCharacter.Agent.isStopped = false;

        distance = Random.Range(distanceRange.x, distanceRange.y);
        waitingTime = Random.Range(waitingTimeRange.x, waitingTimeRange.y);
        reachedPoint = false;
        timePointReached = Time.time;
    }

    public override void OnExit(StateController aiCharacter)
    {
        // Left blank on purpose
    }
}
