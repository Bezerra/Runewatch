using UnityEngine;

/// <summary>
/// Class responsible for activating and deactivating spellbook.
/// </summary>
public class ActivateSpellBook : MonoBehaviour, IFindInput, IFindPlayer
{
    [SerializeField] private GameObject spellbook;

    private IInput input;
    private bool isSpellBookOpened;

    private SpellBookSpells spellBookSpells;

    private void Awake()
    {
        input = FindObjectOfType<PlayerInputCustom>();
        spellBookSpells = GetComponent<SpellBookSpells>();
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
            Time.timeScale = 0;
            spellbook.SetActive(true);
            input.SwitchActionMapToSpellBook();
        }
        else
        {
            Time.timeScale = 1;
            spellbook.SetActive(false);
            input.SwitchActionMapToGameplay();
        }
    }

    public void FindInput()
    {
        if (input != null)
        {
            input.SpellBook -= ControlSpellBook;
        }

        input = FindObjectOfType<PlayerInputCustom>();
        input.SpellBook += ControlSpellBook;
    }

    public void LostInput()
    {
        if (input != null)
            input.SpellBook -= ControlSpellBook;
    }

    public void FindPlayer()
    {
        // Left blank on purpose
    }

    public void PlayerLost()
    {
        if (input != null)
            input.SpellBook -= ControlSpellBook;
    }
}
