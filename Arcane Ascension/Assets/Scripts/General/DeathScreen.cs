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

    public void FindPlayer()
    {
        // Left blank on purpose
    }

    public void PlayerLost()
    {
        canvasToActivate.SetActive(true);
        input.SwitchActionMapToUI();
    }

    public void FindInput()
    {
        input = FindObjectOfType<PlayerInputCustom>();
    }

    public void LostInput()
    {
        // Left blank on purpose
    }
}
