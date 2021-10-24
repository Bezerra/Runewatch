using UnityEngine;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Scriptable object for all states.
/// </summary>
[CreateAssetMenu(menuName = "FSM/States/FSM State", fileName = "State STATENAME")]
[InlineEditor]
public class FSMState : ScriptableObject
{
#if UNITY_EDITOR
    [InlineButton("ChangeFileName", "Update File Name")]
    [SerializeField] private string fileName;

    private void ChangeFileName()
    {
        string assetPath = AssetDatabase.GetAssetPath(this.GetInstanceID());
        AssetDatabase.RenameAsset(assetPath, fileName);
        AssetDatabase.SaveAssets();
    }
#endif

    [SerializeField] [TextArea(3, 3)] private string notes;
    [PropertySpace(30)]

    [SerializeField] private FSMAction[] actions;
    [SerializeField] private FSMTransition[] transitions;
    [SerializeField] private FSMAction[] onEnterStateActions;
    [SerializeField] private FSMAction[] onExitStateActions;

    /// <summary>
    /// Runs every update.
    /// </summary>
    /// <param name="ai">Ai Character.</param>
    public void UpdateState(StateController<Enemy> ai)
    {
        ExecuteActions(ai);
        CheckTransitions(ai);
    }

    /// <summary>
    /// Executes all actions.
    /// </summary>
    /// <param name="ai">Ai Character.</param>
    private void ExecuteActions(StateController<Enemy> ai)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            if (actions[i] != null) actions[i].Execute(ai);
        }
    }

    /// <summary>
    /// Checks all transitions. If a decision from a transition is true, it will change the
    /// current fsm state to the desired state of the transition.
    /// </summary>
    /// <param name="ai">Ai Character.</param>
    private void CheckTransitions(StateController<Enemy> ai)
    {
        for (int i = 0; i < transitions.Length; i++)
        {
            if (transitions[i] != null)
            {
                bool decisionSuccess = transitions[i].Decision.CheckDecision(ai);

                if (decisionSuccess)
                {
                    ai.Transition(transitions[i].IfTrue, ai);
                }
                else
                {
                    ai.Transition(transitions[i].IfFalse, ai);
                }
            }
        }
    }

    /// <summary>
    /// Runs once when entering this state.
    /// </summary>
    /// <param name="ai">Ai Character.</param>
    public void OnEnter(StateController<Enemy> ai)
    {
        for (int i = 0; i < onEnterStateActions.Length; i++)
        {
            if (onEnterStateActions[i] != null) onEnterStateActions[i].Execute(ai);
        }

        for (int i = 0; i < actions.Length; i++)
        {
            if (actions[i] != null) actions[i].OnEnter(ai);
        }

        for (int i = 0; i < transitions.Length; i++)
        {
            if (transitions[i] != null) transitions[i].Decision.OnEnter(ai);
        }
    }

    /// <summary>
    /// Runs once when leaving this state.
    /// </summary>
    /// <param name="ai">Ai Character.</param>
    public void OnExit(StateController<Enemy> ai)
    {
        for (int i = 0; i < onExitStateActions.Length; i++)
        {
            if (onExitStateActions[i] != null) onExitStateActions[i].Execute(ai);
        }

        for (int i = 0; i < actions.Length; i++)
        {
            if (actions[i] != null) actions[i].OnExit(ai);
        }

        for (int i = 0; i < transitions.Length; i++)
        {
            if (transitions[i] != null) transitions[i].Decision.OnExit(ai);
        }
    }
}
