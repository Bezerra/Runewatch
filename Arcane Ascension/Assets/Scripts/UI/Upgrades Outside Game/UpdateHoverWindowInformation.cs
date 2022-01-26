using System.Collections;
using UnityEngine;
using System.Text;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Class responsible updating details window information.
/// Keep in mind that most of this code will only run when the mouse is on
/// top of the skill (window information is being displayed).
/// </summary>
public class UpdateHoverWindowInformation : MonoBehaviour
{
    [SerializeField] private Image baseWindowBackground;
    [SerializeField] private TextMeshProUGUI passiveName;
    [SerializeField] private TextMeshProUGUI tier;
    [SerializeField] private TextMeshProUGUI descriptionCurrent;
    [SerializeField] private TextMeshProUGUI descriptionNext;
    [SerializeField] private Image holdToBuyBackground;
    [SerializeField] private Image holdToBuyFillBackground;
    [SerializeField] private TextMeshProUGUI holdToBuyText;
    [SerializeField] private TextMeshProUGUI descriptionNodeCost;

    // Components
    private IInput input;
    private RunSaveDataController runSaveDataController;
    private SkillTreePassiveController skillTreePassiveController;
    private SkillTreePassiveNode passiveNode;
    private Animator anim;

    private float timerToBuySkill;
    private float timerToHideWindow;
    private IEnumerator holdingToBuyCoroutine;
    private IEnumerator holdingToHideCoroutine;

    private readonly float TIMERTOHIDEWINDOW = 0.05f;

    private void Awake()
    {
        input = FindObjectOfType<PlayerInputCustom>();
        runSaveDataController = FindObjectOfType<RunSaveDataController>();
        skillTreePassiveController = GetComponentInParent<SkillTreePassiveController>();
        anim = GetComponentInParent<Animator>();
    }

    private void OnEnable()
    {
        timerToBuySkill = 0;
        input.HoldingToBuyEvent += HoldingToBuySkill;
        input.HoldingToHideEvent += HoldingToHideWindow;
    }

    private void OnDisable()
    {
        timerToBuySkill = 0;
        timerToHideWindow = 0;
        input.HoldingToBuyEvent -= HoldingToBuySkill;
        input.HoldingToHideEvent -= HoldingToHideWindow;

        foreach (SkillTreePassiveNode node in passiveNode.PreviousConnectionNodes)
            node.DeactivateNodeRequiredImage();
    }

    /// <summary>
    /// Method called when pressing splace button to hide information window.
    /// </summary>
    /// <param name="condition"></param>
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

    /// <summary>
    /// Coroutine invoked when pressing space button to hide information window.
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Method called when pressing splace button to buy a passive.
    /// </summary>
    /// <param name="condition"></param>
    public void HoldingToHideWindow(bool condition)
    {
        if (condition)
        {
            if (holdingToHideCoroutine != null) StopCoroutine(holdingToHideCoroutine);
            holdingToHideCoroutine = HoldingToHideWindowCoroutine();
            StartCoroutine(holdingToHideCoroutine);
        }
        else
        {
            if (holdingToHideCoroutine != null) StopCoroutine(holdingToHideCoroutine);
            holdingToHideCoroutine = null;
            timerToHideWindow = 0;
        }
    }

    /// <summary>
    /// Coroutine invoked when pressing space button to buy a passive.
    /// </summary>
    /// <returns></returns>
    private IEnumerator HoldingToHideWindowCoroutine()
    {
        float maxTimer = TIMERTOHIDEWINDOW;
        while (timerToHideWindow < maxTimer)
        {
            timerToHideWindow += Time.deltaTime;
            yield return null;
        }
    }

    /// <summary>
    /// Updates bar fill amount.
    /// </summary>
    private void Update()
    {
        holdToBuyFillBackground.fillAmount = timerToBuySkill;
        anim.SetBool("pressingH", timerToHideWindow >= TIMERTOHIDEWINDOW);
    }

    /// <summary>
    /// Tries to buy selected passive.
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

    /// <summary>
    /// Updates all details on information window.
    /// </summary>
    /// <param name="passiveNode">Passive node to update.</param>
    /// <param name="calledOnStart">Is this method being called on start.</param>
    public void UpdateWindowDetails(SkillTreePassiveNode passiveNode,
        bool calledOnStart = false)
    {
        // Disables deactivate image on start for all passives
        if (calledOnStart)
        {
            passiveNode.DeactivateNodeRequiredImage();
            return;
        }

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

        // Always activates 'hold to buy parent', in case it deactivated for some reason
        // Do not remove this line (this gameObject was turning off sometimes for some reason)
        holdToBuyBackground.transform.parent.gameObject.SetActive(true);

        // Next tier
        if (passiveNode.NodePassiveNext != null)
        {
            descriptionNext.text = passiveNode.NodePassiveNext.Description.ToString();

            descriptionNodeCost.text =
                "Cost: " + passiveNode.NodePassiveNext.Cost + " AP";
            if (skillTreePassiveController.CurrencySO.CanSpend(
                    CurrencyType.ArcanePower, passiveNode.NodePassiveNext.Cost))
            {
                descriptionNodeCost.color = Color.white;
            }
            else
            {
                descriptionNodeCost.color = Color.red;
            }

            int requiredNodesNextPassive = 0;
            // Checks all required nodes
            foreach (SkillTreePassiveNode previousNode in passiveNode.PreviousConnectionNodes)
            {
                if (previousNode.IsUnlocked)
                {
                    requiredNodesNextPassive++;
                }
            }
            // If all required nodes are already unlocked
            if (requiredNodesNextPassive == passiveNode.PreviousConnectionNodes.Count &&
                passiveNode.CurrentTier < passiveNode.NodePassives.Length)
            {
                // Checks if player has enough AP
                if (skillTreePassiveController.CurrencySO.CanSpend(
                    CurrencyType.ArcanePower, passiveNode.NodePassiveNext.Cost))
                {
                    // This block of code enables fill bars and text to buy a passive
                    holdToBuyBackground.enabled = true;
                    holdToBuyFillBackground.enabled = true;
                    holdToBuyText.enabled = true;

                }
                else
                {
                    // This block of code disables fill bars and text to buy a passive
                    holdToBuyBackground.enabled = false;
                    holdToBuyFillBackground.enabled = false;
                    holdToBuyText.enabled = false;
                }
            }
            else
            {
                // This block of code disables fill bars and text to buy a passive
                holdToBuyBackground.enabled = false;
                holdToBuyFillBackground.enabled = false;
                holdToBuyText.enabled = false;
            }
        }
        else
        {
            descriptionNodeCost.text = " ";
            descriptionNext.text = " ";
            descriptionNext.transform.parent.gameObject.SetActive(false);

            // This block of code disables fill bars and text to buy a passive
            holdToBuyBackground.enabled = false;
            holdToBuyFillBackground.enabled = false;
            holdToBuyText.enabled = false;
        }

        // Required Nodes
        int requiredNodesActivateImage = 0;
        // Checks all required nodes
        foreach (SkillTreePassiveNode previousNode in passiveNode.PreviousConnectionNodes)
        {
            if (previousNode.IsUnlocked)
            {
                requiredNodesActivateImage++;
            }
        }
        // If all required nodes are already unlocked
        if (requiredNodesActivateImage != passiveNode.PreviousConnectionNodes.Count)
        {
            foreach (SkillTreePassiveNode node in passiveNode.PreviousConnectionNodes)
                node.ActivateNodeRequiredImage();
        }
    }
}
