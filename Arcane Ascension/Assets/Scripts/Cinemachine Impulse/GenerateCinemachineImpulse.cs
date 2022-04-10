using UnityEngine;
using Cinemachine;

/// <summary>
/// Class responsible for generating a cinemachine source impulse.
/// Used by every gameobject that generates impulses.
/// </summary>
[RequireComponent(typeof(CinemachineImpulseSource))]
public class GenerateCinemachineImpulse : MonoBehaviour, IReset
{
    protected Camera mainCam;
    protected CinemachineImpulseSource cinemachineSource;

    protected virtual void Awake()
    {
        mainCam = Camera.main;
        cinemachineSource = GetComponent<CinemachineImpulseSource>();
    }

    public virtual void GenerateImpulse()
    {
        if (mainCam == null) mainCam = Camera.main;
        cinemachineSource.GenerateImpulse(mainCam.transform.forward);
    }

    /// <summary>
    /// Called after pool disables all childs on level generation.
    /// </summary>
    public void ResetAfterPoolDisable()
    {
        mainCam = Camera.main;
    }
}
