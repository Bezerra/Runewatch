using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Controls AI states. Class used by enemy game object.
/// </summary>
public class StateController : MonoBehaviour
{
    private const float MINIMUMTIMETOSTAYINSTATE = 2f;

    private FSMState currentState;
    private FSMState nullState;

    // Components needed to control AI on states
    public NavMeshAgent Agent { get; private set; }
    public Enemy EnemyScript { get; private set; }
    public Transform CurrentTarget { get; set; }

    // Time elapsed on state
    public float StateTimeElapsed { get; private set; }

    // Variable to confirm if state can run
    public bool StateRunning { get; set; }

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        EnemyScript = GetComponent<Enemy>();
        currentState = EnemyScript.AllValues.InitialState;
        nullState = EnemyScript.AllValues.NullState;
    }

    private void Start()
    {
        StateRunning = true;
    }

    private void Update()
    {
        if (StateRunning)
        {
            currentState.UpdateState(this);
            StateTimeElapsed += Time.deltaTime;
        }
    }

    /// <summary>
    /// Changes to a new state.
    /// Starts by running OnEnter for all state actions and decisions (this is used to reset variables if needed),
    /// and state OnEnter. Changes to new State.
    /// Runs OnExit for all state actions and decisions, and state OnExit. 
    /// </summary>
    /// <param name="nextState"></param>
    public void Transition(FSMState nextState)
    {
        if (StateTimeElapsed > MINIMUMTIMETOSTAYINSTATE)
        {
            if (nextState != nullState)
            {
                currentState.OnExit(this);
                currentState = nextState;
                currentState.OnEnter(this);
                StateTimeElapsed = 0;
            }
        }
    }
}
