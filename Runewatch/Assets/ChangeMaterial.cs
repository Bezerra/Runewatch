using UnityEngine;
using UnityEngine.UI;

public class ChangeMaterial : MonoBehaviour
{
    
    [SerializeField] private Material shaderMat;
    [SerializeField] private Color hoverColor;
    [SerializeField] private Image selectedButtonFrame;
    [SerializeField] private Image selectedButtonBG;

    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();       
    }

    void Update()
    {
        if(shaderMat)
            shaderMat.SetFloat("Custom_Time", Time.unscaledTime);
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

    public void ResetMat()
    {
        if (image != null)
            image.enabled = true;

        if (selectedButtonBG != null)
            selectedButtonBG.enabled = false;

        if (selectedButtonFrame != null)
            selectedButtonFrame.enabled = false;
    }
}
