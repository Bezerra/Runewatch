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
        button.ChangeMat();
        button.textMesh.font = fontAssetSelected;
        button.audioSourceSelect.Play();
    }


    public void OnButtonExit(ButtonGroupElement button)
    {
        ResetTabs();

    }

    public void OnButtonClick(ButtonGroupElement button)
    {
        button.audioSourceClick.Play();
        ResetTabs();
        button.textMesh.font = fontAssetSelected;
    }

    public void OnButtonCancel(ButtonGroupElement button)
    {
        ResetTabs();
        button.audioSourceBack.Play();
    }

    public void ResetTabs()
    {

        foreach (ButtonGroupElement button in buttonGroup)
        {
            
            button.ResetMat();
            button.textMesh.font = fontAssetIdle;
        }
    }
}
