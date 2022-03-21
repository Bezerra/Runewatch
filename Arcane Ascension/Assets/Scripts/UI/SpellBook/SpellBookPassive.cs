using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class responsible for containing a spell slot information on spellbook.
/// </summary>
public class SpellBookPassive : MonoBehaviour, IComparable<SpellBookPassive>
{
    [SerializeField] private Image image;
    [SerializeField] private Image frameImage;
    [SerializeField] private GameObject middleColumnSpellCard;
    [SerializeField] private AbilityPassiveCardText middleColumnPassiveCard;

    private ActivateSpellBook parentActivateSpellBook;

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
            if (passive != null)
            {
                frameImage.enabled = true;
                image.enabled = true;
                image.sprite = passive.Icon;
            }
        }
    }

    private void Awake()
    {
        frameImage.enabled = false;
        parentActivateSpellBook = GetComponentInParent<ActivateSpellBook>();
    }

    private void OnEnable()
    {
        image.enabled = false;
    }

    /// <summary>
    /// Method called when a spell icon is clicked.
    /// </summary>
    public void ShowMiddleCardUpdateText()
    {
        middleColumnSpellCard.SetActive(false);
        middleColumnPassiveCard.transform.parent.gameObject.SetActive(true);
        middleColumnPassiveCard.UpdateInfo(Passive as IRunPassive);
        parentActivateSpellBook.UpdateAttributesColor(Passive.Relation);
    }

    /// <summary>
    /// Compares this class to sort a list.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(SpellBookPassive other)
    {
        if (Passive == null) return 1;
        if (other.Passive == null) return -1;
        return string.Compare(this.Passive.Name, other.Passive.Name);
    }
}
