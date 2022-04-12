using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Class responsible for controling nodes information and buying nodes.
/// Note that every time a node is updated, the game is saved, but those passives
/// will only take effect once the player starts a new game.
/// </summary>
public class SkillTreePassiveController : MonoBehaviour
{
    [SerializeField] private GameObject existingRunButton;

    [Header("Child components for UI")]
    [SerializeField] private TextMeshProUGUI arcanePowerText;

    [Header("Information for nodes logic")]
    [SerializeField] private CurrencySO currencySO;
    [SerializeField] private GameObject connectionLinePrefab;
    [SerializeField] private Color unlockedColor;
    [SerializeField] private Color lockedColor;

    [Header("Nodes Reset")]
    [SerializeField] private GameObject nodesSpawnParent;
    [SerializeField] private GameObject nodesParentPrefab;
    private GameObject spawnedNodesParentPrefab;
    [SerializeField] private GameObject initialNodesParent;

    // Properties for node logic
    public Color UnlockedColor => unlockedColor;
    public Color LockedColor => lockedColor;
    public CurrencySO CurrencySO => currencySO;
    public GameObject ConnectionLinePrefab => connectionLinePrefab;

    // List with passives
    public List<byte> CurrentPassives { get; private set; }

    // Save system
    private CharacterSaveDataController characterSaveDataController;

    private void Awake()
    {
        CurrentPassives = new List<byte>();
        characterSaveDataController = FindObjectOfType<CharacterSaveDataController>();
    }

    private void OnEnable()
    {
        if (FindObjectOfType<RunSaveDataController>().FileExists() == false)
            existingRunButton.SetActive(false);
        else
            existingRunButton.SetActive(true);
    }
        

    /// <summary>
    /// This logic MUST be on start, because the ID's are getting ordered on awake on
    /// SkillTreePassivesExecute, as a precaution.
    /// </summary>
    private void Start()
    {
        CharacterSaveData saveData = characterSaveDataController.SaveData;

        // Adds saved data passives to a list with current passives
        if (saveData.CurrentSkillTreePassives != null)
        {
            foreach (byte passive in saveData.CurrentSkillTreePassives)
            {
                CurrentPassives.Add(passive);
            }
            CurrentPassives.Sort();
        }

        currencySO.ResetCurrency();

        // If the file is completely empty. It's a new character
        // and this will happen
        if (CurrentPassives.Count == 0)
        {
            CurrentPassives.Add(0); // Adds default spell

            // THIS GAIN CURRENCY IS TEMPORARY, IT'S ONLY HERE SO THE FIRST TIME OPENING THE GAME
            // THE PLAYER WILL GET AP CURRENCY TO SPEND ON SKILL TREE, REMOVE ON FINAL BUILD
            // AND SET DEFAULT AP GAINED ON PLAYERCURRENCY SCRIPT.
            currencySO.GainCurrency(CurrencyType.ArcanePower, currencySO.DefaultArcanePower);
            //////////////////////////////////////////////////////////////////////////////////////
            // currencySO.DefaultArcanePower IN THIS NEXT LINE IS ALSO TEMP, IT'S SAVING DEFAULT AP.
            // IT SHOULD SAVE AS 0, SINCE IT'S THE FIRST NEW GAME, AP WILL ONLY SAVE IF THIS IF
            // STATEMENT IS ELSE (MEANING A SAVE FILE ALREADY EXISTS, THEREFORE SAVES AP (>0)
            characterSaveDataController.Save(new byte[] { 0 }, currencySO.DefaultArcanePower);
        }
        else
        {
            currencySO.GainCurrency(CurrencyType.ArcanePower, saveData.ArcanePower);
        }

        UpdateArcanePowerText();
    }

    /// <summary>
    /// Deletes save file (deletes all adquired passives) and returns all spent ap
    /// back to player.
    /// </summary>
    public void ResetButton()
    {
        if (CurrentPassives.Count == 1) return;

        SkillTreePassiveNode[] allNodes = GetComponentsInChildren<SkillTreePassiveNode>();

        int currentArcanePower = characterSaveDataController.SaveData.ArcanePower;
        int spentAP = 0;
        for (int i = 0; i < allNodes.Length; i++)
        {
            if (allNodes[i].NodePassive == null) continue;

            for (int j = 0; j < allNodes[i].NodePassive.Tier; j++)
            {
                spentAP += allNodes[i].NodePassives[j].Cost;
            }
        }

        // Deletes character save file
        EndCurrentCharacterOnSkillTree();

        // Resets currency and returns spent currency
        currencySO.ResetCurrency();
        currencySO.GainCurrency(CurrencyType.ArcanePower, currentArcanePower + spentAP);

        // Creates a new save file with default spell + arcane currency
        CurrentPassives = new List<byte>();
        CurrentPassives.Add(0); // Adds default spell
        characterSaveDataController.Save(new byte[] { 0 }, currentArcanePower + spentAP);

        // Updates text
        UpdateArcanePowerText();

        if (initialNodesParent != null) Destroy(initialNodesParent.gameObject);
        else Destroy(spawnedNodesParentPrefab.gameObject);
        spawnedNodesParentPrefab = Instantiate(nodesParentPrefab, nodesSpawnParent.transform);
    }

    /// <summary>
    /// Called when pressing leave button. Saves current passives to save file.
    /// </summary>
    public void LeaveButton()
    {
        byte[] passivesID = new byte[CurrentPassives.Count];
        for (int i = 0; i < CurrentPassives.Count; i++)
        {
            passivesID[i] = CurrentPassives[i];
        }

        characterSaveDataController.Save(
            passivesID, characterSaveDataController.SaveData.ArcanePower);
    }

    /// <summary>
    /// Updates arcane power text.
    /// </summary>
    public void UpdateArcanePowerText()
    {
        arcanePowerText.text =
            "Arcane Power: " + characterSaveDataController.SaveData.ArcanePower.ToString();
    }

    /// <summary>
    /// Activates and deactivates 'existing run button'.
    /// </summary>
    /// <param name="condition">True to activate, false to deactivate.</param>
    public void ControlExistingRunButton(bool condition)
    {
        if (condition) existingRunButton.SetActive(true);
        else existingRunButton.SetActive(false);
    }

    /// <summary>
    /// Deletes run save file.
    /// </summary>
    public void EndCurrentRunOnSkillTree()
    {
        FindObjectOfType<RunSaveDataController>().DeleteFile();
        existingRunButton.SetActive(false);
    }

    /// <summary>
    /// Deletes run save file.
    /// </summary>
    public void EndCurrentCharacterOnSkillTree() =>
        FindObjectOfType<CharacterSaveDataController>().DeleteFile();
}
