using UnityEngine;
using System.Collections;
using UnityEngine.VFX;

/// <summary>
/// Class for extinguishable objects.
/// </summary>
public class Extinguishable : AbstractInteractionWithSpell
{
    private VisualEffect visualEffect;
    private YieldInstruction wfs;
    private Light pointLight;

    private float lightIntensity;

    private void Awake()
    {
        visualEffect = GetComponentInChildren<VisualEffect>();
        pointLight = GetComponentInChildren<Light>();
        lightIntensity = pointLight.intensity;
        wfs = new WaitForSeconds(3);
    }

    protected override void ActionToTake()
    {
        pointLight.intensity = 0;
        visualEffect.Stop();
        StartCoroutine(StartBurning());
    }

    private IEnumerator StartBurning()
    {
        yield return wfs;
        visualEffect.Play();
        pointLight.intensity = lightIntensity;
    }
}
