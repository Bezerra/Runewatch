using UnityEngine;

/// <summary>
/// Action to get player's target.
/// </summary>
[CreateAssetMenu(menuName = "FSM/Actions/Action Get Player Target", 
    fileName = "Action Get Player Target")]
sealed public class ActionGetPlayerTarget : FSMAction
{
    public override void Execute(StateController<Enemy> ai)
    {
        // Left blank on purpose
    }

    public override void OnEnter(StateController<Enemy> ai)
    {
        if (ai.Controller.PlayerScript != null)
            ai.Controller.CurrentTarget = ai.Controller.PlayerScript.Eyes.transform;

        if (ai.Controller.CurrentTarget == null)
            ai.Controller.CurrentTarget = FindObjectOfType<Player>().Eyes.transform;
    }

    public override void OnExit(StateController<Enemy> ai)
    {
        ai.Controller.CurrentTarget = ai.Controller.PlayerScript.Eyes.transform;

        if (ai.Controller.CurrentTarget == null)
            ai.Controller.CurrentTarget = FindObjectOfType<Player>().Eyes.transform;
    }
}
