using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Updates shadows options.
/// </summary>
public class ShadowsOption : ButtonArrowOption
{
    protected override void UpdateOption(float value)
    {
        UniversalAdditionalCameraData cam = Camera.main.GetUniversalAdditionalCameraData();
        switch (value)
        {
            case 0:
                cam.renderShadows = false;
                textToUpdate.text = "Off";
                break;
            case 1:
                cam.renderShadows = true;
                textToUpdate.text = "On";
                break;
        }
    }
}
