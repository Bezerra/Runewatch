using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Action to chase player.
/// </summary>
[CreateAssetMenu(menuName = "FSM/Actions/Action Chase Player", fileName = "Action Chase Player")]
sealed public class ActionChasePlayer : FSMAction
{
    public override void Execute(StateController<Enemy> ai)
    {
        ChasePlayer(ai);
    }

    private void ChasePlayer(StateController<Enemy> ai)
    {
        // If the enemy is inside a current attack that needs him to stop, ignores the rest of the method
        if (ai.Controller.WalkingBackwards || ai.Controller.RunningBackwards ||
            ai.Controller.IsAttackingWithStoppingTime)
        {
            return;
        }

        // This will only be executed if the enemy is not walking backwards
        // If the agent has reached its final destination
        if (ai.Controller.CurrentTarget != null)
        {
            if (Vector3.Distance(ai.Controller.transform.position,
                    ai.Controller.CurrentTarget.transform.position) >
                    ai.Controller.CurrentlySelectedSpell.Range + 0.02f)
            {
                ai.Controller.Agent.SetDestination(
                    ai.Controller.CurrentTarget.position +
                    ai.Controller.CurrentTarget.position.Direction(ai.Controller.transform.position) *
                    ai.Controller.CurrentlySelectedSpell.Range);
            }
        }
    }

    public override void OnEnter(StateController<Enemy> ai)
    {
        if (ai.Controller.PlayerScript != null)
        {
            if (ai.Controller.CurrentTarget == null)
                ai.Controller.CurrentTarget = FindObjectOfType<Player>().Eyes.transform;
        }
    }

    public override void OnExit(StateController<Enemy> ai)
    {
        // Left blank on purpose
    }
}
