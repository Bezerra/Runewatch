using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Class responsible for updating spell cards information.
/// </summary>
public class AbilityPassiveCardText : MonoBehaviour
{
    [SerializeField] private Image passiveTierGem;
    [SerializeField] private Sprite[] TierGems;
    [SerializeField] private Image passiveImage;
    [SerializeField] private TextMeshProUGUI passiveNameText;
    [SerializeField] private TextMeshProUGUI passiveDescriptionText;

    [Header("Empty card gameobject")]
    [SerializeField] private GameObject emptyCard;

    /// <summary>
    /// Updates passive card info.
    /// </summary>
    /// <param name="passive"></param>
    public void UpdateInfo(IRunStatPassive passive)
    {
        switch (passive.Tier)
        {
            case 1:
                passiveTierGem.sprite = TierGems[1];
                break;
            case 2:
                passiveTierGem.sprite = TierGems[2];
                break;
            case 3:
                passiveTierGem.sprite = TierGems[3];
                break;
            default:
                passiveTierGem.sprite = TierGems[0];
                break;
        }
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
