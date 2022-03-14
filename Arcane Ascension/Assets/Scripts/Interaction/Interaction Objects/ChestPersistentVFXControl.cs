using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestPersistentVFXControl : MonoBehaviour
{
    [Header("ONLY particles that are unscaled by default")]
    [SerializeField] private ParticleSystem[] particleSystems;
    private ParticleSystem.MainModule[] mainModules;

    private Chest chest;

    private void Awake()
    {
        chest = GetComponentInParent<Chest>();
        mainModules = new ParticleSystem.MainModule[particleSystems.Length];
        for (int i = 0; i < particleSystems.Length; i++)
        {
            mainModules[i] = particleSystems[i].main;
        }
    }

    /// <summary>
    /// Turns scaled time to false.
    /// </summary>
    private void OnEnable()
    {
        for (int i = 0; i < mainModules.Length; i++)
        {
            mainModules[i].useUnscaledTime = false;
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < mainModules.Length; i++)
        {
            mainModules[i].useUnscaledTime = true;
        }
    }
}
