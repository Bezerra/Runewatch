using UnityEngine;

/// <summary>
/// Class responsible for enabling child ability canvas.
/// </summary>
public class AbilitiesCanvas : MonoBehaviour, IFindInput
{
    // Components
    private IInput input;

    [SerializeField] private GameObject threeSpellCanvas;
    [SerializeField] private GameObject oneSpellCanvas;
    [SerializeField] private GameObject threePassiveCanvas;
    [SerializeField] private GameObject spellsFullCanvas;

    private GameObject bookThatActivatedThisCanvas;
    private GameObject orbThatActivatedThisCanvas;

    private void Awake() =>
        input = FindObjectOfType<PlayerInputCustom>();

    public void EnableThreeSpellCanvas(GameObject bookThatActivatedThisCanvas)
    {
        this.bookThatActivatedThisCanvas = bookThatActivatedThisCanvas;
        threeSpellCanvas.SetActive(true);
    }

    public void EnableOneSpellCanvas() =>
        oneSpellCanvas.SetActive(true);

    public void EnableThreePassiveCanvas(GameObject orbThatActivatedThisCanvas)
    {
        this.orbThatActivatedThisCanvas = orbThatActivatedThisCanvas;
        threePassiveCanvas.SetActive(true);
    }

    public void DisableAll()
    {
        if (bookThatActivatedThisCanvas != null)
        {
            if (bookThatActivatedThisCanvas.activeSelf)
                bookThatActivatedThisCanvas.SetActive(false);
        }
        
        if (orbThatActivatedThisCanvas != null)
        {
            if (orbThatActivatedThisCanvas.activeSelf)
                orbThatActivatedThisCanvas.SetActive(false);
        }

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

        threeSpellCanvas.SetActive(false);
        oneSpellCanvas.SetActive(false);
        threePassiveCanvas.SetActive(false);
        spellsFullCanvas.SetActive(false);
    }

    public void FindInput(PlayerInputCustom input)
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

    public void LostInput(PlayerInputCustom input)
    {
        // Left blank on purpose
    }
}
