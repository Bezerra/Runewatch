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
    private PlayerStats playerStats;
    private PlayerSpells playerSpells;
    private AllSpells allSpells;
    private SelectionBase playerRoot;
    private PlayerCurrency playerCurrency;

    [Header("Enemy")]
    [SerializeField] private GameObject dummyEnemy;

    [Range(2,5)][SerializeField] private float spawnDistance;

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
        playerCurrency = FindObjectOfType<PlayerCurrency>();
        playerSpells = FindObjectOfType<PlayerSpells>();
        playerStats = FindObjectOfType<PlayerStats>();
        allSpells = FindObjectOfType<AllSpells>();
        playerRoot = FindObjectOfType<Player>().GetComponentInParent<SelectionBase>();
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

        if (playerSpells == null || playerStats == null || allSpells == null || playerCurrency == null)
            FindRequiredComponents();

        string[] splitWords = input.ToLower().Trim().Split(' ');
        int number = 0;
        int number2 = 0;

        if (splitWords.Length == 3)
        {
            try
            {
                number = Convert.ToInt32(splitWords[1]);
            }
            catch { }
            try
            {
                number2 = Convert.ToInt32(splitWords[2]);
            }
            catch { }
        }

        if (splitWords.Length == 3 &&
            splitWords[0] == "spell" && 
            (number >= 0 && number <= Byte.MaxValue) &&
            (number2 >= 1 && number2 <= 4))
        {
            bool playerAlreadyHasSpell = false;
            foreach(SpellSO spell in playerSpells.CurrentSpells)
            {
                if (spell != null)
                {
                    if (spell.ID == number)
                        playerAlreadyHasSpell = true;
                }
            }

            if (playerAlreadyHasSpell == false)
            {
                foreach (SpellSO spell in allSpells.SpellList)
                {
                    if (spell.ID == Convert.ToByte(splitWords[1]))
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
        else if (splitWords.Length == 3 &&
                splitWords[0] == "currency" &&
                (number2 >= 0 && number2 <= Int32.MaxValue))
        {
            if (splitWords[1] == "gold")
            {
                playerCurrency.GainCurrency(CurrencyType.Gold, number2);
                DisableConsole();
            }
            if (splitWords[1] == "arcane")
            {
                playerCurrency.GainCurrency(CurrencyType.ArcanePower, number2);
                DisableConsole();
            }
        }
        else
        {
            switch (input.ToLower().Trim())
            {
                case "controls v1":
                    Debug.Log("Controls switched to v1");
                    PlayerPrefs.SetString("Controls", "Computer");
                    this.input.UpdateControlScheme();
                    DisableConsole();
                    break;

                case "controls v2":
                    Debug.Log("Controls switched to v2");
                    PlayerPrefs.SetString("Controls", "Computerv2");
                    this.input.UpdateControlScheme();
                    DisableConsole();
                    break;

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

                case "enemy":
                    if (CanSpawn())
                    {
                        Debug.Log("Spawn enemy");
                        Instantiate(
                            dummyEnemy, playerStats.transform.position + 
                            playerStats.transform.forward * spawnDistance, Quaternion.identity);
                    }
                    DisableConsole();
                    break;

                case "invisible 1":
                    Debug.Log("Player invisible true");
                    ChangeLayersAllChilds(playerRoot.transform, Layers.PlayerLayerNum, Layers.IgnoreLayerNum, true);
                    DisableConsole();
                    break;

                case "invisible 0":
                    Debug.Log("Player invisible false");
                    playerStats.gameObject.layer = Layers.PlayerLayerNum;
                    ChangeLayersAllChilds(playerRoot.transform, Layers.PlayerLayerNum, Layers.IgnoreLayerNum, false);
                    DisableConsole();
                    break;

                case "scroll":
                    if (CanSpawn())
                    {
                        Debug.Log("Spawn spell scroll");
                        ItemLootPoolCreator.Pool.InstantiateFromPool(
                            LootType.UnknownSpell.ToString(),
                            playerStats.transform.position +
                            playerStats.transform.forward * spawnDistance, Quaternion.identity);
                    }
                    DisableConsole();
                    break;

                case "orb":
                    if (CanSpawn())
                    {
                        Debug.Log("Spawn passive orb");
                        ItemLootPoolCreator.Pool.InstantiateFromPool(
                            LootType.PassiveOrb.ToString(),
                            playerStats.transform.position +
                            playerStats.transform.forward * spawnDistance, Quaternion.identity);
                    }
                    DisableConsole();
                    break;

                default:
                    inputField.text = "";
                    inputField.ActivateInputField();
                    break;
            }
        }
    }

    private void ChangeLayersAllChilds(Transform obj, int playerLayer, int ignoreLayer, bool turnInvisible)
    {
        Transform[] childs = obj.GetComponentsInChildren<Transform>();

        if (turnInvisible)
        {
            foreach (Transform child in childs)
            {
                if (child.gameObject.layer == playerLayer)
                    child.gameObject.layer = ignoreLayer;
            }
        }
        else
        {
            foreach (Transform child in childs)
            {
                if (child.gameObject.layer == ignoreLayer)
                    child.gameObject.layer = playerLayer;
            }
        }
    }

    private void AddSpell(SpellSO spell, int number2)
    {
        playerSpells.RemoveSpell(number2 - 1);
        playerSpells.AddSpell(spell, number2 - 1);
    }

    private void Godmode(float temp) => StartCoroutine(GodmodeCoroutine());
    private IEnumerator GodmodeCoroutine()
    {
        yield return wffu;
        playerStats.Heal(playerStats.CommonAttributes.MaxHealth, StatsType.Health);
    }

    private void InfiniteMana(float temp) => StartCoroutine(InfiniteManaCoroutine());
    private IEnumerator InfiniteManaCoroutine()
    {
        yield return wffu;
        playerStats.Heal(playerStats.PlayerAttributes.MaxMana, StatsType.Mana);
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

    private bool CanSpawn()
    {
        Player player = playerRoot.GetComponentInChildren<Player>();
        Ray forward = new Ray(player.Eyes.transform.position, player.Eyes.forward);
        if (Physics.Raycast(forward, spawnDistance))
            return false;
        return true;
    }
}
