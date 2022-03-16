using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Scritable object responsible for rotating an object and executing animation trigger
/// after the object has the rotation of the player.
/// </summary>
[CreateAssetMenu(menuName = "Events/Event Rotate And Execute Animation Trigger",
    fileName = "Event Rotate And Execute Animation Trigger")]
public class EventRotateAndExecuteAnimationTriggerSO : EventAbstractSO
{
    [SerializeField] private string triggerName = "Execute";

    public override void Execute(AbstractEventOnInteraction invoker, 
        PlayerInteraction interactor = null)
    {
        invoker.transform.RotateTo(interactor.transform.position);
        invoker.GetComponentInChildren<Animator>().SetTrigger(triggerName);
    }
}
