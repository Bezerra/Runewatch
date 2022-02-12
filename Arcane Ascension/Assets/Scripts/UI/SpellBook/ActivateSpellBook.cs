using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>
/// Class responsible for activating and deactivating spellbook.
/// </summary>
public class ActivateSpellBook : MonoBehaviour, IFindInput, IFindPlayer
{
    [SerializeField] private GameObject spellbook;
    [SerializeField] private GameObject middleColumnSpellCard;
    [SerializeField] private GameObject middleColumnPassiveCard;
    [SerializeField] private TextMeshProUGUI spellbookClosingText;

    public IInput Input { get; private set; }
    private bool isSpellBookOpened;

    private SpellBookSpells spellBookSpells;
    private SpellBookPassives spellBookPassives;
    private SpellBookAttributes spellBookAttributes;
    private PlayerSpells playerSpells;
    public ISpell SelectedSpellOnBookOpen { get; private set; }

    private void Awake()
    {
        Input = FindObjectOfType<PlayerInputCustom>();
        spellBookSpells = GetComponent<SpellBookSpells>();
        spellBookPassives = GetComponent<SpellBookPassives>();
        spellBookAttributes = GetComponent<SpellBookAttributes>();
        playerSpells = FindObjectOfType<PlayerSpells>();
    }

    private void OnEnable()
    {
        FindInput();
    }

    private void OnDisable()
    {
        LostInput();
    }

    private void Start()
    {
        isSpellBookOpened = false;
    }

    private void ControlSpellBook()
    {
        isSpellBookOpened = !isSpellBookOpened;

        if (isSpellBookOpened)
        {
            ActionPath actionPath = FindObjectOfType<ActionPath>();
            if (actionPath != null)
                spellbookClosingText.text = "Press " + 
                    actionPath.GetPath(BindingsAction.SpellBook) + " to close";

            Time.timeScale = 0;
            spellbook.SetActive(true);
            spellBookSpells.UpdateSpellSlots();
            spellBookPassives.UpdatePassiveSlots();
            spellBookAttributes.UpdateText();
            Input.SwitchActionMapToSpellBook();
            SelectedSpellOnBookOpen = 
                playerSpells.CurrentSpells[playerSpells.CurrentSpellIndex];
        }
        else
        {
            Time.timeScale = 1;
            middleColumnSpellCard.SetActive(false);
            middleColumnPassiveCard.SetActive(false);
            spellbook.SetActive(false);
            Input.SwitchActionMapToGameplay();
        }
    }

    public void FindInput()
    {
        if (Input != null)
        {
            Input.SpellBook -= ControlSpellBook;
        }

        Input = FindObjectOfType<PlayerInputCustom>();
        Input.SpellBook += ControlSpellBook;
    }

    public void LostInput()
    {
        if (Input != null)
            Input.SpellBook -= ControlSpellBook;
    }

    public void FindPlayer()
    {
        StartCoroutine(SetSpellbookVariablesCoroutine());
        playerSpells = FindObjectOfType<PlayerSpells>();
    }

    private IEnumerator SetSpellbookVariablesCoroutine()
    {
        yield return new WaitForFixedUpdate();
        spellbook.SetActive(true);
        spellBookSpells.UpdateSpellSlots();
        spellBookPassives.UpdatePassiveSlots();
        spellBookAttributes.UpdateText();
        spellbook.SetActive(false);
    }

    public void PlayerLost()
    {
        if (Input != null)
            Input.SpellBook -= ControlSpellBook;
    }
}
