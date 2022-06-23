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

    public GameObject backButton;

    private GameObject lastSelectedGO;


    public void Subscribe(ButtonGroupElement button)
    {
        if (buttonGroup == null)
        {
            buttonGroup = new List<ButtonGroupElement>();
        }

        buttonGroup.Add(button);
    }

    public void Update()
    {
        FindLastSelectedGO();
    }

    public void OnEnable()
    {

        EventSystem.current.SetSelectedGameObject(buttonGroup[0].gameObject);


    }

    public void OnButtonEnter(ButtonGroupElement button)
    {
        EventSystem.current.SetSelectedGameObject(button.gameObject);
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
        EventSystem.current.SetSelectedGameObject(backButton);
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
        ResetTabs();
        OnButtonClick(button);
    }


    public void FindLastSelectedGO()
    {
        if (EventSystem.current.currentSelectedGameObject != null &&
                EventSystem.current.currentSelectedGameObject != lastSelectedGO)
        {
            lastSelectedGO = EventSystem.current.currentSelectedGameObject;
        }
        // If the button is null, it selects the last selected button
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(lastSelectedGO);
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
