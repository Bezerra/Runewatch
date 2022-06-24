using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ButtonGroupElement : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler, ICancelHandler, ISelectHandler, ISubmitHandler

{

    [Header("Visual Switch Components")]

    public ButtonGroup buttonGroup;
    public AudioSource audioSourceClick;
    public AudioSource audioSourceSelect;
    public AudioSource audioSourceBack;
    public TextMeshProUGUI textMesh;

    [Header("OnClick Event")]

    public UnityEvent onButtonClicked;
    public UnityEvent onButtonCancel;

    [Header("Material Switch on Hover")]

    [SerializeField] private Material shaderMat;
    [SerializeField] private Color hoverColor;
    [SerializeField] private Image selectedButtonFrame;
    [SerializeField] private Image selectedButtonBG;
    private Image image;

    // Start is called before the first frame update
    private void Start()
    {
        buttonGroup = GetComponentInParent<ButtonGroup>();
        image = GetComponent<Image>();
        buttonGroup.Subscribe(this);
        
    }


    void Update()
    {
        if (shaderMat)
            shaderMat.SetFloat("Custom_Time", Time.unscaledTime);    
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        buttonGroup.OnButtonClick(this);
        Click();
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
        Cancel();
    }

    public void ResetMat()
    {
        if (image != null)
            image.enabled = true;

        if (selectedButtonBG != null)
            selectedButtonBG.enabled = false;

        if (selectedButtonFrame != null)
            selectedButtonFrame.enabled = false;
    }

    public void ChangeMat()
    {

        if (image != null)
            image.enabled = false;

        if (selectedButtonBG != null)
            selectedButtonBG.enabled = true;

        if (selectedButtonFrame != null)
            selectedButtonFrame.enabled = true;

        if (selectedButtonBG != null)
        {
            selectedButtonBG.material = shaderMat;
            selectedButtonBG.color = hoverColor;
        }
    }

    public void Click()
    {
        if (onButtonClicked != null)
        {
            onButtonClicked.Invoke();
        }
    }

    public void Cancel()
    {
        if (onButtonCancel != null)
        {
            onButtonCancel.Invoke();
        }
    }


    public void OnSelect(BaseEventData eventData)
    {

        buttonGroup.OnButtonSelect(this);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        buttonGroup.OnButtonSubmit(this);

    }

    
}
