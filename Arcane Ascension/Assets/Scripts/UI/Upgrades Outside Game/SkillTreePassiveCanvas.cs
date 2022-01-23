using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Class responsible for controling nodes information and buying nodes.
/// Note that every time a node is updated, the game is saved, but those passives
/// will only take effect once the player starts a new game.
/// </summary>
public class SkillTreePassiveCanvas : MonoBehaviour
{
    [SerializeField] private GameObject existingRunButton;

    [Header("Child components for UI")]
    [SerializeField] private TextMeshProUGUI arcanePowerText;
    [SerializeField] private TextMeshProUGUI passiveName;
    [SerializeField] private TextMeshProUGUI passiveDescription;
    [SerializeField] private TextMeshProUGUI passiveCost;
    [SerializeField] private Button buyButton;
    [SerializeField] private Image buyButtonImage;

    [Header("Information for nodes logic")]
    [SerializeField] private CurrencySO currencySO;
    [SerializeField] private GameObject connectionLinePrefab;
    [SerializeField] private Color unlockedColor;
    [SerializeField] private Color lockedColor;

    // Properties for node logic
    public Color UnlockedColor => unlockedColor;
    public Color LockedColor => lockedColor;
    public CurrencySO CurrencySO => currencySO;
    public GameObject ConnectionLinePrefab => connectionLinePrefab;

    // Variables for showing info and buying nodes
    private SkillTreePassiveNode passiveNode;

    // List with passives
    public List<byte> CurrentPassives { get; private set; }

    // Save system
    private CharacterSaveDataController characterSaveDataController;
    private RunSaveDataController runSaveDataController;

    private void Awake()
    {
        characterSaveDataController = FindObjectOfType<CharacterSaveDataController>();
        runSaveDataController = FindObjectOfType<RunSaveDataController>();

        CurrentPassives = new List<byte>();
    }

    private void OnEnable()
    {
        ClearAllInformation();
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

        ClearAllInformation();

        arcanePowerText.text =
            "Arcane Power: " + saveData.ArcanePower.ToString();
    }

    /// <summary>
    /// Update UI information on this canvas.
    /// </summary>
    /// <param name="passiveNode"></param>
    public void UpdateInformation(SkillTreePassiveNode passiveNode)
    {
        // Update buy button
        // If player has enough arcane power to buy selected node
        if (passiveNode.NodePassiveNext != null &&
            currencySO.CanSpend(CurrencyType.ArcanePower, passiveNode.NodePassiveNext.Cost))
        {
            int requiredNodes = 0;
            // Checks all required nodes
            foreach (SkillTreePassiveNode previousNode in passiveNode.PreviousConnectionNodes)
            {
                if (previousNode.IsUnlocked)
                {
                    requiredNodes++;
                }
            }
            // If all required nodes are already unlocked
            if (requiredNodes == passiveNode.PreviousConnectionNodes.Count &&
                passiveNode.CurrentTier < passiveNode.NodePassives.Length)
            {
                buyButton.enabled = true;
                buyButtonImage.color = unlockedColor;
            }
            else
            {
                buyButton.enabled = false;
                buyButtonImage.color = lockedColor;
            }
        }
        else
        {
            buyButton.enabled = false;
            buyButtonImage.color = lockedColor;
        }

        if (passiveNode.CurrentTier < passiveNode.NodePassives.Length)
        {
            this.passiveNode = passiveNode;
            passiveCost.text = "Cost: " + passiveNode.NodePassiveNext.Cost.ToString() + " arcane power";
        }
    }

    /// <summary>
    /// Buy selected passive.
    /// </summary>
    public void BuyPassive()
    {
        if (runSaveDataController.FileExists() == false)
        {
            // Must be before unlock (before tier raise)
            CurrentPassives.Add(passiveNode.NodePassives[passiveNode.CurrentTier].ID);

            // Unlocks passive, spends money and updates arcane power.
            passiveNode.Unlock();
            arcanePowerText.text =
                "Arcane Power: " + characterSaveDataController.SaveData.ArcanePower.ToString();

            existingRunButton.SetActive(false);
        }
        else
        {
            existingRunButton.SetActive(true);
        }
    }

    public void LeaveButton()
    {
        byte[] passivesID = new byte[CurrentPassives.Count];
        for (int i = 0; i < CurrentPassives.Count; i++)
        {
            passivesID[i] = CurrentPassives[i];
        }

        // Saves current passives list and arcane power
        characterSaveDataController.Save(
            passivesID, characterSaveDataController.SaveData.ArcanePower);
    }

    /// <summary>
    /// Resets all information.
    /// </summary>
    public void ClearAllInformation()
    {
        passiveNode = null;
        passiveName.text = " ";
        passiveDescription.text = " ";
        passiveCost.text = " ";
        buyButton.enabled = false;
        buyButtonImage.color = lockedColor;

        if (runSaveDataController.FileExists() == false)
        {
            existingRunButton.SetActive(false);
        }
    }

    public void EndCurrentRunOnSkillTree()
    {
        FindObjectOfType<RunSaveDataController>().DeleteFile();
        existingRunButton.SetActive(false);
    }
}
