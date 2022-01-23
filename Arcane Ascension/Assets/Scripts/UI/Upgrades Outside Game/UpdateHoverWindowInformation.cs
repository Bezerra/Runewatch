using UnityEngine;
using System.Text;
using TMPro;

/// <summary>
/// Class responsible updating details window information.
/// </summary>
public class UpdateHoverWindowInformation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI windowName;
    [SerializeField] private TextMeshProUGUI windowTier;
    [SerializeField] private TextMeshProUGUI windowDescriptionCurrent;
    [SerializeField] private TextMeshProUGUI windowDescriptionNext;

    public void UpdateWindowDetails(SkillTreePassiveNode passiveNode)
    {
        if (passiveNode.NodePassives == null || passiveNode.NodePassives.Length < 1) return;
        string[] passiveNameSplit = passiveNode?.NodePassives?[0]?.Name.Split();

        StringBuilder passiveNameToPrint = new StringBuilder();
        // Ignores last numbers on passive name
        for (int i = 0; i < passiveNameSplit.Length - 1; i++)
        {
            if (i > 0) passiveNameToPrint.Append(" ");
            passiveNameToPrint.Append(passiveNameSplit[i]);
        }
        windowName.text = passiveNameToPrint.ToString();
        windowTier.text = passiveNode.CurrentTier + " / " + passiveNode.NodePassives.Length;

        // Current effect
        if (passiveNode.CurrentTier == 0)
        {
            windowDescriptionCurrent.text = "Passive not obtained.";
            windowDescriptionCurrent.color = new Color(0.75f, 0.75f, 0.75f, 1f);
        }
        else
        {
            windowDescriptionCurrent.text = passiveNode.NodePassive.Description.ToString();
            windowDescriptionCurrent.color = Color.white;
        }

        // Next tier effect
        if (passiveNode.NodePassiveNext != null)
        {
            windowDescriptionNext.text = passiveNode.NodePassiveNext.Description.ToString();
        }
        else
        {
            windowDescriptionNext.text = " ";
            windowDescriptionNext.transform.parent.gameObject.SetActive(false);
        }
    }
}
