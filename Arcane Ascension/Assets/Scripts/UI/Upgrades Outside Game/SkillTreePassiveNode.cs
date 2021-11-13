using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SkillTreePassiveNode : MonoBehaviour
{
    [Header("For tests")]
    [SerializeField] private bool isUnlocked;

    [Header("Passive tiers of this node")]
    [SerializeField] private SkillTreePassiveSO[] nodePassive;

    [Header("Connections")]
    [SerializeField] private List<SkillTreePassiveNode> previousConnectionNodes;
    [SerializeField] private List<SkillTreePassiveNode> nextConnectionNodes;

    [Header("Colors")]
    [SerializeField] private Color unlockedColor;
    [SerializeField] private Color lockedColor;
    [HideInInspector] [SerializeField] private TextMeshProUGUI nodeName;
    [HideInInspector] [SerializeField] private TextMeshProUGUI nodeCost;
    [HideInInspector] [SerializeField] private CurrencySO currency;

    // Properties
    public List<SkillTreePassiveNode> PreviousConnectionNodes => previousConnectionNodes;
    public bool IsUnclocked => isUnlocked;

    private Image nodeImage;

    private void Awake()
    {
        nodeImage = GetComponent<Image>();
    }

    private void Start()
    {
        nodeImage.color = lockedColor;
        nodeName.text = nodePassive[0].Name;
        nodeCost.text = nodePassive[0].Cost.ToString();

        if (isUnlocked) nodeImage.color = unlockedColor;
    }

    private void OnValidate()
    {
        if (nodeName != null && nodePassive.Length > 0)
        {
            nodeName.text = nodePassive[0].Name;
            nodeCost.text = nodePassive[0].Cost.ToString();
        }
    }

    public void TryUnlock()
    {
        int requiredNodes = 0;
        foreach(SkillTreePassiveNode previousNode in PreviousConnectionNodes)
        {
            if (previousNode.IsUnclocked)
            {
                requiredNodes++;
            }
        }
        if (requiredNodes == PreviousConnectionNodes.Count)
        {
            Unlock();
        }
    }

    private void Unlock()
    {
        isUnlocked = true;
        nodeImage.color = unlockedColor;
    }
}
