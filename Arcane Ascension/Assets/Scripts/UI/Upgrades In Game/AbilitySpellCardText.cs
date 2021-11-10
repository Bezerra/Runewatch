using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilitySpellCardText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI spellTierText;
    [SerializeField] private TextMeshProUGUI spellDamageText;
    [SerializeField] private Image spellImage;
    [SerializeField] private TextMeshProUGUI spellNameText;
    [SerializeField] private TextMeshProUGUI spellMana;
    [SerializeField] private TextMeshProUGUI spellDescriptionText;
    [SerializeField] private Image spellTargetImage;
    [SerializeField] private Image spellElementImage;

    public void UpdateInfo(ISpell spell)
    {
        spellTierText.text = spell.Tier.ToString();
        spellDamageText.text = Mathf.Floor(spell.MinMaxDamage.x).ToString() + " - " + Mathf.Floor(spell.MinMaxDamage.y).ToString();
        spellImage.sprite = spell.Icon;
        spellNameText.text = spell.Name;
        spellMana.text = spell.ManaCost.ToString();
        spellDescriptionText.text = spell.Description;
        spellTargetImage.sprite = spell.TargetTypeIcon;
        spellElementImage.sprite = spell.ElementIcon;
    }
}
