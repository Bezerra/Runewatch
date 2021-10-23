using UnityEngine;
using UnityEngine.Rendering.Universal;
using Cinemachine;

/// <summary>
/// Class responsible for handling delayed camera rotation and movement towards final camera transform.
/// </summary>
public class DelayedCamera : MonoBehaviour
{
    // Components
    private Player player;
    private PlayerMovement playerMovement;
    private PlayerCastSpell playerCastSpell;
    private CinemachineBasicMultiChannelPerlin cinemachineNoise;

    // Desired camera position + rotation
    [SerializeField] private Transform targetCameraTransform;

    // Hands
    [SerializeField] private Camera handsCamera;
    [SerializeField] private Transform hands;


    private void Awake()
    {
        player = transform.parent.GetComponentInChildren<Player>();
        playerMovement = player.GetComponent<PlayerMovement>();
        playerCastSpell = player.GetComponent<PlayerCastSpell>();
        cinemachineNoise = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        // Adds overlay camera (hands camera) to main camera stack
        // This has to happen through code because the player is not initially set on the game,
        // so when it spawns, it adds its hands camera to main camera stack.
        UniversalAdditionalCameraData cameraData = Camera.main.GetUniversalAdditionalCameraData();
        cameraData.cameraStack.Add(handsCamera);
    }

    /// <summary>
    /// Updates camera noise.
    /// </summary>
    private void Update()
    {
        // Can't be an event because the player can be stopped pressing running key
        // ( and the camera would shake more in that situation too )
        if (playerMovement.Speed != player.Values.Speed)
            cinemachineNoise.m_FrequencyGain = 2.5f;
        else
            cinemachineNoise.m_FrequencyGain = 0.8f;
    }

    /// <summary>
    /// Controls camera movement.
    /// </summary>
    private void FixedUpdate()
    {
        if (player != null)
        {
            // Rotates hands towards final camera position
            hands.rotation =
                Quaternion.Lerp(
                    hands.rotation,
                    targetCameraTransform.rotation,
                    Time.fixedDeltaTime * player.Values.HandsMovementSpeed);

            // Sets delayed camera position equal to target camera transform position
            transform.position = targetCameraTransform.transform.position;

            // Lerps delayed camera rotation smoothly towards target camera rotation
            transform.rotation =
                Quaternion.Lerp(
                    transform.rotation,
                    targetCameraTransform.transform.rotation,
                    Time.fixedDeltaTime * player.Values.DelayedCameraRotationSpeed);
        }
    }
}
