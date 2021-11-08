using System.Collections;
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
    private CinemachineVirtualCamera virtualCinemachine;
    private CinemachineBasicMultiChannelPerlin cinemachineNoise;

    private YieldInstruction wffu;
    private IEnumerator runFOVUpdateCoroutine;

    // Desired camera position + rotation
    [SerializeField] private Transform targetCameraTransform;

    // Hands
    [SerializeField] private Camera handsCamera;
    [SerializeField] private Transform hands;


    private void Awake()
    {
        player = transform.parent.GetComponentInChildren<Player>();
        playerMovement = player.GetComponent<PlayerMovement>();
        virtualCinemachine = GetComponent<CinemachineVirtualCamera>();
        cinemachineNoise = virtualCinemachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        wffu = new WaitForFixedUpdate();

        // Adds overlay camera (hands camera) to main camera stack
        // This has to happen through code because the player is not initially set on the game,
        // so when it spawns, it adds its hands camera to main camera stack.
        UniversalAdditionalCameraData cameraData = Camera.main.GetUniversalAdditionalCameraData();
        cameraData.cameraStack.Add(handsCamera);
    }

    private void Start()
    {
        virtualCinemachine.m_Lens.FieldOfView = player.Values.DefaultFOV;
    }

    private void OnEnable()
    {
        playerMovement.EventRun += RunFOVUpdate;
    }

    private void OnDisable()
    {
        playerMovement.EventRun -= RunFOVUpdate;
    }

    /// <summary>
    /// Updates camera noise.
    /// </summary>
    private void Update()
    {
        // Can't be an event because the player can be stopped pressing running key
        // ( and the camera would shake more in that situation too )
        if (playerMovement.Speed > player.Values.Speed)
            cinemachineNoise.m_FrequencyGain = player.Values.NoiseFrequencyValueWhileRunning;
        else
            cinemachineNoise.m_FrequencyGain = player.Values.DefaultNoiseFrequencyValue;
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

    private void RunFOVUpdate(bool condition)
    {
        if (runFOVUpdateCoroutine != null) StopCoroutine(runFOVUpdateCoroutine);
        runFOVUpdateCoroutine = RunFOVUpdateCoroutine(condition);
        StartCoroutine(runFOVUpdateCoroutine);
    }
    private IEnumerator RunFOVUpdateCoroutine(bool condition)
    {
        if (condition) // Run true
        {
            while (virtualCinemachine.m_Lens.FieldOfView < player.Values.FOVWhileRunning)
            {
                virtualCinemachine.m_Lens.FieldOfView = Mathf.Lerp(
                    virtualCinemachine.m_Lens.FieldOfView, player.Values.FOVWhileRunning, Time.fixedDeltaTime * 13);
                yield return wffu;
            }
        }
        else
        {
            while (virtualCinemachine.m_Lens.FieldOfView > player.Values.DefaultFOV)
            {
                virtualCinemachine.m_Lens.FieldOfView = Mathf.Lerp(
                    virtualCinemachine.m_Lens.FieldOfView, player.Values.DefaultFOV, Time.fixedDeltaTime * 13);
                yield return wffu;
            }
        }
    }
}
