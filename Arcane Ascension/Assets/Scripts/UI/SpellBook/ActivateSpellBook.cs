using UnityEngine;

/// <summary>
/// Class responsible for activating and deactivating spellbook.
/// </summary>
public class ActivateSpellBook : MonoBehaviour, IFindInput, IFindPlayer
{
    [SerializeField] private GameObject spellbook;

    public IInput Input { get; private set; }
    private bool isSpellBookOpened;

    private SpellBookSpells spellBookSpells;
    private SpellBookPassives spellBookPassives;

    private void Awake()
    {
        Input = FindObjectOfType<PlayerInputCustom>();
        spellBookSpells = GetComponent<SpellBookSpells>();
        spellBookPassives = GetComponent<SpellBookPassives>();
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
            spellBookSpells.UpdateSpellSlots();
            spellBookPassives.UpdatePassiveSlots();
            Time.timeScale = 0;
            spellbook.SetActive(true);
            Input.SwitchActionMapToSpellBook();
        }
        else
        {
            Time.timeScale = 1;
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
        // Left blank on purpose
    }

    public void PlayerLost()
    {
        if (Input != null)
            Input.SpellBook -= ControlSpellBook;
    }
}
