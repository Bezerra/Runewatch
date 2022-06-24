using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ButtonGroup : MonoBehaviour
{
    public List<ButtonGroupElement> buttonGroup;

    public TMP_FontAsset fontAssetSelected;

    public TMP_FontAsset fontAssetIdle;

    private GameObject lastSelectedGO;


    public void Subscribe(ButtonGroupElement button)
    {
        if (buttonGroup == null)
        {
            buttonGroup = new List<ButtonGroupElement>();
        }

        buttonGroup.Add(button);
    }

    public void OnEnable()
    {

        EventSystem.current.SetSelectedGameObject(buttonGroup[0].gameObject);
        buttonGroup[0].ChangeMat();
        buttonGroup[0].textMesh.font = fontAssetSelected;

    }

    private void Update()
    {
        FindLastSelectedGO();
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
    }

    public void OnButtonCancel(ButtonGroupElement button)
    {
        
        ResetTabs();
        button.audioSourceBack.Play();
    }

    public void OnButtonSelect(ButtonGroupElement button)
    {
        
        ResetTabs();
        button.ChangeMat();
        button.textMesh.font = fontAssetSelected;
        button.audioSourceSelect.Play();
        
        

    }

    public void OnButtonSubmit(ButtonGroupElement button)
    {
        button.audioSourceClick.Play();
        ResetTabs();
        button.Click();
    }


    public void FindLastSelectedGO()
    {

        if (EventSystem.current.currentSelectedGameObject != null &&
                EventSystem.current.currentSelectedGameObject != lastSelectedGO)
        {
            lastSelectedGO = EventSystem.current.currentSelectedGameObject;
        }

        // If the button is null, it selects the last selected button
        if (this.enabled == true && EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(buttonGroup[0].gameObject);
        }
        
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
