using System.Collections;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.EventSystems;

/// <summary>
/// Class responsible for controling console and applying cheats.
/// </summary>
public class CheatConsole : MonoBehaviour
{
    // Components
    private PlayerInputCustom input;

    // Variables
    [SerializeField] private GameObject consoleGameObject;
    [SerializeField] private TMP_InputField inputField;
    private bool showConsole;
    private YieldInstruction wffu;
    private EventSystem eventSystem;

    // Variables for cheats
    private Stats playerStats;
    private PlayerSpells playerSpells;
    private AllSpells allSpells;
    

    private void Awake()
    {
        input = FindObjectOfType<PlayerInputCustom>();
        wffu = new WaitForFixedUpdate();
        eventSystem = FindObjectOfType<EventSystem>();

        FindRequiredComponents();
    }

    private void OnEnable()
    {
        input.CheatConsole += ShowConsole;
    }

    private void OnDisable()
    {
        input.CheatConsole -= ShowConsole;
    }

    /// <summary>
    /// Finds required components for cheats.
    /// </summary>
    private void FindRequiredComponents()
    {
        playerSpells = FindObjectOfType<Player>().GetComponent<PlayerSpells>();
        playerStats = FindObjectOfType<Player>().GetComponent<Stats>();
        allSpells = FindObjectOfType<AllSpells>();
    }

    /////////////////////////////////// Cheats code /////////////////////////////////////
    #region Cheats
    /// <summary>
    /// On value submited.
    /// </summary>
    /// <param name="input">Player's input from input field.</param>
    public void OnInputFieldSubmit(string input)
    {
        if (input.Length == 0)
        {

            return;
        }

        if (playerSpells == null || playerStats == null || allSpells == null)
            FindRequiredComponents();

        string[] splitWords = input.ToLower().Trim().Split(' ');
        byte spellNumber = 0;
        byte slotNumber = 0;

        if (splitWords.Length == 3)
        {
            spellNumber = Convert.ToByte(splitWords[1]);
            slotNumber = Convert.ToByte(splitWords[2]);
        }

        if (splitWords.Length == 3 &&
            splitWords[0] == "spell" && 
            (spellNumber >= 0 && spellNumber <= Byte.MaxValue) &&
            (slotNumber >= 1 && slotNumber <= 4))
        {
            bool playerAlreadyHasSpell = false;
            foreach(SpellSO spell in playerSpells.CurrentSpells)
            {
                if (spell != null)
                {
                    if (spell.SpellID == spellNumber)
                        playerAlreadyHasSpell = true;
                }
            }

            if (playerAlreadyHasSpell == false)
            {
                foreach (SpellSO spell in allSpells.SpellList)
                {
                    if (spell.SpellID == Convert.ToByte(splitWords[1]))
                    {
                        AddSpell(spell, Convert.ToByte(splitWords[2]));
                        DisableConsole();
                        break;
                    }
                }
            }
            else
            {
                inputField.text = " ";
                inputField.ActivateInputField();
            }
        }
        else
        {
            switch (input.ToLower().Trim())
            {
                case "god 1":
                    Debug.Log("God mode activated");
                    playerStats.EventTakeDamage += Godmode;
                    DisableConsole();
                    break;

                case "god 0":
                    Debug.Log("God mode deactivated");
                    playerStats.EventTakeDamage -= Godmode;
                    DisableConsole();
                    break;

                case "mana 1":
                    Debug.Log("Infinite mana activated");
                    playerStats.EventSpentMana += InfiniteMana;
                    DisableConsole();
                    break;

                case "mana 0":
                    Debug.Log("Infinite mana deactivated");
                    playerStats.EventSpentMana -= InfiniteMana;
                    DisableConsole();
                    break;

                default:
                    inputField.text = "";
                    inputField.ActivateInputField();
                    break;
            }
        }
    }

    private void AddSpell(SpellSO spell, byte slotNumber)
    {
        playerSpells.RemoveSpell(slotNumber - 1);
        playerSpells.AddSpell(spell, slotNumber - 1);
    }

    private void Godmode(float temp) => StartCoroutine(GodmodeCoroutine());
    private IEnumerator GodmodeCoroutine()
    {
        yield return wffu;
        playerStats.Heal(playerStats.Attributes.MaxHealth, StatsType.Health);
    }

    private void InfiniteMana(float temp) => StartCoroutine(InfiniteManaCoroutine());
    private IEnumerator InfiniteManaCoroutine()
    {
        yield return wffu;
        playerStats.Heal(playerStats.Attributes.MaxMana, StatsType.Mana);
    }
    #endregion
    /////////////////////////////////// Cheats code /////////////////////////////////////

    /// <summary>
    /// Controls console.
    /// </summary>
    private void ShowConsole()
    {
        showConsole = !showConsole;

        if (showConsole == true)
        {
            input.SwitchActionMapToCheatConsole();
            consoleGameObject.SetActive(true);
            inputField.gameObject.SetActive(true);
            inputField.ActivateInputField();
        }
        else
        {
            DisableConsole();
        }
    }

    /// <summary>
    /// Disables console entirely.
    /// </summary>
    private void DisableConsole()
    {
        inputField.text = "";
        consoleGameObject.SetActive(false);
        input.SwitchActionMapToGameplay();
    }

    private void Update()
    {
        if (consoleGameObject.activeSelf)
        {
            if (eventSystem.currentSelectedGameObject == null)
                inputField.ActivateInputField();
        }
    }
}
