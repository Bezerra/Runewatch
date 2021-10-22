using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CheatConsole : MonoBehaviour
{
    // Components
    private PlayerInputCustom input;

    // Variables
    [SerializeField] private GameObject consoleGameObject;
    [SerializeField] private TMP_InputField inputField;
    private bool showConsole;
    private YieldInstruction wffu;

    // Variables for cheats
    private Stats playerStats;
    

    private void Awake()
    {
        input = FindObjectOfType<PlayerInputCustom>();
        wffu = new WaitForFixedUpdate();

        playerStats = FindObjectOfType<Player>().GetComponent<Stats>();
    }

    private void OnEnable()
    {
        input.CheatConsole += ShowConsole;
    }

    private void OnDisable()
    {
        input.CheatConsole -= ShowConsole;
    }

    public void OnInputFieldSubmit(string input)
    {
        switch(input.ToLower().Trim())
        {
            case "god 1":
                Debug.Log("God mode activated");
                playerStats.EventTakeDamage += Godmode;
                DeactivateConsole();
                break;

            case "god 0":
                Debug.Log("God mode deactivated");
                playerStats.EventTakeDamage -= Godmode;
                DeactivateConsole();
                break;

            case "mana 1":
                Debug.Log("Infinite mana activated");
                playerStats.EventTakeDamage += InfiniteMana;
                DeactivateConsole();
                break;

            case "mana 0":
                Debug.Log("Infinite mana deactivated");
                playerStats.EventTakeDamage -= InfiniteMana;
                DeactivateConsole();
                break;

            default:
                inputField.text = "";
                inputField.ActivateInputField();
                break;
        }
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
            inputField.gameObject.SetActive(false);
            consoleGameObject.SetActive(false);
            input.SwitchActionMapToGameplay();
        }
    }

    public void DeactivateConsole()
    {
        inputField.text = "";
        inputField.gameObject.SetActive(false);
        consoleGameObject.SetActive(false);
        input.SwitchActionMapToGameplay();
        showConsole = false;
    }
}
