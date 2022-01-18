using System.Collections;
using UnityEngine;

/// <summary>
/// Class responsible for fading out a light on hand effect.
/// </summary>
public class HandEffectLightFade : MonoBehaviour
{
    [Range(2f, 12f)] [SerializeField] private float fadeSpeed = 6f;

    private Light lightComp;

    private float defaultValue;

    private void Awake()
    {
        lightComp = GetComponent<Light>();
        defaultValue = lightComp.intensity;
    }

    private void OnEnable()
    {
        lightComp.intensity = defaultValue;
    }

    public void DeactivateLight() => StartCoroutine(DeactivateCoroutine());

    private IEnumerator DeactivateCoroutine()
    {
        while (lightComp.intensity > 0)
        {
            lightComp.intensity -= Time.deltaTime * fadeSpeed;
            yield return null;
        }
    }
}
