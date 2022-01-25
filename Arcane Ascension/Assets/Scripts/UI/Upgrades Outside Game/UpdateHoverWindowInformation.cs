using System.Collections;
using UnityEngine;
using System.Text;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Class responsible updating details window information.
/// </summary>
public class UpdateHoverWindowInformation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI passiveName;
    [SerializeField] private TextMeshProUGUI tier;
    [SerializeField] private TextMeshProUGUI descriptionCurrent;
    [SerializeField] private TextMeshProUGUI descriptionNext;
    [SerializeField] private GameObject holdToBuyParent;
    [SerializeField] private Image holdToBuyFillBackground;
    [SerializeField] private TextMeshProUGUI descriptionNodeCost;

    // Components
    private IInput input;
    private RunSaveDataController runSaveDataController;
    private SkillTreePassiveController skillTreePassiveController;
    private SkillTreePassiveNode passiveNode;

    private bool windowIsSelected;
    private float timerToBuySkill;
    private IEnumerator holdingToBuyCoroutine;

    private void Awake()
    {
        input = FindObjectOfType<PlayerInputCustom>();
        runSaveDataController = FindObjectOfType<RunSaveDataController>();
        skillTreePassiveController = GetComponentInParent<SkillTreePassiveController>();
    }

    private void OnEnable()
    {
        windowIsSelected = true;
        timerToBuySkill = 0;
        input.HoldingToBuyEvent += HoldingToBuySkill;
    }

    private void OnDisable()
    {
        windowIsSelected = false;
        timerToBuySkill = 0;
        input.HoldingToBuyEvent -= HoldingToBuySkill;
    }

    public void HoldingToBuySkill(bool condition)
    {
        if (condition)
        {
            if (holdingToBuyCoroutine != null) StopCoroutine(holdingToBuyCoroutine);
            holdingToBuyCoroutine = HoldingToBuySkillCoroutine();
            StartCoroutine(holdingToBuyCoroutine);
        }
        else
        {
            if (holdingToBuyCoroutine != null) StopCoroutine(holdingToBuyCoroutine);
            holdingToBuyCoroutine = null;
            timerToBuySkill = 0;
        }
    }

    private IEnumerator HoldingToBuySkillCoroutine()
    {
        float maxTimer = 1f;
        while(timerToBuySkill < maxTimer)
        {
            timerToBuySkill += Time.deltaTime;
            yield return null;
        }
        BuyPassive();
        HoldingToBuySkill(false);
    }

    private void Update() =>
        holdToBuyFillBackground.fillAmount = timerToBuySkill;

    /// <summary>
    /// Buy selected passive.
    /// </summary>
    private void BuyPassive()
    {
        bool canBuyPassive = false;

        // If there's not a run going on, tries to buy the skill
        if (runSaveDataController.FileExists() == false)
        {
            // Checks if the player has enough arcane power
            if (skillTreePassiveController.CurrencySO.CanSpend(
                CurrencyType.ArcanePower, passiveNode.NodePassiveNext.Cost))
            {
                // If this node doesn't require any previous connections
                if (passiveNode.PreviousConnectionNodes.Count == 0)
                {
                    // If it's not in maximum tier yet
                    if (passiveNode.CurrentTier < passiveNode.NodePassives.Length)
                    {
                        canBuyPassive = true;
                    }
                }
                else
                {
                    int requiredNodes = 0;
                    // Checks all required nodes
                    foreach (SkillTreePassiveNode node in passiveNode.PreviousConnectionNodes)
                    {
                        if (node.IsUnlocked)
                        {
                            requiredNodes++;
                        }
                    }

                    // If all required nodes are already unlocked and the player has enough arcane power
                    if (requiredNodes == passiveNode.PreviousConnectionNodes.Count)
                    {
                        // If it's not in maximum tier yet
                        if (passiveNode.CurrentTier < passiveNode.NodePassives.Length)
                        {
                            canBuyPassive = true;
                        }
                    }
                }
            }

            if (canBuyPassive)
            {
                // Unlocks passive, spends money and updates arcane power.
                passiveNode.Unlock();

                skillTreePassiveController.UpdateArcanePowerText();
                skillTreePassiveController.ControlExistingRunButton(false);

                // Updates details window info
                UpdateWindowDetails(passiveNode);
            }
        }
        // Else it will turn on a window
        else
        {
            skillTreePassiveController.ControlExistingRunButton(true);
        }
    }

    public void UpdateWindowDetails(SkillTreePassiveNode passiveNode, 
        bool calledOnStart = false)
    {
        this.passiveNode = passiveNode;

        if (passiveNode.NodePassives == null || passiveNode.NodePassives.Length < 1) return;
        string[] passiveNameSplit = passiveNode?.NodePassives?[0]?.Name.Split();

        StringBuilder passiveNameToPrint = new StringBuilder();
        // Ignores last numbers on passive name
        for (int i = 0; i < passiveNameSplit.Length - 1; i++)
        {
            if (i > 0) passiveNameToPrint.Append(" ");
            passiveNameToPrint.Append(passiveNameSplit[i]);
        }
        passiveName.text = passiveNameToPrint.ToString();
        tier.text = passiveNode.CurrentTier + " / " + passiveNode.NodePassives.Length;

        // Current effect
        if (passiveNode.CurrentTier == 0)
        {
            descriptionCurrent.text = "Passive not obtained.";
            descriptionCurrent.color = new Color(0.75f, 0.75f, 0.75f, 1f);
        }
        else
        {
            descriptionCurrent.text = passiveNode.NodePassive.Description.ToString();
            descriptionCurrent.color = Color.white;
        }

        // Next tier
        if (passiveNode.NodePassiveNext != null)
        {
            descriptionNext.text = passiveNode.NodePassiveNext.Description.ToString();
            descriptionNodeCost.text =
                "Cost: " + passiveNode.NodePassiveNext.Cost + " AP";

            // If called on start, ignores the rest
            if (calledOnStart) return;

            // Checks if requirements to buy the skill are met
            if (skillTreePassiveController.CurrencySO.CanSpend(
                    CurrencyType.ArcanePower, passiveNode.NodePassiveNext.Cost))
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
                    holdToBuyParent.SetActive(true);
                }
                else
                {
                    holdToBuyParent.SetActive(false);
                }
            }
        }
        else
        {
            descriptionNodeCost.text = " ";
            descriptionNext.text = " ";
            descriptionNext.transform.parent.gameObject.SetActive(false);
            holdToBuyParent.SetActive(false);
        }
    }
}
