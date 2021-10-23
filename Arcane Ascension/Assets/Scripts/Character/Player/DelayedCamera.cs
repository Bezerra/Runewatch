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
    private PlayerCastSpell playerCastSpell;
    private CinemachineBasicMultiChannelPerlin cinemachineNoise;

    private YieldInstruction wffu;
    private IEnumerator shakeCoroutine;

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
        wffu = new WaitForFixedUpdate();

        // Adds overlay camera (hands camera) to main camera stack
        // This has to happen through code because the player is not initially set on the game,
        // so when it spawns, it adds its hands camera to main camera stack.
        UniversalAdditionalCameraData cameraData = Camera.main.GetUniversalAdditionalCameraData();
        cameraData.cameraStack.Add(handsCamera);
    }

    private void OnEnable()
    {
        playerCastSpell.EventAttack += ScreenShake;
        playerCastSpell.EventCancelAttack += CancelContinuousScreenShake;
    }

    private void OnDisable()
    {
        playerCastSpell.EventAttack -= ScreenShake;
        playerCastSpell.EventCancelAttack -= CancelContinuousScreenShake;
    }

    /// <summary>
    /// Updates camera noise.
    /// </summary>
    private void Update()
    {
        // Screen not shaking
        if (shakeCoroutine == null)
        {
            // Can't be an event because the player can be stopped pressing running key
            // ( and the camera would shake more in that situation too )
            if (playerMovement.Speed > player.Values.Speed)
                cinemachineNoise.m_FrequencyGain = player.Values.NoiseFrequencyValueWhileRunning;
            else
                cinemachineNoise.m_FrequencyGain = player.Values.DefaultNoiseFrequencyValue;
        }
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

    /// <summary>
    /// Starts screen shake coroutine.
    /// </summary>
    /// <param name="castType">Type of cast.</param>
    private void ScreenShake(SpellCastType castType)
    {
        if (shakeCoroutine != null) StopCoroutine(shakeCoroutine);

        if (castType == SpellCastType.OneShotCast || castType == SpellCastType.OneShotCastWithRelease)
            shakeCoroutine = ScreenShakeOneShotCoroutine();
        else
            shakeCoroutine = ScreenShakeContinuousCoroutine();

        StartCoroutine(shakeCoroutine);
    }

    /// <summary>
    /// Shakes screen for an amount of time.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ScreenShakeOneShotCoroutine()
    {
        cinemachineNoise.m_FrequencyGain = player.Values.CameraShakeForce;

        float shakeTime = 0;
        while(shakeTime < player.Values.CameraShakeTime)
        {
            cinemachineNoise.m_FrequencyGain = 
                Mathf.Lerp(cinemachineNoise.m_FrequencyGain, player.Values.DefaultNoiseFrequencyValue, Time.deltaTime * 5);
            yield return wffu;
            shakeTime += Time.deltaTime;
        }

        shakeCoroutine = null;
        cinemachineNoise.m_FrequencyGain = player.Values.DefaultNoiseFrequencyValue;
    }

    /// <summary>
    /// Shakes screen while this is true.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ScreenShakeContinuousCoroutine()
    {
        cinemachineNoise.m_FrequencyGain = player.Values.CameraShakeForce * 0.5f;

        while (shakeCoroutine != null)
        {
            yield return wffu;
        }
    }

    /// <summary>
    /// Cance
    /// </summary>
    private void CancelContinuousScreenShake()
    {
        shakeCoroutine = null;
        cinemachineNoise.m_FrequencyGain = player.Values.DefaultNoiseFrequencyValue;
    }
}
