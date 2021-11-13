using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System.Text;

public class SkillTreePassiveNode : MonoBehaviour
{
    [Header("Passive tiers of this node")]
    [SerializeField] private SkillTreePassiveSO[] nodePassive;
    private byte currentTier;

    [Header("Connections")]
    [SerializeField] private List<SkillTreePassiveNode> previousConnectionNodes;
    [SerializeField] private List<SkillTreePassiveNode> nextConnectionNodes;

    [Header("Colors")]
    [SerializeField] private Color unlockedColor;
    [SerializeField] private Color lockedColor;
    [HideInInspector] [SerializeField] private TextMeshProUGUI nodeName;
    [HideInInspector] [SerializeField] private TextMeshProUGUI nodeTier;
    [HideInInspector] [SerializeField] private CurrencySO currency;

    // Properties
    public List<SkillTreePassiveNode> PreviousConnectionNodes => previousConnectionNodes;
    public bool IsUnlocked { get; private set; }

    // Components
    private TEMPTOCHANGEINPUT TEMPPARENT;
    private Image nodeImage;

    private void Awake()
    {
        nodeImage = GetComponent<Image>();
        TEMPPARENT = GetComponentInParent < TEMPTOCHANGEINPUT > ();
    }

    private void Start()
    {
        currentTier = 0;
        nodeImage.color = lockedColor;

        for (int i = 0; i < nodePassive.Length; i++)
        {
            if (TEMPPARENT.Passives.Contains(nodePassive[i].ID))
            {
                currentTier++;
                UpdateUI();
            }
        }  
    }

    private void OnValidate()
    {
        if (nodeName != null && nodePassive.Length > 0)
        {
            string[] passiveNameSplit = nodePassive[0].Name.Split();
            StringBuilder passiveNameToPrint = new StringBuilder();
            // Ignores last numbers on passive name
            for (int i = 0; i < passiveNameSplit.Length - 1; i++)
            {
                if (i > 0) passiveNameToPrint.Append(" ");
                passiveNameToPrint.Append(passiveNameSplit[i]);
            }
            nodeName.text = passiveNameToPrint.ToString();

            nodeTier.text = "0 / " + nodePassive.Length;
        }

        if (nodePassive.Length > 0)
        {
            nodePassive = nodePassive.OrderBy(i => i.Tier).ToArray();
        }
    }

    public void TryUnlock()
    {
        // If this node doesn't require any previous connections
        if (previousConnectionNodes.Count == 0)
        {
            // If it's not in maximum tier yet
            if (currentTier < nodePassive.Length)
            {
                // If player has enough arcane power
                if (currency.CanSpend(CurrencyType.ArcanePower, nodePassive[currentTier].Cost))
                {
                    // Increments tier, spends money, updates UI to next tier
                    currentTier++;
                    currency.SpendCurrency(CurrencyType.ArcanePower, nodePassive[currentTier-1].Cost);
                    UpdateUI();
                }
            }
            return;
        }

        int requiredNodes = 0;
        // Checks all required nodes
        foreach (SkillTreePassiveNode previousNode in PreviousConnectionNodes)
        {
            if (previousNode.IsUnlocked)
            {
                requiredNodes++;
            }
        }
        // If all required nodes are already unlocked and the player has enough arcane power
        if (requiredNodes == PreviousConnectionNodes.Count &&
            currency.CanSpend(CurrencyType.ArcanePower, nodePassive[currentTier].Cost))
        {
            // If it's not in maximum tier yet
            if (currentTier < nodePassive.Length)
            {
                // If player has enough arcane power
                if (currency.CanSpend(CurrencyType.ArcanePower, nodePassive[currentTier].Cost))
                {
                    // Increments tier, spends money, updates UI to next tier
                    currentTier++;
                    currency.SpendCurrency(CurrencyType.ArcanePower, nodePassive[currentTier - 1].Cost);
                    UpdateUI();
                }
            }
        }
    }

    /// <summary>
    /// Updates UI of current tier.
    /// </summary>
    private void UpdateUI()
    {
        IsUnlocked = true;
        nodeImage.color = unlockedColor;

        // Uses current tier - 1, so it matches passives in this node array index
        nodeName.text = nodePassive[currentTier - 1].Name;
        nodeTier.text = nodePassive[currentTier - 1].Tier.ToString() + " / " + nodePassive.Length;

        // If this node tier (+1 so it matches node array maximum length) is the same as
        // this node passives length, it blocks the node 
        if (currentTier == nodePassive.Length)
        {
            nodeImage.color = lockedColor;
            GetComponent<Button>().enabled = false;
        }
    }
}
