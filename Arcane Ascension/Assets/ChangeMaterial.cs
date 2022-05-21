using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeMaterial : MonoBehaviour
{
    
    [SerializeField] private Material shaderMat;
    [SerializeField] private Color hoverColor;
    [SerializeField] private Image selectedButtonFrame;
    [SerializeField] private Image selectedButtonBG;

    private Material currentMat;
    private Color currentColor;
    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        currentMat = image.material;
        currentColor = image.color;
       
    }

    void Update()
    {
        shaderMat.SetFloat("Custom_Time", Time.unscaledTime);
    }

    public void ChangeMat()
    {
        image.enabled = false;
        selectedButtonBG.enabled = true;
        selectedButtonFrame.enabled = true;

        selectedButtonBG.material = shaderMat;
        selectedButtonBG.color = hoverColor;
       
    }

    public void ResetMat()
    {
        image.enabled = true;
        selectedButtonBG.enabled = false;
        selectedButtonFrame.enabled = false;

        //image.enabled = false;
        //image.material = currentMat;
        //image.color = currentColor;
    }
}
