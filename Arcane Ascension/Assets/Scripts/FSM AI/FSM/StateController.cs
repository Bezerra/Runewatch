using UnityEngine;

/// <summary>
/// Controls AI states. Class used by enemy game object.
/// </summary>
public class StateController<T> where T : Enemy
{
    private readonly float MINIMUMTIMETOSTAYINSTATE;
    private readonly FSMState nullState;
    private FSMState currentState;

    /// <summary>
    /// Character controlling this state machine.
    /// </summary>
    public T Controller { get; private set; }

    /// <summary>
    /// Time elapsed while in current state.
    /// </summary>
    public float StateTimeElapsed { get; private set; }

    /// <summary>
    /// Getter to know if state is running.
    /// </summary>
    public bool StateRunning { get; set; }

    /// <summary>
    /// Constructor for StateController.
    /// </summary>
    /// <param name="character">Character controlling this state machine.</param>
    /// <param name="timeToStayInState">Minimum time to stay in current state.</param>
    public StateController(T character, float timeToStayInState)
    {
        MINIMUMTIMETOSTAYINSTATE = timeToStayInState;
        Controller = character;
        currentState = Controller.AllValues.InitialState;
        nullState = Controller.AllValues.NullState;
    }

    /// <summary>
    /// Starts current state.
    /// </summary>
    /// <param name="ai">StateController of this state machine.</param>
    public void Start(StateController<Enemy> ai)
    {
        StateRunning = true;
    }

    /// <summary>
    /// Updates current state.
    /// </summary>
    /// <param name="ai">StateController of this state machine.</param>
    public void Update(StateController<Enemy> ai)
    {
        if (StateRunning)
        {
            currentState.UpdateState(ai);
            StateTimeElapsed += Time.deltaTime;
        }
    }

    /// <summary>
    /// Changes to a new state.
    /// Starts by running OnEnter for all state actions and decisions (this is used to reset variables if needed),
    /// and state OnEnter. Changes to new State.
    /// Runs OnExit for all state actions and decisions, and state OnExit. 
    /// </summary>
    /// <param name="nextState">Next state to change into.</param>
    /// <param name="ai">StateController of this state machine.</param>
    public void Transition(FSMState nextState, StateController<Enemy> ai)
    {
        if (StateTimeElapsed > MINIMUMTIMETOSTAYINSTATE)
        {
            if (nextState != nullState)
            {
                currentState.OnExit(ai);
                currentState = nextState;
                currentState.OnEnter(ai);
                StateTimeElapsed = 0;
                Debug.Log(currentState);
            }
        }
    }
}
