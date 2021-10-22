using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
public class CheatConsole : MonoBehaviour
{
    // Components
    private PlayerInputCustom input;

    // Variables
    private bool showConsole;
    private string commandInput;

    // Commands
    private static DebugCommand GODMODE;

    private IList<object> commandList;

    // Variables for cheats
    private Stats playerStats;

    private void Awake()
    {
        input = FindObjectOfType<PlayerInputCustom>();
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        playerStats = FindObjectOfType<Player>().GetComponent<Stats>();

        GODMODE = new DebugCommand("god_mode", "heals player everytime he takes damage", "god_mode", () =>
            playerStats.EventTakeDamage += Godmode);

        commandList = new List<object>
        {
            GODMODE,
        };
    }

    ~CheatConsole()
    {
        playerStats.EventTakeDamage -= Godmode;
    }

    private void OnEnable()
    {
        input.CheatConsole += ShowConsole;
    }

    private void OnDisable()
    {
        input.CheatConsole -= ShowConsole;
    }

    private void Godmode(float damageToHeal)
    {
        playerStats.Heal(damageToHeal, StatsType.Health);
    }

    private void ShowConsole()
    {
        showConsole = !showConsole;
        if (showConsole == true)
        {
            input.SwitchActionMapToCheatConsole();
        }
        else
        {
            SubmitInput();
            commandInput = "";
            input.SwitchActionMapToGameplay();
        }
    }

    private void SubmitInput()
    {
        for (int i = 0; i < commandList.Count; i++)
        {
            DebugCommandBase commandBase = commandList[i] as DebugCommandBase;

            if(commandInput.Contains(commandBase.CommandID))
            {
                if(commandList[i] as DebugCommand != null)
                {
                    (commandList[i] as DebugCommand).Invoke();
                }
            }
        }
    }

    private void OnGUI()
    {
        if (showConsole == false)
            return;

        GUI.Box(new Rect(-10, Screen.height - 20, Screen.width + 20, 30), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0);
        commandInput = GUI.TextField(new Rect(10, Screen.height - 20, Screen.width, 20), commandInput);
    }


    private class DebugCommandBase
    {
        public string CommandID { get; }
        public string CommandDescription { get; }
        public string CommandFormat { get; }

        public DebugCommandBase (string id, string description, string format)
        {
            CommandID = id;
            CommandDescription = description;
            CommandFormat = format;
        }
    }

    private class DebugCommand: DebugCommandBase
    {
        private Action action;
        public DebugCommand(string id, string description, string format, Action action) : base(id, description, format)
        {
            this.action = action;
        }

        public void Invoke() => action.Invoke();
    }
}
