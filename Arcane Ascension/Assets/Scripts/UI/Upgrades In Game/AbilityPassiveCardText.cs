using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Class responsible for updating spell cards information.
/// </summary>
public class AbilityPassiveCardText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI passiveTierText;
    [SerializeField] private Image passiveImage;
    [SerializeField] private TextMeshProUGUI passiveNameText;
    [SerializeField] private TextMeshProUGUI passiveDescriptionText;

    [Header("Empty card gameobject")]
    [SerializeField] private GameObject emptyCard;

    /// <summary>
    /// Updates passive card info.
    /// </summary>
    /// <param name="passive"></param>
    public void UpdateInfo(IPassive passive)
    {
        passiveTierText.text = passive.Tier.ToString();
        passiveImage.sprite = passive.Icon;
        passiveNameText.text = passive.Name;
        passiveDescriptionText.text = passive.Description;
    }

    public void ShowEmptyCard(bool show)
    {
        if (show) emptyCard.SetActive(true);
        else emptyCard.SetActive(false);
    }
}
