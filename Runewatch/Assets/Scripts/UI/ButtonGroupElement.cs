using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ButtonGroupElement : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler, ICancelHandler
{
    public ButtonGroup buttonGroup;

    public AudioSource audioSourceClick;

    public AudioSource audioSourceSelect;

    public AudioSource audioSourceBack;

    public TextMeshProUGUI textMesh;

    public ChangeMaterial changeMaterial;

    

    private void Start()
    {
        buttonGroup.Subscribe(this);
        changeMaterial = GetComponent<ChangeMaterial>();

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        buttonGroup.OnButtonClick(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonGroup.OnButtonEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonGroup.OnButtonExit(this);
    }

    public void OnCancel(BaseEventData eventData)
    {
        buttonGroup.OnButtonCancel(this);
    }
}
