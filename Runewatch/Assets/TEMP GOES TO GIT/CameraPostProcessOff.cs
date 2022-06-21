using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// TEMPORARY. ONLY USED TO TURN POST PROCESS OF WHEN GAME STARTS.
/// POST PROCESS IS THE ONE USED ON HANDSCAMERA, MAIN CAMERA SHOULD REMAIN TURNED OFF.
/// </summary>
public class CameraPostProcessOff : MonoBehaviour, IFindPlayer
{
    [Header("This is used for procedural generation demonstration scene or for scenes" +
        " that do not spawn the player")]
    [SerializeField] private bool keepMainCamPostProcess;

    /// <summary>
    /// POST PROCESS IS THE ONE USED ON HANDSCAMERA, MAIN CAMERA SHOULD REMAIN TURNED OFF.
    /// </summary>
    private void Awake()
    {
        UniversalAdditionalCameraData cameraData = 
            Camera.main.GetUniversalAdditionalCameraData();

        if (keepMainCamPostProcess)
            PostProcessOn();
        else
            cameraData.renderPostProcessing = false;
    }

    public void PostProcessOn()
    {
        UniversalAdditionalCameraData cameraData = 
            Camera.main.GetUniversalAdditionalCameraData();

        cameraData.renderPostProcessing = true;
    }

    public void FindPlayer(Player player)
    {
        // Left blank on purpose
    }

    /// <summary>
    /// When the player dies, turns post process back on.
    /// </summary>
    public void PlayerLost(Player player)
    {
        PostProcessOn();
    }
}
