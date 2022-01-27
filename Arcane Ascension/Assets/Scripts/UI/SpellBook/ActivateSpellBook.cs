using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateSpellBook : MonoBehaviour, IFindInput
{
    [SerializeField] private GameObject spellbook;

    private IInput input;
    private bool isSpellBookOpened;

    private void Awake()
    {
        input = FindObjectOfType<PlayerInputCustom>();
    }

    private void OnEnable()
    {
        input.SpellBook += ControlSpellBook;
    }

    private void OnDisable()
    {
        input.SpellBook -= ControlSpellBook;
    }

    private void Start()
    {
        isSpellBookOpened = false;
    }

    private void ControlSpellBook()
    {
        isSpellBookOpened = !isSpellBookOpened;

        if (isSpellBookOpened)
        {
            spellbook.SetActive(true);
            input.SwitchActionMapToUI();
        }
        else
        {
            spellbook.SetActive(false);
            input.SwitchActionMapToGameplay();
        }
    }




    public void FindInput()
    {
        if (input == null) input = FindObjectOfType<PlayerInputCustom>();
    }

    public void LostInput()
    {
        
    }
}
