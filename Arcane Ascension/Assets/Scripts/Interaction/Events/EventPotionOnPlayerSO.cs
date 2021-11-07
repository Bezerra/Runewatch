using UnityEngine;

/// <summary>
/// Scritable object responsible for pausing the game and switch controls to abilities UI.
/// </summary>
[CreateAssetMenu(menuName = "Events/Event Potion on Player",
    fileName = "Event Potion on Player")]
public class EventPotionOnPlayerSO : EventAbstractSO
{
    [SerializeField] private PotionSO potion;

    public override void Execute(AbstractEventOnInteraction invoker)
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();

        if (potion.PotionType == PotionType.Health)
        {
            playerStats.Heal(potion.Percentage * playerStats.MaxHealth / 100, StatsType.Health);
        }
        else
        {
            playerStats.Heal(potion.Percentage * playerStats.MaxMana / 100, StatsType.Mana);
        }
    }
}
