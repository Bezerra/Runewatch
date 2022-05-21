using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeMaterial : MonoBehaviour
{
    
    [SerializeField] private Material shaderMat;
    [SerializeField] private Color hoverColor;

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
        shaderMat.SetFloat("UnscaledTime", Time.unscaledTime);
    }

    public void ChangeMat()
    {
        image.material = shaderMat;
        image.color = hoverColor;
       
    }

    public void ResetMat()
    {
        image.material = currentMat;
        image.color = currentColor;
    }
}
