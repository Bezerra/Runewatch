using UnityEngine;

/// <summary>
/// Scritable object responsible for executing animation trigger.
/// </summary>
[CreateAssetMenu(menuName = "Events/Event Execute Animation Trigger",
    fileName = "Event Execute Animation Trigger")]
public class EventExecuteAnimationTriggerSO : EventAbstractSO
{
    [SerializeField] private string triggerName = "Execute";

    public override void Execute(AbstractEventOnInteraction invoker, PlayerInteraction interactor = null)
    {
        invoker.GetComponentInChildren<Animator>().SetTrigger(triggerName);
    }
}
