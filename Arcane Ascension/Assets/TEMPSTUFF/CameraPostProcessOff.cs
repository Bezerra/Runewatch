using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// TEMPORARY. ONLY USED TO TURN POST PROCESS OF WHEN GAME STARTS.
/// POST PROCESS IS THE ONE USED ON HANDSCAMERA, MAIN CAMERA SHOULD REMAIN TURNED OFF.
/// </summary>
public class CameraPostProcessOff : MonoBehaviour
{
    /// <summary>
    /// POST PROCESS IS THE ONE USED ON HANDSCAMERA, MAIN CAMERA SHOULD REMAIN TURNED OFF.
    /// </summary>
    private void Awake()
    {
        UniversalAdditionalCameraData cameraData = Camera.main.GetUniversalAdditionalCameraData();

        cameraData.renderPostProcessing = false;
    }
}
