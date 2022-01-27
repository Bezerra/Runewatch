using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using System.Collections.Generic;
using System;

/// <summary>
/// Class responsible for containing a spell slot information on spellbook.
/// </summary>
public class SpellBookSpell : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, 
    IComparable<SpellBookSpell>
{
    [SerializeField] private Image image;

    public ISpell Spell { get; set; }

    private ActivateSpellBook parentActivateSpellBook;

    [SerializeField] private List<SpellBookSpell> spellIcons;
    private Canvas canvas;

    private Vector2 clampXPositions;

    private void Awake()
    {
        parentActivateSpellBook = GetComponentInParent<ActivateSpellBook>();
        canvas = GetComponent<Canvas>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        float positionY = transform.position.y;
        Vector2 newPosition = parentActivateSpellBook.Input.MousePosition;
        newPosition.y = positionY;
        newPosition.x = Mathf.Clamp(newPosition.x, clampXPositions.x, clampXPositions.y);
        transform.position = newPosition;
    }

    public void UpdateSpellSlotImage()
    {
        if (Spell != null)
            image.sprite = Spell.Icon;
    }



    public void OnPointerUp(PointerEventData eventData)
    {
        canvas.sortingOrder = 30;

        spellIcons.Sort();

        foreach (SpellBookSpell spellIcon in spellIcons)
        {
            spellIcon.transform.SetAsFirstSibling();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        float maxX = Mathf.NegativeInfinity;
        float minX = Mathf.Infinity;
        foreach (SpellBookSpell spellIcon in spellIcons)
        {
            if (spellIcon.transform.position.x < minX) minX = spellIcon.transform.position.x - 0.1f;
            if (spellIcon.transform.position.x > maxX) maxX = spellIcon.transform.position.x + 0.1f;
        }

        clampXPositions =
            new Vector2(minX, maxX);

        canvas.sortingOrder = 31;
    }

    public int CompareTo(SpellBookSpell other)
    {
        if (other.transform.position.x > transform.position.x) return 1;
        else if (other.transform.position.x < transform.position.x) return -1;
        else return 0;
    }
}
