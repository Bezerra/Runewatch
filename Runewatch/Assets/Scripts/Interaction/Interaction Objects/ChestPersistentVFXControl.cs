using UnityEngine;

/// <summary>
/// Turns particles into scaled time after gameobject enabled.
/// </summary>
public class ChestPersistentVFXControl : MonoBehaviour
{
    [Header("ONLY particles that are unscaled by default")]
    [SerializeField] private ParticleSystem[] particleSystems;
    private ParticleSystem.MainModule[] mainModules;

    private void Awake()
    {
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
