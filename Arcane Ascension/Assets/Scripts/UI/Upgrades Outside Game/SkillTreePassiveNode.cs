using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System.Text;
using ExtensionMethods;

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
    [HideInInspector] [SerializeField] private GameObject connectionLinePrefab;

    // Properties
    public List<SkillTreePassiveNode> PreviousConnectionNodes => previousConnectionNodes;
    public List<SkillTreePassiveNode> NextConnectionNodes => nextConnectionNodes;
    public bool IsUnlocked { get; private set; }

    // Components
    private TEMPTOCHANGEINPUT TEMPPARENT;
    private Image nodeImage;

    private void Awake()
    {
        nodeImage = GetComponent<Image>();
        TEMPPARENT = GetComponentInParent<TEMPTOCHANGEINPUT>();

        currentTier = 0;
        nodeImage.color = lockedColor;

        // Creates connections to all nodes
        foreach (SkillTreePassiveNode node in nextConnectionNodes)
        {
            CreateInstantConnectionLine(node.gameObject);
        }
    }

    private void Start()
    {
        // Checks if the player already has this node, from 0 to higher tiers.
        // In that case, increments the tier of the node and updates UI.
        for (int i = 0; i < nodePassive.Length; i++)
        {
            if (TEMPPARENT.Passives.Contains(nodePassive[i].ID))
            {
                currentTier++;
                UpdateUI();
            }
        }
    }

    /// <summary>
    /// Updates node information on editor.
    /// </summary>
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

    /// <summary>
    /// Increments current node tier. Unlocks a locked node.
    /// </summary>
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
        if (IsUnlocked ==  false)
        {
            IsUnlocked = true;
            nodeImage.color = unlockedColor;

            // If this node was unlocked, it will grow a line from the previosu node to this one
            if (previousConnectionNodes.Count > 0)
            {
                foreach (SkillTreePassiveNode node in previousConnectionNodes)
                {
                    node.StartCoroutine(node.GrowConnectionLine(gameObject));
                }
            }
        }

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

    public IEnumerator GrowConnectionLine(GameObject targetGO)
    {
        // Disables button
        Button button = GetComponent<Button>();
        button.enabled = false;

        // If a line with this name already exists, it destroys it
        Destroy(this.gameObject.transform.parent.Find(this.gameObject.name + " connection").gameObject);

        // Creates a line
        GameObject connectionLineGO = Instantiate(connectionLinePrefab, transform.position, Quaternion.identity);
        connectionLineGO.transform.SetParent(this.gameObject.transform.parent);
        connectionLineGO.transform.SetAsFirstSibling();
        connectionLineGO.name = this.gameObject.name + " connection";

        // Gets image and line and colors it
        Image connectionLineImage = connectionLineGO.GetComponent<Image>();
        connectionLineImage.color = unlockedColor;

        // Gets a rectangle, calculates distance and rotation from this node to the next node
        RectTransform connectionLine = connectionLineGO.GetComponent<RectTransform>();
        RectTransform target = targetGO.GetComponent<RectTransform>();
        connectionLine.localScale = Vector3.one;
        float distance = Vector2.Distance(connectionLine.anchoredPosition, target.anchoredPosition);
        float angle = 
            Mathf.Atan2(target.anchoredPosition.y - connectionLine.anchoredPosition.y, 
            target.anchoredPosition.x - connectionLine.anchoredPosition.x);
        connectionLine.localEulerAngles = new Vector3(0, 0, ((180 / Mathf.PI) * angle));
        connectionLine.sizeDelta = new Vector2(0, 5f);

        // Increments that rectangle smoothly until the next node
        float currentLineWidth = 0;
        while (connectionLine.sizeDelta.x < distance)
        {
            yield return null;
            currentLineWidth += Time.deltaTime * 600f;
            connectionLine.sizeDelta = new Vector2(currentLineWidth, 5f);
        }

        // Enables button back
        button.enabled = true;

        /*
        if (nextConnectionNodes.Count > 0)
        {
            foreach (SkillTreePassiveNode node in nextConnectionNodes)
            {
                foreach (SkillTreePassiveNode nodeInNextNode in node.NextConnectionNodes)
                {
                    node.StartCoroutine(GrowConnectionLine(nodeInNextNode.gameObject));
                }
            }
        }
        */
    }

    private void CreateInstantConnectionLine(GameObject targetGO)
    {
        // Creates a line
        GameObject connectionLineGO = Instantiate(connectionLinePrefab, transform.position, Quaternion.identity);
        connectionLineGO.transform.SetParent(this.gameObject.transform.parent);
        connectionLineGO.transform.SetAsFirstSibling();
        connectionLineGO.name = this.gameObject.name + " connection";

        // Gets image and line and colors it
        Image connectionLineImage = connectionLineGO.GetComponent<Image>();
        connectionLineImage.color = lockedColor;

        // Gets a rectangle, calculates distance and rotation from this node to the next node
        RectTransform connectionLine = connectionLineGO.GetComponent<RectTransform>();
        RectTransform target = targetGO.GetComponent<RectTransform>();
        connectionLine.localScale = Vector3.one;
        float distance = Vector2.Distance(connectionLine.anchoredPosition, target.anchoredPosition);
        float angle =
            Mathf.Atan2(target.anchoredPosition.y - connectionLine.anchoredPosition.y,
            target.anchoredPosition.x - connectionLine.anchoredPosition.x);
        connectionLine.localEulerAngles = new Vector3(0, 0, ((180 / Mathf.PI) * angle));
        connectionLine.sizeDelta = new Vector2(0, 5f);

        // Connects this node to the next node
        connectionLine.sizeDelta = new Vector2(distance, 5f);
    }
}
