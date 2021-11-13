using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Class responsible for controling nodes information and buying nodes.
/// </summary>
public class SkillTreePassiveCanvas : MonoBehaviour
{
    [Header("Child components")]
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

    public Color UnlockedColor => unlockedColor;
    public Color LockedColor => lockedColor;
    public CurrencySO CurrencySO => currencySO;
    public GameObject ConnectionLinePrefab => connectionLinePrefab;

    // Variables for showing info and buying nodes logic
    private SkillTreePassiveNode passiveNode;

    private void OnEnable()
    {
        ClearAllInformation();
    }

    /// <summary>
    /// Update UI information on this canvas.
    /// </summary>
    /// <param name="passiveNode"></param>
    public void UpdateInformation(SkillTreePassiveNode passiveNode)
    {

        if (passiveNode.NodePassive.Tier <= passiveNode.NodePassives.Length)
        {
            this.passiveNode = passiveNode;
            passiveName.text = passiveNode.NodePassive.Name.ToString();
            passiveDescription.text = passiveNode.NodePassive.Description.ToString();
            passiveCost.text = "Cost: " + passiveNode.NodePassive.Cost.ToString() + " arcane power";
        }

        // Update buy button
        // If player has enough arcane power to buy selected node
        if (currencySO.CanSpend(CurrencyType.ArcanePower, passiveNode.NodePassive.Cost))
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
    }

    /// <summary>
    /// Buy selected passive.
    /// </summary>
    public void BuyPassive()
    {
        // Unlocks passive, spends money and updates arcane power.
        passiveNode.Unlock();
        arcanePowerText.text =
            "Arcane Power: " + PlayerPrefs.GetInt(CurrencyType.ArcanePower.ToString()).ToString();
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

        // Update AP
        arcanePowerText.text = 
            "Arcane Power: " + PlayerPrefs.GetInt(CurrencyType.ArcanePower.ToString()).ToString();
    }
}
