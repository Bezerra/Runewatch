using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Class responsible for setting hands camera as stack.
/// </summary>
public class PlayerHandsCamera : MonoBehaviour
{
    private Camera handsCamera;

    private void Awake()
    {
        handsCamera = GetComponent<Camera>();

        // Adds overlay camera (hands camera) to main camera stack
        // This has to happen through code because the player is not initially set on the game,
        // so when it spawns, it adds its hands camera to main camera stack.
        UniversalAdditionalCameraData cameraData = Camera.main.GetUniversalAdditionalCameraData();
        cameraData.cameraStack.Add(handsCamera);
    }
}
