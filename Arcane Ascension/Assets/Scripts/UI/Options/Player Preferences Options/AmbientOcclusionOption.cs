using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Updates ambient occlusion options.
/// WARNING: NUMBER OF ARRAY IS SET TO 0 BECAUSE WE ONLY HAVE ONE FORWARD RENDERER DATA.
/// </summary>
public class AmbientOcclusionOption : ButtonArrowOption
{
    [SerializeField] private ForwardRendererData urpRenderer;

    protected override void UpdateOption(float value)
    {
        switch (value)
        {
            case 0:
                urpRenderer.rendererFeatures[0].SetActive(false);
                textToUpdate.text = "Off";
                break;

            case 1:
                urpRenderer.rendererFeatures[0].SetActive(true);
                textToUpdate.text = "On";
                break;
        }
    }
}
