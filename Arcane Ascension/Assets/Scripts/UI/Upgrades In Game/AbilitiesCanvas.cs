using UnityEngine;

/// <summary>
/// Class responsible for enabling child ability canvas.
/// </summary>
public class AbilitiesCanvas : MonoBehaviour
{
    // Components
    private PlayerInputCustom input;

    [SerializeField] private GameObject threeSpellCanvas;
    [SerializeField] private GameObject oneSpellCanvas;
    [SerializeField] private GameObject threePassiveCanvas;
    [SerializeField] private GameObject spellsFullCanvas;

    private void Awake()
    {
        input = FindObjectOfType<PlayerInputCustom>();
    }

    public void EnableThreeSpellCanvas() =>
        threeSpellCanvas.SetActive(true);

    public void EnableOneSpellCanvas() =>
        oneSpellCanvas.SetActive(true);

    public void EnableThreePassiveCanvas() =>
        threePassiveCanvas.SetActive(true);

    public void DisableAll()
    {
        threeSpellCanvas.SetActive(false);
        oneSpellCanvas.SetActive(false);
        threePassiveCanvas.SetActive(false);
        spellsFullCanvas.SetActive(false);
        input.SwitchActionMapToGameplay();
        Time.timeScale = 1;
    }

    private void EnableAll()
    {
        threeSpellCanvas.SetActive(true);
        oneSpellCanvas.SetActive(true);
        threePassiveCanvas.SetActive(true);
        spellsFullCanvas.SetActive(true);
    }

    /// <summary>
    /// Enable and disable fixes a bug on build, where cards weren't updating on the first interaction.
    /// </summary>
    private void Start()
    {
        EnableAll();
        DisableAll();
    }
}
