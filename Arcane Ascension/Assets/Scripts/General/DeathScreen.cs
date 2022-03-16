using UnityEngine;

/// <summary>
/// Class responsible for activating death screen and tracking all information
/// contained on it.
/// </summary>
public class DeathScreen : MonoBehaviour, IFindPlayer, IFindInput
{
    [SerializeField] private GameObject canvasToActivate;

    private IInput input;

    private void Awake()
    {
        input = FindObjectOfType<PlayerInputCustom>();
    }

    public void FindPlayer(Player player)
    {
        // Left blank on purpose
    }

    public void PlayerLost(Player player)
    {
        canvasToActivate.SetActive(true);
        input.SwitchActionMapToUI();
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
