using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptable object with general PLAYER values.
/// </summary>
[InlineEditor]
[CreateAssetMenu(menuName = "Values/Player Values", fileName = "Player Values")]
public class PlayerValuesSO : CharacterValuesSO
{
    [Tooltip("Multiplies normal speed for this value")]
    [Range(1.1f, 2f)] [SerializeField] private float runningSpeedMultiplier;
    public float RunningSpeed => speed * runningSpeedMultiplier;

    [Range(0, 1f)] [SerializeField] private float gravityIncrement;
    public float GravityIncrement => gravityIncrement;

    [Range(1, 20f)] [SerializeField] private float jumpForce;
    public float JumpForce => jumpForce;

    [Range(0, 2f)] [SerializeField] private float jumpTime;
    public float JumpTime => jumpTime;

    [Tooltip("Default value = 0.33. Multiplication of time.deltaTime to reduce dash overtime.")]
    [Range(20f, 40f)][SerializeField] private float dashingTimeReducer = 33f;
    public float DashingTimeReducer => dashingTimeReducer;

    [Tooltip("Default value = 0.25. Time that dash takes effect.")]
    [Range(0.05f, 0.3f)][SerializeField] private float dashingTime = 0.25f;
    public float DashingTime => dashingTime;

    [Tooltip("Default value = 7.5f. Initial dash force.")]
    [Range(3, 11f)] [SerializeField] private float dashDefaultValue = 7.5f;
    public float DashDefaultValue => dashDefaultValue;

    [Range(1, 15)] [SerializeField] private float handsMovementSpeed;
    public float HandsMovementSpeed => handsMovementSpeed;

    [Range(10, 40)] [SerializeField] private float delayedCameraRotationSpeed;
    public float DelayedCameraRotationSpeed => delayedCameraRotationSpeed;

    [RangeMinMax(-200, 200)] [SerializeField] private Vector2 cameraRange;
    public Vector2 CameraRange => cameraRange;

    [Range(5, 25)] [SerializeField] private float cameraSpeed;
    public float CameraSpeed => cameraSpeed;

    [Range(0.1f, 0.5f)][SerializeField] private float cameraShakeTime = 0.3f;
    public float CameraShakeTime => cameraShakeTime;

    [Range(1f, 30f)] [SerializeField] private float cameraShakeForce = 15f;
    public float CameraShakeForce => cameraShakeForce;

    [Range(0f, 1f)] [SerializeField] private float defaultNoiseFrequencyValue = 0.8f;
    public float DefaultNoiseFrequencyValue => defaultNoiseFrequencyValue;

    [Range(1f, 5f)] [SerializeField] private float noiseFrequencyValueWhileRunning = 2.5f;
    public float NoiseFrequencyValueWhileRunning => noiseFrequencyValueWhileRunning;


    [Range(0.1f, 4)] [SerializeField] private float cameraTilt;
    public float CameraTilt => cameraTilt;

    [Range(1, 20)] [SerializeField] private float cameraTiltSpeed;
    public float CameraTiltSpeed => cameraTiltSpeed;
}
