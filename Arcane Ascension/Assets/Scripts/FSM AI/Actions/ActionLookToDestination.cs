using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Action to chase player.
/// </summary>
[CreateAssetMenu(menuName = "FSM/Actions/Action Look To Destination", fileName = "Action Look To Destination")]
sealed public class ActionLookToDestination : FSMAction
{
    public override void Execute(StateController<Enemy> ai)
    {
        RotateToDestination(ai);
    }

    /// <summary>
    /// Rotates towards destination
    /// </summary>
    /// <param name="ai"></param>
    private void RotateToDestination(StateController<Enemy> ai)
    {
        Vector3 direction = ai.Controller.transform.Direction(ai.Controller.Agent.destination);
        Quaternion finalDirection = Quaternion.LookRotation(direction);
            ai.Controller.transform.rotation = Quaternion.Lerp(
            ai.Controller.transform.rotation,
            finalDirection, Time.deltaTime * ai.Controller.Values.RotationSpeed * 2f);
    }

    public override void OnEnter(StateController<Enemy> ai)
    {
        // Left blank on purpose
    }

    public override void OnExit(StateController<Enemy> ai)
    {
        // Left blank on purpose
    }
}
