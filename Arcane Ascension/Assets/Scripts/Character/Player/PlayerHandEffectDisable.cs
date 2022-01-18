using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandEffectDisable : MonoBehaviour
{
    private ParticleSystem[] particles;
    private HandEffectLightFade handEffectLight;
    private PlayerCastSpell castSpell;

    private void Awake()
    {
        particles = GetComponentsInChildren<ParticleSystem>();
        handEffectLight = GetComponentInChildren<HandEffectLightFade>();
        castSpell = FindObjectOfType<PlayerCastSpell>();
    }

    private void OnEnable()
    {
        castSpell.AttackAnimationStart += Stop;
        castSpell.AttackAnimationEnd += Play;
    }

    private void OnDisable()
    {
        castSpell.AttackAnimationStart -= Stop;
        castSpell.AttackAnimationEnd -= Play;
    }

    public void Stop()
    {
        handEffectLight.DeactivateLight();
        foreach (ParticleSystem particle in particles)
            particle.Stop();
    }

    public void Play()
    {
        handEffectLight.gameObject.SetActive(false);
        handEffectLight.gameObject.SetActive(true);
        foreach (ParticleSystem particle in particles)
            particle.Play();
    }
}
