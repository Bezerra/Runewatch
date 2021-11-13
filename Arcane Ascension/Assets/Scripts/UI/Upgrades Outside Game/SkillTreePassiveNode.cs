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
    [SerializeField] private SkillTreePassiveSO[] nodePassives;
    public SkillTreePassiveSO[] NodePassives => nodePassives;
    public byte CurrentTier { get; private set; }

    // Current passive obtained with current tier
    public SkillTreePassiveSO NodePassive
    {
        get
        {
            if (CurrentTier < nodePassives.Length)
            {
                return nodePassives[CurrentTier];
            }
            else
            {
                return nodePassives[CurrentTier - 1];
            }
        }
    }
    
    [Header("Connections")]
    [SerializeField] private List<SkillTreePassiveNode> previousConnectionNodes;
    [SerializeField] private List<SkillTreePassiveNode> nextConnectionNodes;

    [Header("Node Child Components")]
    [HideInInspector] [SerializeField] private TextMeshProUGUI nodeName;
    [HideInInspector] [SerializeField] private TextMeshProUGUI nodeTier;

    [Header("DASDAS")]
    [SerializeField] private Image nodeImage;

    // Properties
    public List<SkillTreePassiveNode> PreviousConnectionNodes => previousConnectionNodes;
    public List<SkillTreePassiveNode> NextConnectionNodes => nextConnectionNodes;
    public bool IsUnlocked { get; private set; }

    // Components
    private TEMPTOCHANGEINPUT TEMPPARENT;
    private SkillTreePassiveCanvas parentNodeController;

    private void Awake()
    {
        TEMPPARENT = GetComponentInParent<TEMPTOCHANGEINPUT>();
        parentNodeController = GetComponentInParent<SkillTreePassiveCanvas>();

        CurrentTier = 0;
        nodeImage.color = parentNodeController.LockedColor;

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
        for (int i = 0; i < nodePassives.Length; i++)
        {
            if (TEMPPARENT.Passives.Contains(nodePassives[i].ID))
            {
                CurrentTier++;
                UpdateUI();
            }
        }
    }

    /// <summary>
    /// Updates node information on editor.
    /// </summary>
    private void OnValidate()
    {
        if (nodeName != null && nodePassives.Length > 0)
        {
            string[] passiveNameSplit = nodePassives[0].Name.Split();
            StringBuilder passiveNameToPrint = new StringBuilder();
            // Ignores last numbers on passive name
            for (int i = 0; i < passiveNameSplit.Length - 1; i++)
            {
                if (i > 0) passiveNameToPrint.Append(" ");
                passiveNameToPrint.Append(passiveNameSplit[i]);
            }
            nodeName.text = passiveNameToPrint.ToString();

            nodeTier.text = "0 / " + nodePassives.Length;
            nodeImage.sprite = nodePassives[0].Icon;
        }

        if (nodePassives.Length > 0)
        {
            nodePassives = nodePassives.OrderBy(i => i.Tier).ToArray();
        }
    }

    /// <summary>
    /// Triggered with button click. Obtains a node information.
    /// </summary>
    public void ObtainNodeInformation() =>
        parentNodeController.UpdateInformation(this);

    /// <summary>
    /// Increments current node tier. Unlocks a locked node.
    /// </summary>
    public void Unlock()
    {
        // If this node doesn't require any previous connections
        if (previousConnectionNodes.Count == 0)
        {
            // If it's not in maximum tier yet
            if (CurrentTier < nodePassives.Length)
            {
                // If player has enough arcane power
                if (parentNodeController.CurrencySO.CanSpend(CurrencyType.ArcanePower, nodePassives[CurrentTier].Cost))
                {
                    // Increments tier, spends money, updates UI to next tier
                    CurrentTier++;
                    parentNodeController.CurrencySO.SpendCurrency(CurrencyType.ArcanePower, nodePassives[CurrentTier-1].Cost);
                    UpdateUI();
                    parentNodeController.UpdateInformation(this);
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
            parentNodeController.CurrencySO.CanSpend(CurrencyType.ArcanePower, nodePassives[CurrentTier].Cost))
        {
            // If it's not in maximum tier yet
            if (CurrentTier < nodePassives.Length)
            {
                // If player has enough arcane power
                if (parentNodeController.CurrencySO.CanSpend(CurrencyType.ArcanePower, nodePassives[CurrentTier].Cost))
                {
                    // Increments tier, spends money, updates UI to next tier
                    CurrentTier++;
                    parentNodeController.CurrencySO.SpendCurrency(CurrencyType.ArcanePower, nodePassives[CurrentTier - 1].Cost);
                    UpdateUI();
                    parentNodeController.UpdateInformation(this);
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
            nodeImage.color = parentNodeController.UnlockedColor;

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
        nodeName.text = nodePassives[CurrentTier - 1].Name;
        nodeTier.text = nodePassives[CurrentTier - 1].Tier.ToString() + " / " + nodePassives.Length;
        nodeImage.sprite = nodePassives[CurrentTier - 1].Icon;

        if (CurrentTier == nodePassives.Length)
        {
            nodeImage.color = parentNodeController.LockedColor;
        }
    }

    /// <summary>
    /// Grows a connection line from current node to a target node.
    /// </summary>
    /// <param name="targetGO">Target node to grow line to.</param>
    /// <returns>Null.</returns>
    public IEnumerator GrowConnectionLine(GameObject targetGO)
    {
        // Disables button
        Button button = GetComponent<Button>();
        Button targetButton = targetGO.GetComponent<Button>();
        button.enabled = false;
        targetButton.enabled = false;

        // If a line with this name already exists, it destroys it
        Destroy(this.gameObject.transform.parent.Find(this.gameObject.name + " connect to " + targetGO.gameObject.name).gameObject);

        // Creates a line
        GameObject connectionLineGO = 
            Instantiate(parentNodeController.ConnectionLinePrefab, transform.position, Quaternion.identity);
        connectionLineGO.transform.SetParent(this.gameObject.transform.parent);
        connectionLineGO.transform.SetAsFirstSibling();
        connectionLineGO.name = this.gameObject.name + " connect to " + targetGO.gameObject.name;

        // Gets image and line and colors it
        Image connectionLineImage = connectionLineGO.GetComponent<Image>();
        connectionLineImage.color = parentNodeController.UnlockedColor;

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
            currentLineWidth += Time.deltaTime * 1100f;
            connectionLine.sizeDelta = new Vector2(currentLineWidth, 5f);
        }

        // Enables button back
        button.enabled = true;
        targetButton.enabled = true;
    }

    private void CreateInstantConnectionLine(GameObject targetGO)
    {
        // Creates a line
        GameObject connectionLineGO = 
            Instantiate(parentNodeController.ConnectionLinePrefab, transform.position, Quaternion.identity);
        connectionLineGO.transform.SetParent(this.gameObject.transform.parent);
        connectionLineGO.transform.SetAsFirstSibling();
        connectionLineGO.name = this.gameObject.name + " connect to " + targetGO.gameObject.name;

        // Gets image and line and colors it
        Image connectionLineImage = connectionLineGO.GetComponent<Image>();
        connectionLineImage.color = parentNodeController.LockedColor;

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
