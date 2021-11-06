using UnityEngine;

/// <summary>
/// Scritable object responsible for opening three passives canvas.
/// </summary>
[CreateAssetMenu(menuName = "Events/Event Open Three Passives Menu",
    fileName = "Event Open Three Passives Menu")]
public class EventOpenThreePassivesMenuSO : EventAbstractSO
{
    public override void Execute(EventOnInteraction invoker)
    {
        FindObjectOfType<AbilitiesCanvas>().EnableThreePassiveCanvas();
    }
}
