using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text;

public class SkillTreePassiveCanvas : MonoBehaviour
{
    [Header("Child components")]
    [SerializeField] private List<TextMeshProUGUI> passiveBoosts;
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
        if (passiveNode.NodePassive.Tier <= passiveNode.QuantityPassives)
        {
            this.passiveNode = passiveNode;
            passiveName.text = passiveNode.NodePassive.Name.ToString();
            passiveDescription.text = passiveNode.NodePassive.Description.ToString();
            passiveCost.text = "Cost: " + passiveNode.NodePassive.Cost.ToString() + " arcane power";

            for (int i = 0; i < passiveBoosts.Count; i++)
            {
                if (passiveNode.CurrentTier > i)
                    passiveBoosts[i].color = unlockedColor;
                else
                    passiveBoosts[i].color = lockedColor;

                if (passiveNode.NodePassives.Length > i)
                    passiveBoosts[i].text = passiveNode.NodePassives[i].Description;
                else
                    passiveBoosts[i].text = " ";
            }
        }

        // Update buy button
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
            // If all required nodes are already unlocked and the player has enough arcane power
            if (requiredNodes == passiveNode.PreviousConnectionNodes.Count &&
                passiveNode.CurrentTier < passiveNode.QuantityPassives)
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

        for (int i = 0; i < passiveBoosts.Count; i++)
        {
            passiveBoosts[i].text = " ";
        }

        // Update AP
        arcanePowerText.text = 
            "Arcane Power: " + PlayerPrefs.GetInt(CurrencyType.ArcanePower.ToString()).ToString();
    }
}
