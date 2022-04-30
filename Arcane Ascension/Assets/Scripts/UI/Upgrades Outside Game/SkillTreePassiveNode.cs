using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System.Text;

/// <summary>
/// Class responsible for contrling the behaviour and logic of a skill tree passive node.
/// </summary>
public class SkillTreePassiveNode : MonoBehaviour
{
    private readonly float LINEGROWTHSPEED = 3f;
    private readonly float LINEHEIGHT = 5f;

    [Header("Passive tiers of this node")]
    [SerializeField] private SkillTreePassiveSO[] nodePassives;
    public SkillTreePassiveSO[] NodePassives => nodePassives;
    public byte CurrentTier { get; private set; }

    /// <summary>
    /// Returns current passive with current tier.
    /// </summary>
    public SkillTreePassiveSO NodePassive
    {
        get
        {
            if (CurrentTier == 0)
            {
                return null;
            }
            else if (CurrentTier > 0 && CurrentTier <= nodePassives.Length)
            {
                return nodePassives[CurrentTier - 1];
            }
            return null;
        }
    }

    /// <summary>
    /// Returns next passive on the node.
    /// </summary>
    public SkillTreePassiveSO NodePassiveNext
    {
        get
        {
            if (CurrentTier < nodePassives.Length)
            {
                return nodePassives[CurrentTier];
            }
            else
            {
                return null;
            }
        }
    }

    [Header("Connections")]
    [SerializeField] private List<SkillTreePassiveNode> previousConnectionNodes;
    [SerializeField] private List<SkillTreePassiveNode> nextConnectionNodes;

    [Header("Node Child Components")]
    [HideInInspector] [SerializeField] private TextMeshProUGUI nodeName;
    [HideInInspector] [SerializeField] private TextMeshProUGUI nodeTier;
    [HideInInspector] [SerializeField] private Image nodeImage;
    [HideInInspector] [SerializeField] private GameObject nodeRequired;

    // Properties
    public List<SkillTreePassiveNode> PreviousConnectionNodes => previousConnectionNodes;
    public List<SkillTreePassiveNode> NextConnectionNodes => nextConnectionNodes;
    public bool IsUnlocked { get; private set; }

    private SkillTreePassiveController skillTreePassiveController;

    [SerializeField] private UpdateHoverWindowInformation detailsWindowInformation;

    private Animator anim;

    private void Awake()
    {
        skillTreePassiveController = GetComponentInParent<SkillTreePassiveController>();
        anim = GetComponent<Animator>();
    }

    private void Start() =>
        UpdateTiersOnStart();

    private void ResetAll()
    {
        CurrentTier = 0;
        IsUnlocked = false;
    }

    /// <summary>
    /// Updates all nodes and UI on start.
    /// </summary>
    public void UpdateTiersOnStart()
    {
        ResetAll();

        // Creates connections to all nodes
        if (nextConnectionNodes.Count > 0)
        {
            foreach (SkillTreePassiveNode node in nextConnectionNodes)
            {
                CreateInstantConnectionLine(node.gameObject);
            }
        }

        // Checks if the player already has this node, from 0 to higher tiers.
        // In that case, increments the tier of the node and updates UI.
        for (int i = 0; i < nodePassives.Length; i++)
        {
            if (skillTreePassiveController.CurrentPassives.Contains(nodePassives[i].ID))
            {
                CurrentTier++;
                UpdateUI();

                anim.SetTrigger("PassiveUnlocked");
            }
        }

        // Updates details window info
        detailsWindowInformation.UpdateWindowDetails(this, true);
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

        // Updates details window info
        detailsWindowInformation.UpdateWindowDetails(this, true);
    }

    /// <summary>
    /// Increments current node tier. Unlocks a locked node. Levels node. Spends currency.
    /// </summary>
    public void Unlock()
    {
        // Increments tier, spends money, updates UI to next tier
        skillTreePassiveController.CurrencySO.SpendCurrency(
            CurrencyType.ArcanePower, NodePassiveNext.Cost);

        skillTreePassiveController.CurrentPassives.Add
            (NodePassiveNext.ID);

        CurrentTier++;

        UpdateUI();

        // Updates details window info
        detailsWindowInformation.UpdateWindowDetails(this);
    }

    /// <summary>
    /// Updates UI of current tier.
    /// </summary>
    private void UpdateUI()
    {
        if (IsUnlocked == false)
        {
            IsUnlocked = true;
            nodeImage.color = skillTreePassiveController.UnlockedColor;

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
    }

    /// <summary>
    /// Activates required images on this node and all previous required nodes.
    /// </summary>
    public void ActivateNodeRequiredImage()
    {
        nodeRequired.SetActive(true);
        foreach (SkillTreePassiveNode node in PreviousConnectionNodes)
            node.ActivateNodeRequiredImage();
    }

    /// <summary>
    /// Deactivates required images on this node and all previous required nodes.
    /// </summary>
    public void DeactivateNodeRequiredImage()
    {
        nodeRequired.SetActive(false);
        foreach (SkillTreePassiveNode node in PreviousConnectionNodes)
            node.DeactivateNodeRequiredImage();
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

        // Creates a line
        GameObject connectionLineGO = 
            Instantiate(skillTreePassiveController.ConnectionLinePrefab, transform.position, Quaternion.identity);
        connectionLineGO.transform.SetParent(this.gameObject.transform.parent);
        connectionLineGO.transform.SetAsLastSibling();
        connectionLineGO.name = this.gameObject.name + " connect to " + targetGO.name;

        // Sets passive nodes as last sibling so they get rendered on top
        targetGO.transform.SetAsLastSibling();
        transform.SetAsLastSibling();

        // Gets image and line and colors it
        Image connectionLineImage = connectionLineGO.GetComponent<Image>();
        connectionLineImage.color = skillTreePassiveController.UnlockedColor;

        // Gets a rectangle, calculates distance and rotation from this node to the next node
        RectTransform connectionLine = connectionLineGO.GetComponent<RectTransform>();
        RectTransform target = targetGO.GetComponent<RectTransform>();
        connectionLine.localScale = Vector3.one;
        float distance = Vector2.Distance(connectionLine.anchoredPosition, target.anchoredPosition);
        float angle = 
            Mathf.Atan2(target.anchoredPosition.y - connectionLine.anchoredPosition.y, 
            target.anchoredPosition.x - connectionLine.anchoredPosition.x);
        connectionLine.localEulerAngles = new Vector3(0, 0, ((180 / Mathf.PI) * angle));
        connectionLine.sizeDelta = new Vector2(0, LINEHEIGHT);

        // Increments that rectangle smoothly until the next node
        float currentLineWidth = 0;
        while (connectionLine.sizeDelta.x < distance)
        {
            yield return null;
            // All lines grow at the same speed
            currentLineWidth += (((Time.deltaTime) * distance) * LINEGROWTHSPEED);
            connectionLine.sizeDelta = new Vector2(currentLineWidth, LINEHEIGHT * 1.25f);
        }

        // Enables button back
        button.enabled = true;
        targetButton.enabled = true;
    }

    /// <summary>
    /// Instantly creates a connect line to a gameobject.
    /// </summary>
    /// <param name="targetGO"></param>
    private void CreateInstantConnectionLine(GameObject targetGO)
    {
        // Creates a line
        GameObject connectionLineGO = 
            Instantiate(skillTreePassiveController.ConnectionLinePrefab, 
            transform.position, Quaternion.identity);
        connectionLineGO.transform.SetParent(this.gameObject.transform.parent);
        connectionLineGO.transform.SetAsFirstSibling();
        connectionLineGO.name = this.gameObject.name + " connect to " + targetGO.name;

        // Gets image and line and colors it
        Image connectionLineImage = connectionLineGO.GetComponent<Image>();
        connectionLineImage.color = skillTreePassiveController.LockedColor;

        // Sets passive nodes as last sibling so they get rendered on top
        targetGO.transform.SetAsLastSibling();
        transform.SetAsLastSibling();

        // Gets a rectangle, calculates distance and rotation from this node to the next node
        RectTransform connectionLine = connectionLineGO.GetComponent<RectTransform>();
        RectTransform target = targetGO.GetComponent<RectTransform>();
        connectionLine.localScale = Vector3.one;
        float distance = Vector2.Distance(connectionLine.anchoredPosition, target.anchoredPosition);
        float angle =
            Mathf.Atan2(target.anchoredPosition.y - connectionLine.anchoredPosition.y,
            target.anchoredPosition.x - connectionLine.anchoredPosition.x);
        connectionLine.localEulerAngles = new Vector3(0, 0, ((180 / Mathf.PI) * angle));

        // Connects this node to the next node
        connectionLine.sizeDelta = new Vector2(distance, LINEHEIGHT);
    }
}
