using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;

/// <summary>
/// Class responsible for containing a spell slot information on spellbook.
/// </summary>
public class SpellBookSpell : MonoBehaviour, IPointerDownHandler, 
    IDragHandler, IPointerUpHandler, IComparable<SpellBookSpell>
{
    [SerializeField] private Image image;
    [SerializeField] private List<SpellBookSpell> spellIcons;
    [SerializeField] private GameObject middleColumnPassiveCard;
    [SerializeField] private AbilitySpellCardText middleColumnSpellCard;

    // Components
    private ActivateSpellBook parentActivateSpellBook;
    private SpellBookSpells parentSpellBookSpells;
    private Canvas canvas;

    private Vector2 clampXPositions;

    private ISpell spell;
    /// <summary>
    /// Updates this icon spell and image.
    /// </summary>
    public ISpell Spell
    {
        get => spell;
        set
        {
            spell = value;
            if (spell != null)
            {
                image.enabled = true;
                image.sprite = Spell.Icon;
            }
        }
    }

    private void Awake()
    {
        image.enabled = false;
        parentSpellBookSpells = GetComponentInParent<SpellBookSpells>();
        parentActivateSpellBook = GetComponentInParent<ActivateSpellBook>();
        canvas = GetComponent<Canvas>();
    }

    /// <summary>
    /// While the player is dragging an icon, updates its position.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        float positionY = transform.position.y;
        Vector2 newPosition = parentActivateSpellBook.Input.MousePosition;
        newPosition.y = positionY;
        newPosition.x = Mathf.Clamp(newPosition.x, clampXPositions.x, clampXPositions.y);
        transform.position = newPosition;
    }


    /// <summary>
    /// When the player releases the mouse, sorts spell icons by theyr X position and
    /// organizes them.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        canvas.sortingOrder = 30;

        // Sorts by transform.position.x
        spellIcons.Sort();

        // Orders as first sibling (list is in reverse order, so it will set
        // the last elements as first sibling first
        foreach (SpellBookSpell spellIcon in spellIcons)
        {
            spellIcon.transform.SetAsFirstSibling();
        }

        parentSpellBookSpells.ReorganizePlayerSpells();
    }

    /// <summary>
    /// When the player presses the mouse, updates icon limits and canvas order.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        float maxX = Mathf.NegativeInfinity;
        float minX = Mathf.Infinity;
        foreach (SpellBookSpell spellIcon in spellIcons)
        {
            if (spellIcon.transform.position.x < minX) 
                minX = spellIcon.transform.position.x - 0.1f;
            if (spellIcon.transform.position.x > maxX) 
                maxX = spellIcon.transform.position.x + 0.1f;
        }

        clampXPositions =
            new Vector2(minX, maxX);

        canvas.sortingOrder = 31;
    }

    /// <summary>
    /// Method called when a spell icon is clicked.
    /// </summary>
    public void ShowMiddleCardUpdateText()
    {
        middleColumnPassiveCard.SetActive(false);
        middleColumnSpellCard.transform.parent.gameObject.SetActive(true);
        middleColumnSpellCard.UpdateInfo(Spell);
        parentActivateSpellBook.UpdateAttributesColor(Spell.Relation);

    }

    /// <summary>
    /// Compares this class to sort a list.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(SpellBookSpell other)
    {
        if (other.transform.position.x > transform.position.x) return 1;
        else if (other.transform.position.x < transform.position.x) return -1;
        else return 0;
    }
}
