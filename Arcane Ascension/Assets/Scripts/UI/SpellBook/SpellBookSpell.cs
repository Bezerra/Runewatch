using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class responsible for containing a spell slot information on spellbook.
/// </summary>
public class SpellBookSpell : MonoBehaviour
{
    [SerializeField] private Image image;

    public ISpell Spell { get; set; }

    public void UpdateSpellSlotImage()
    {
        if (Spell != null)
            image.sprite = Spell.Icon;
    }
}
