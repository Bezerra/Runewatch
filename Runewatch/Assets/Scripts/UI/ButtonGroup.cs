using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonGroup : MonoBehaviour
{
    public List<ButtonGroupElement> buttonGroup;

    public TMP_FontAsset fontAssetSelected;

    public TMP_FontAsset fontAssetIdle;


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

        //Weird null dont know why
        //button.textMesh.font = fontAssetSelected;
        

        
    }


    public void OnButtonExit(ButtonGroupElement button)
    {
        ResetTabs();
    }

    public void OnButtonClick(ButtonGroupElement button)
    {
        ResetTabs();

        //Weird null dont know why
        //button.textMesh.font = fontAssetSelected;
    }

    public void OnButtonCancel(ButtonGroupElement button)
    {
        ResetTabs();
    }

    public void ResetTabs()
    {

        foreach (ButtonGroupElement button in buttonGroup)
        {
            
            button.changeMaterial.ResetMat();

            //Weird null dont know why
            //button.textMesh.font = fontAssetIdle;
        }
    }
}
