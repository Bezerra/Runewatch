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

    private void Awake()
    {
        parentActivateSpellBook = GetComponentInParent<ActivateSpellBook>();
        canvas = GetComponent<Canvas>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = parentActivateSpellBook.Input.MousePosition;
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

        foreach (SpellBookSpell go in spellIcons)
        {
            go.transform.SetAsFirstSibling();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("AHH");
        canvas.sortingOrder = 31;
    }

    public int CompareTo(SpellBookSpell other)
    {
        if (other.transform.position.x > transform.position.x) return 1;
        else if (other.transform.position.x < transform.position.x) return -1;
        else return 0;
    }
}
