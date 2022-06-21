using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Class responsible for updating spell cards information.
/// </summary>
public class AbilitySpellCardText : MonoBehaviour
{
    [SerializeField] private Image spellTierGem;
    [SerializeField] private Sprite[] TierGems;
    [SerializeField] private TextMeshProUGUI spellDamageText;
    [SerializeField] private TextMeshProUGUI spellDamageBehaviour;
    [SerializeField] private Image spellImage;
    [SerializeField] private TextMeshProUGUI spellNameText;
    [SerializeField] private TextMeshProUGUI spellMana;
    [SerializeField] private TextMeshProUGUI spellDescriptionText;
    [SerializeField] private Image spellElementGem;
    [SerializeField] private Sprite[] ElementGems;
    [SerializeField] private ScriptableObject DmgBehaviour;


    [Header("Empty card gameobject")]
    [SerializeField] private GameObject emptyCard;

    /// <summary>
    /// Updates spell card info.
    /// </summary>
    /// <param name="spell"></param>
    public void UpdateInfo(ISpell spell)
    {

        
        spellDamageText.text = Mathf.RoundToInt((spell.MinMaxDamage.x + spell.MinMaxDamage.y) / 2 ).ToString();
        spellImage.sprite = spell.Icon;
        spellNameText.text = spell.Name;
        spellMana.text = spell.ManaCost.ToString();
        spellDescriptionText.text = spell.Description;

        if (spell.DamageBehaviour == DmgBehaviour)
        {
            spellDamageBehaviour.text = "Area of Effect";
        }
        else if (spell.DamageBehaviour == null)
        {
            spellDamageBehaviour.text = "Self";

        }
        else
            spellDamageBehaviour.text = "Single Target";


        switch (spell.Element)

        {
            case ElementType.Ignis:
                spellElementGem.sprite = ElementGems[1];
                break;
            case ElementType.Aqua:
                spellElementGem.sprite = ElementGems[2];
                break;
            case ElementType.Terra:
                spellElementGem.sprite = ElementGems[3];
                break;
            case ElementType.Fulgur:
                spellElementGem.sprite = ElementGems[4];
                break;
            case ElementType.Natura:
                spellElementGem.sprite = ElementGems[5];
                break;
            case ElementType.Lux:
                spellElementGem.sprite = ElementGems[6];
                break;
            case ElementType.Umbra:
                spellElementGem.sprite = ElementGems[7];
                break;
            default:
                spellElementGem.sprite = ElementGems[0];
                break;
        }
        
        switch (spell.Tier)
        {
            case 1:
                spellTierGem.sprite = TierGems[0];
                break;
            case 2:
                spellTierGem.sprite = TierGems[1];
                break;
            case 3:
                spellTierGem.sprite = TierGems[2];
                break;
            default:
                spellTierGem.sprite = TierGems[3];
                break;
        }
    }

    public void ShowEmptyCard(bool show)
    {
        if (emptyCard != null)
        {
            if (show) emptyCard.SetActive(true);
            else emptyCard.SetActive(false);
        }
    }
}
