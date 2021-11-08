using UnityEngine;
using Cinemachine;

/// <summary>
/// Class responsible for generating a cinemachine source impulse.
/// Used by every gameobject that generates impulses.
/// </summary>
[RequireComponent(typeof(CinemachineImpulseSource))]
public class GenerateCinemachineImpulse : MonoBehaviour
{
    protected Camera mainCam;
    protected CinemachineImpulseSource cinemachineSource;

    protected virtual void Awake()
    {
        mainCam = Camera.main;
        cinemachineSource = GetComponent<CinemachineImpulseSource>();
    }
}
