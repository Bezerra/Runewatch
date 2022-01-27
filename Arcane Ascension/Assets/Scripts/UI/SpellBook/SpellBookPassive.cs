using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class responsible for containing a spell slot information on spellbook.
/// </summary>
public class SpellBookPassive : MonoBehaviour
{
    [SerializeField] private Image image;

    private IAbility passive;
    /// <summary>
    /// Updates this icon spell and image.
    /// </summary>
    public IAbility Passive
    {
        get => passive;
        set
        {
            passive = value;
            if (passive != null) image.sprite = passive.Icon;
        }
    }
}
