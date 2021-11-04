using UnityEngine;

/// <summary>
/// Scritable object responsible for pausing the game and switch controls to abilities UI.
/// </summary>
[CreateAssetMenu(menuName = "Events/Event Pauses To Ability Menu",
    fileName = "Event Pauses To Ability Menu")]
public class EventPauseToAbilityMenuSO : EventAbstractSO
{
    public override void Execute(EventOnInteraction invoker)
    {
        Time.timeScale = 0;
        FindObjectOfType<PlayerInputCustom>().SwitchActionMapToAbilitiesUI();
    }
}
