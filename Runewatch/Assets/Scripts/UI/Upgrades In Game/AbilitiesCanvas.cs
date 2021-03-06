using System.Collections;
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

    private GameObject objectThatActivatedThisCanvas;
    private GameObject orbThatActivatedThisCanvas;

    private void Awake() =>
        input = FindObjectOfType<PlayerInputCustom>();

    public void EnableThreeSpellCanvas(GameObject objectThatActivatedThisCanvas)
    {
        this.objectThatActivatedThisCanvas = objectThatActivatedThisCanvas;
        threeSpellCanvas.SetActive(true);
    }

    public void EnableOneSpellCanvas(GameObject objectThatActivatedThisCanvas)
    {
        this.objectThatActivatedThisCanvas = objectThatActivatedThisCanvas;
        oneSpellCanvas.SetActive(true);
    }

    public void EnableThreePassiveCanvas(GameObject orbThatActivatedThisCanvas)
    {
        this.orbThatActivatedThisCanvas = orbThatActivatedThisCanvas;
        threePassiveCanvas.SetActive(true);
    }

    public void DisableAll()
    {
        // Deactivates spell scrolls only, not chests
        if (objectThatActivatedThisCanvas != null)
        {
            if (objectThatActivatedThisCanvas.TryGetComponent(out SpellScroll spellScroll))
            {
                if (objectThatActivatedThisCanvas.activeSelf)
                {
                    objectThatActivatedThisCanvas.SetActive(false);
                }   
            }
        }
        
        // Deactivates orbs
        if (orbThatActivatedThisCanvas != null)
        {
            if (orbThatActivatedThisCanvas.TryGetComponent(out PassiveOrb passiveOrb))
            {
                if (orbThatActivatedThisCanvas.activeSelf)
                {
                    orbThatActivatedThisCanvas.SetActive(false);
                }
            }
            
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
        //EnableAll();

        //threeSpellCanvas.SetActive(false);
        //oneSpellCanvas.SetActive(false);
        //threePassiveCanvas.SetActive(false);
        //spellsFullCanvas.SetActive(false);

        StartCoroutine(UpdateAllMenuVariables());
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

    public IEnumerator UpdateAllMenuVariables()
    {

        ThreeSpellEnable();
        OneSpellCanvasEnable();
        ThreePassiveCanvasEnable();
        spellsFullCanvasEnable();


        yield return null;

        ThreeSpellDisable();
        OneSpellCanvasDisable();
        ThreePassiveCanvasDisable();
        spellsFullCanvasDisable();

    }
    public void ThreeSpellEnable() => threeSpellCanvas.SetActive(true);
    public void OneSpellCanvasEnable() => oneSpellCanvas.SetActive(true);
    public void ThreePassiveCanvasEnable() => threePassiveCanvas.SetActive(true);
    public void spellsFullCanvasEnable() => spellsFullCanvas.SetActive(true);

    public void ThreeSpellDisable() => threeSpellCanvas.SetActive(false);
    public void OneSpellCanvasDisable() => oneSpellCanvas.SetActive(false);
    public void ThreePassiveCanvasDisable() => threePassiveCanvas.SetActive(false);
    public void spellsFullCanvasDisable() => spellsFullCanvas.SetActive(false);


    public void LostInput(PlayerInputCustom input)
    {
        // Left blank on purpose
    }
}
