using System.Collections;
using UnityEngine;
using Cinemachine;
using ExtensionMethods;

/// <summary>
/// Class responsible for handling camera movement and rotation.
/// </summary>
public class PlayerCamera : MonoBehaviour
{
    // Components
    private Player player;
    private PlayerMovement playerMovement;
    private IInput input;
    private CinemachineVirtualCamera virtualCinemachine;
    private CinemachineBasicMultiChannelPerlin cinemachineNoise;

    // Camera variables
    private float cameraX;
    private float zRotation;
    private float currentZRotation;

    // Coroutines
    private YieldInstruction wffu;
    private IEnumerator runFOVUpdateCoroutine;
    private bool running;
    private float runningCurrentTime;

    private void Awake()
    {
        player = GetComponent<Player>();
        playerMovement = GetComponent<PlayerMovement>();
        input = FindObjectOfType<PlayerInputCustom>();
        virtualCinemachine = GetComponentInChildren<CinemachineVirtualCamera>();
        cinemachineNoise = virtualCinemachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        wffu = new WaitForFixedUpdate();
        zRotation = 0;
        cameraX = 0;
    }

    private void Start()
    {
        virtualCinemachine.m_Lens.FieldOfView = player.Values.DefaultFOV;
    }

    private void OnEnable() =>
        playerMovement.EventRun += PlayerRunningEvent;

    private void OnDisable() =>
        playerMovement.EventRun -= PlayerRunningEvent;

    /// <summary>
    /// Sets running variable.
    /// </summary>
    private void Update()
    {
        // Sets running this way so the change on FoV coroutine is not too abrupt
        // when the player changes directions (cause movement is change to 0 for a frame)
        if (input.Movement.magnitude > 0 &&
            playerMovement.Running)
        {
            runningCurrentTime = Time.time;
            running = true;
        }
        else
        {
            if (Time.time - runningCurrentTime > 0.1f)
            {
                running = false;
            }
        }
    }


    private void FixedUpdate()
    {
        CameraControl();
        CameraTilt();
    }

    private void CameraControl()
    {
        // This player prefs with mouse speed is temp for delivery, don't forget to change later
        ////////////////////////////////////////////////////////////////////////////////////////

        // Rotates camera on X axis (vertical)
        cameraX -= input.Camera.y * player.Values.CameraSpeed *
            PlayerPrefs.GetFloat("Mouse", 1) * Time.fixedDeltaTime;
        cameraX = Mathf.Clamp(
            cameraX, player.Values.CameraRange.x, player.Values.CameraRange.y);

        virtualCinemachine.transform.eulerAngles =
            new Vector3(cameraX, 
            virtualCinemachine.transform.eulerAngles.y, virtualCinemachine.transform.eulerAngles.z);

        // Rotates the PLAYER on Y axis (horizontal)
        transform.Rotate(input.Camera.x * player.Values.CameraSpeed * 
            PlayerPrefs.GetFloat("Mouse", 1) * Time.fixedDeltaTime * Vector3.up);
    }

    /// <summary>
    /// Tilts the camera while strafing.
    /// </summary>
    private void CameraTilt()
    {
        if (input.Movement.x > 0)
        {
            zRotation = Mathf.SmoothDamp(
                zRotation, -player.Values.CameraTilt, ref currentZRotation, Time.fixedDeltaTime * player.Values.CameraTiltSpeed);
        }
        else if (input.Movement.x < 0)
        {
            zRotation = Mathf.SmoothDamp(
                zRotation, player.Values.CameraTilt, ref currentZRotation, Time.fixedDeltaTime * player.Values.CameraTiltSpeed);
        }
        else
        {
            zRotation = Mathf.SmoothDamp(
                zRotation, 0, ref currentZRotation, Time.fixedDeltaTime * player.Values.CameraTiltSpeed);
        }

        virtualCinemachine.transform.eulerAngles = 
            new Vector3(virtualCinemachine.transform.eulerAngles.x, virtualCinemachine.transform.eulerAngles.y, zRotation);
    }

    /// <summary>
    /// Triggered when the player runs.
    /// </summary>
    /// <param name="condition">Running true or false.</param>
    private void PlayerRunningEvent(bool condition)
    {
        if (runFOVUpdateCoroutine != null) StopCoroutine(runFOVUpdateCoroutine);
        runFOVUpdateCoroutine = RunFOVUpdateCoroutine(condition);
        StartCoroutine(runFOVUpdateCoroutine);
    }

    /// <summary>
    /// Lerps field of view and camera frequency gain depending if the player is running or not.
    /// </summary>
    /// <param name="condition">Running true or false.</param>
    /// <returns>WFFU.</returns>
    private IEnumerator RunFOVUpdateCoroutine(bool condition)
    {
        if (condition) // Run true
        {
            while (virtualCinemachine.m_Lens.FieldOfView < player.Values.FOVWhileRunning)
            {
                if (running)
                {
                    virtualCinemachine.m_Lens.FieldOfView = Mathf.Lerp(
                        virtualCinemachine.m_Lens.FieldOfView, player.Values.FOVWhileRunning, Time.fixedDeltaTime * 13);

                    cinemachineNoise.m_FrequencyGain = player.Values.NoiseFrequencyValueWhileRunning;
                }

                if (running == false)
                {
                    cinemachineNoise.m_FrequencyGain = player.Values.DefaultNoiseFrequencyValue;

                    virtualCinemachine.m_Lens.FieldOfView = Mathf.Lerp(
                        virtualCinemachine.m_Lens.FieldOfView, player.Values.DefaultFOV, Time.fixedDeltaTime * 13);
                }

                if (running == false &&
                    virtualCinemachine.m_Lens.FieldOfView.Similiar(player.Values.FOVWhileRunning, 0.05f))
                {
                    PlayerRunningEvent(false);
                }

                yield return wffu;
            }
        }
        else
        {
            while (virtualCinemachine.m_Lens.FieldOfView > player.Values.DefaultFOV)
            {
                virtualCinemachine.m_Lens.FieldOfView = Mathf.Lerp(
                    virtualCinemachine.m_Lens.FieldOfView, player.Values.DefaultFOV, Time.fixedDeltaTime * 13);

                cinemachineNoise.m_FrequencyGain = player.Values.DefaultNoiseFrequencyValue;

                if (virtualCinemachine.m_Lens.FieldOfView.Similiar(player.Values.DefaultFOV, 0.05f))
                {
                    if (running)
                    {
                        PlayerRunningEvent(true);
                    }
                }

                yield return wffu;
            }
        }
    }
}
