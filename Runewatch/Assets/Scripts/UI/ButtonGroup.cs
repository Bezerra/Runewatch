using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonGroup : MonoBehaviour
{
    public List<ButtonGroupElement> buttonGroup;

    public ButtonGroupElement selectedButton;


    public void Subscribe(ButtonGroupElement button)
    {
        if (buttonGroup == null)
        {
            buttonGroup = new List<ButtonGroupElement>();
        }

        buttonGroup.Add(button);
    }

    public void OnButtonEnter(ButtonGroupElement button)
    {
        ResetTabs();
        button.changeMaterial.ChangeMat();
    }

    public void OnButtonExit(ButtonGroupElement button)
    {

    }

    public void OnButtonSelected(ButtonGroupElement button)
    {
        selectedButton = button;
        ResetTabs();
        button.changeMaterial.ChangeMat();
    }

    public void OnButtonCancel(ButtonGroupElement button)
    {

    }

    public void ResetTabs()
    {
        foreach (ButtonGroupElement button in buttonGroup)
        {
            button.changeMaterial.ResetMat();
        }
    }
}
