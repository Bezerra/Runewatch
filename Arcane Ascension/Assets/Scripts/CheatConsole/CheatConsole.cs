using UnityEngine;
using TMPro;
using System;
using UnityEngine.EventSystems;
using System.Collections.Generic;

/// <summary>
/// Class responsible for controling console and applying cheats.
/// </summary>
public class CheatConsole : MonoBehaviour, IFindPlayer, IFindInput
{
    // Components
    private IInput input;

    // Variables
    [SerializeField] private GameObject consoleGameObject;
    [SerializeField] private TMP_InputField inputField;
    private bool showConsole;
    private EventSystem eventSystem;

    // Variables for cheats
    private PlayerStats playerStats;
    private PlayerSpells playerSpells;
    private AllSpells allSpells;
    private SelectionBase playerRoot;
    private PlayerCurrency playerCurrency;
    private bool infiniteHealth;
    private bool infiniteMana;

    [Header("Enemy")]
    [SerializeField] private GameObject dummyEnemy;

    [Range(2,5)][SerializeField] private float spawnDistance;

    private void Awake()
    {
        input = FindObjectOfType<PlayerInputCustom>();
        eventSystem = FindObjectOfType<EventSystem>();

        FindRequiredComponents();
    }

    private void OnEnable() =>
        input.CheatConsole += ShowConsole;

    private void OnDisable() =>
        input.CheatConsole -= ShowConsole;

    /// <summary>
    /// Finds required components for cheats.
    /// </summary>
    private void FindRequiredComponents()
    {
        playerCurrency = FindObjectOfType<PlayerCurrency>();
        playerSpells = FindObjectOfType<PlayerSpells>();
        playerStats = FindObjectOfType<PlayerStats>();
        allSpells = FindObjectOfType<AllSpells>();
        Player pl = FindObjectOfType<Player>();
        if (pl != null)
            playerRoot = pl.GetComponentInParent<SelectionBase>();
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
                case "god 1":
                    Debug.Log("God mode activated");
                    infiniteHealth = true;
                    DisableConsole();
                    break;

                case "god 0":
                    Debug.Log("God mode deactivated");
                    infiniteHealth = false;
                    DisableConsole();
                    break;

                case "mana 1":
                    Debug.Log("Infinite mana activated");
                    infiniteMana = true;
                    DisableConsole();
                    break;

                case "mana 0":
                    Debug.Log("Infinite mana deactivated");
                    infiniteMana = false;
                    DisableConsole();
                    break;

                case "enemy":
                    if (CanSpawn())
                    {
                        Debug.Log("Spawn enemy");

                        Player pl = playerRoot.GetComponentInChildren<Player>();

                        Ray ray = new Ray(pl.Eyes.transform.position, pl.transform.forward);
                        if (Physics.Raycast(ray, spawnDistance, Layers.Walls))
                        {
                            // Do nothing
                        }
                        else
                        {
                            Ray ray2 = new Ray(
                                pl.Eyes.transform.position + pl.Eyes.transform.forward * spawnDistance,
                                Vector3.down);
                            if (Physics.Raycast(ray2, out RaycastHit floorHit, 10, Layers.WallsFloor))
                            {
                                Instantiate(dummyEnemy, floorHit.point,
                                    Quaternion.LookRotation(playerRoot.transform.position, Vector3.up));
                            }
                        }

                        
                    }
                    DisableConsole();
                    break;

                case "fly 1":
                    Debug.Log("Player fly true");

                    // Player Stuff
                    playerRoot.GetComponentInChildren<CharacterController>(true).enabled = false;
                    FindObjectOfType<PlayerFly>().CheatApplied = true;
                    ChangeLayersAllChilds(playerRoot.transform, Layers.PlayerLayerNum, Layers.InvisiblePlayerLayerNum, true);
                    foreach (Enemy en in FindObjectsOfType<Enemy>())
                        en.CurrentTarget = null;

                    // Level Stuff
                    // Creates a new list with all level pieces
                    IList<LevelPiece> generatedRoomPieces = 
                        FindObjectOfType<LevelGenerator>().AllLevelPiecesGenerated;
                    // Disables the rest of the pieces
                    for (int i = 0; i < generatedRoomPieces.Count; i++)
                    {
                        if (generatedRoomPieces[i] != null)
                        {
                            StartCoroutine(generatedRoomPieces[i].EnableChildOccludeesCoroutine());
                        }
                    }

                    DisableConsole();
                    break;

                case "fly 0":
                    Debug.Log("Player invisible false");
                    FindObjectOfType<PlayerFly>().CheatApplied = false;
                    playerRoot.GetComponentInChildren<CharacterController>(true).enabled = true;
                    ChangeLayersAllChilds(playerRoot.transform, Layers.PlayerLayerNum, Layers.InvisiblePlayerLayerNum, false);
                    DisableConsole();
                    break;

                case "invisible 1":
                    Debug.Log("Player invisible true");
                    ChangeLayersAllChilds(playerRoot.transform, Layers.PlayerLayerNum, Layers.InvisiblePlayerLayerNum, true);
                    foreach (Enemy en in FindObjectsOfType<Enemy>())
                        en.CurrentTarget = null;
                    DisableConsole();
                    break;

                case "invisible 0":
                    Debug.Log("Player invisible false");
                    ChangeLayersAllChilds(playerRoot.transform, Layers.PlayerLayerNum, Layers.InvisiblePlayerLayerNum, false);
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

                case "death":
                    Debug.Log("Death");
                    playerStats.TakeDamage(playerStats.MaxHealth, ElementType.Lux, Vector3.zero);
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
        FindObjectOfType<PlayerHandEffect>().UpdatePlayerHandEffect(playerSpells.ActiveSpell);
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
        if (infiniteHealth) playerStats.Health = playerStats.MaxHealth;
        if (infiniteMana) playerStats.Mana = playerStats.MaxMana;

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

    public void FindPlayer()
    {
        FindRequiredComponents();
    }

    public void PlayerLost()
    {
        inputField.text = "";
        consoleGameObject.SetActive(false);

        if (input != null)
            input.CheatConsole -= ShowConsole;
    }

    public void FindInput()
    {
        if (input != null)
        {
            input.CheatConsole -= ShowConsole;
        }

        input = FindObjectOfType<PlayerInputCustom>();
        input.CheatConsole += ShowConsole;
    }
        
    public void LostInput()
    {
        if (input != null)
        {
            input.CheatConsole -= ShowConsole;
        }
    }
}
