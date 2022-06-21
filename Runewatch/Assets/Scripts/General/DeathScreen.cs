using UnityEngine;

/// <summary>
/// Class responsible for activating death screen and tracking all information
/// contained on it.
/// </summary>
public class DeathScreen : MonoBehaviour, IFindPlayer, IFindInput
{
    [SerializeField] private GameObject canvasToActivate;
    [SerializeField] private RunStatsLogicSO runStatsLogic;

    private IInput input;
    private EndRunStats endRunStats;

    private void Awake()
    {
        input = FindObjectOfType<PlayerInputCustom>();
        endRunStats = GetComponentInChildren<EndRunStats>(true);
    }

    public void FindPlayer(Player player)
    {
        // Left blank on purpose
    }

    public void PlayerLost(Player player)
    {
        input.SwitchActionMapToUI();
    }

    public void UpdateDeathScreenVariables()
    {
        // Updates all variables
        runStatsLogic.TriggerRunStats(RunStatsType.Accuracy);
        runStatsLogic.TriggerRunStats(RunStatsType.RunTime, value:
            (int)GameplayTime.GameTimer.TotalSeconds);

        // Saves
        runStatsLogic.SaveAchievements();

        // Enables canvas
        canvasToActivate.SetActive(true);

        // Updates text
        endRunStats.UpdateText();   
    }

    public void FindInput(PlayerInputCustom input = null)
    {
        if (input != null)
        {
            this.input = input;
        }
        else
        {
            this.input = FindObjectOfType<PlayerInputCustom>();
        }
    }

    public void LostInput(PlayerInputCustom input = null)
    {
        // Left blank on purpose
    }
}
