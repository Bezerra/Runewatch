using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using ExtensionMethods;

public class ButtonGroupElement : MonoBehaviour, IPointerEnterHandler,
    IPointerClickHandler, IPointerExitHandler, ICancelHandler,
    ISelectHandler, ISubmitHandler, IMoveHandler, IDeselectHandler

{
    [SerializeField] private ButtonType buttonType;

    [Header("Arrow Buttons")]
    public GameObject leftArrow;
    public GameObject rightArrow;
    public Scrollbar scrollBar;
    public Slider sliderBar;

    [Header("Visual Switch Components")]
    private ButtonGroup buttonGroup;
    public TextMeshProUGUI textMesh;

    [Header("Button Events")]
    public UnityEvent onButtonClicked;
    public UnityEvent onButtonCancel;
    public UnityEvent onButtonSelect;
    public UnityEvent onButtonDeselect;

    [Header("Material Switch on Hover")]
    [SerializeField] private Material shaderMat;
    [SerializeField] private Color hoverColor;
    [SerializeField] private Image selectedButtonFrame;
    [SerializeField] private Image selectedButtonBG;

    private Image image;
    private float updateScrollBarValue;
    private float updateSliderBarValue;
    


    public void OnEnable()
    {
        buttonGroup = GetComponentInParent<ButtonGroup>();
        image = GetComponent<Image>();
        buttonGroup.Subscribe(this);

        //Debug.Log(EventSystem.current.currentSelectedGameObject);
        if (1 / buttonGroup.buttonGroup.Count < 0.1)
        {
            updateScrollBarValue = 0.1f;
        }
        else
        {
            updateScrollBarValue = 1 / buttonGroup.buttonGroup.Count;
        }

        if (sliderBar != null && sliderBar.minValue != -1 &&
            sliderBar.maxValue != 1)
        {
            updateSliderBarValue = FloatExtensions.Remap(sliderBar.minValue,
                sliderBar.maxValue, 0, 1, 1);
        }
        else
        {
            updateSliderBarValue = (float)0.1;
        }



        if (scrollBar != null) scrollBar.value = 1;
        
    }

    public void OnDisable()
    {
        if (scrollBar != null) scrollBar.value = 1;
        
    }

    public void Update()
    {
        if (shaderMat)
            shaderMat.SetFloat("Custom_Time", Time.unscaledTime);    
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (buttonType == ButtonType.NormalButton)
        {
            buttonGroup.OnButtonClick(this);
            Click();
        }
        
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

    public void Selected()
    {

        if (onButtonSelect != null)
        {
            onButtonSelect.Invoke();
        }
    }

    public void Deselect()
    {

        if (onButtonDeselect != null)
        {
            onButtonDeselect.Invoke();
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        buttonGroup.OnButtonSelect(this);
        Selected();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        Deselect();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (buttonType == ButtonType.NormalButton)
        {
            buttonGroup.OnButtonSubmit(this);
        }
        

    }


    public void OnMove(AxisEventData eventData)
    {
        Debug.Log(eventData.selectedObject);
        if (leftArrow != null && rightArrow != null)
        {

            if (eventData.moveDir == MoveDirection.Right)
            {
                rightArrow.GetComponent<ArrowButtonElement>().ScriptTrigger();
            }
            if (eventData.moveDir == MoveDirection.Left)
            {
                leftArrow.GetComponent<ArrowButtonElement>().ScriptTrigger();
            }

        }
        if (scrollBar != null)
        {


            if (eventData.moveDir == MoveDirection.Up)
            {
                if (scrollBar.value + updateScrollBarValue > 1)
                {
                    scrollBar.value = 1;
                }
                else
                {
                    scrollBar.value += updateScrollBarValue;
                }
            }
            if (eventData.moveDir == MoveDirection.Down)
            {
                if (scrollBar.value - updateScrollBarValue < 0)
                {
                    scrollBar.value = 0;
                }
                else
                {
                    scrollBar.value -= updateScrollBarValue;
                }
            }
        }

        if (sliderBar != null)
        {
            if (eventData.moveDir == MoveDirection.Right)
            {
                if (sliderBar.value + updateSliderBarValue > sliderBar.maxValue)
                {
                    sliderBar.value = sliderBar.maxValue;
                }
                else
                {
                    sliderBar.value += updateSliderBarValue;
                }
            }
            if (eventData.moveDir == MoveDirection.Left)
            {
                if (sliderBar.value - updateSliderBarValue < sliderBar.minValue)
                {
                    sliderBar.value = sliderBar.minValue;
                }
                else
                {
                    sliderBar.value -= updateSliderBarValue;
                }
            }
        }
    }
}
