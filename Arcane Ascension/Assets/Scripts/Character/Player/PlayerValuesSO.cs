using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptable object with general PLAYER values.
/// </summary>
[InlineEditor]
[CreateAssetMenu(menuName = "Values/Player Values", fileName = "Player Values")]
public class PlayerValuesSO : CharacterValuesSO
{
    [BoxGroup("General Values")]
    [Tooltip("Multiplies normal speed for this value")]
    [Range(1.1f, 2f)] [SerializeField] private float runningSpeedMultiplier;
    public float RunningSpeed => speed * runningSpeedMultiplier;

    [BoxGroup("Jump")]
    [Range(0, 1f)] [SerializeField] private float gravityIncrement;
    public float GravityIncrement => gravityIncrement;

    [BoxGroup("Jump")]
    [Range(1, 20f)] [SerializeField] private float jumpForce;
    public float JumpForce => jumpForce;

    [BoxGroup("Jump")]
    [Range(0, 2f)] [SerializeField] private float jumpTime;
    public float JumpTime => jumpTime;

    [BoxGroup("Dash")]
    [Tooltip("Default value = 0.33. Multiplication of time.deltaTime to reduce dash overtime.")]
    [Range(20f, 40f)][SerializeField] private float dashingTimeReducer = 33f;
    public float DashingTimeReducer => dashingTimeReducer;

    [BoxGroup("Dash")]
    [Tooltip("Default value = 0.25. Time that dash takes effect.")]
    [Range(0.05f, 0.3f)][SerializeField] private float dashingTime = 0.25f;
    public float DashingTime => dashingTime;

    [BoxGroup("Dash")]
    [Tooltip("Default value = 7.5f. Initial dash force.")]
    [Range(3, 11f)] [SerializeField] private float dashDefaultValue = 7.5f;
    public float DashDefaultValue => dashDefaultValue;

    [BoxGroup("Arms")]
    [Range(1, 15)] [SerializeField] private float handsMovementSpeed;
    public float HandsMovementSpeed => handsMovementSpeed;

    [BoxGroup("Camera")]
    [Range(10, 40)] [SerializeField] private float delayedCameraRotationSpeed;
    public float DelayedCameraRotationSpeed => delayedCameraRotationSpeed;

    [BoxGroup("Camera")]
    [RangeMinMax(-200, 200)] [SerializeField] private Vector2 cameraRange;
    public Vector2 CameraRange => cameraRange;

    [BoxGroup("Camera")]
    [Range(5, 25)] [SerializeField] private float cameraSpeed;
    public float CameraSpeed => cameraSpeed;

    [BoxGroup("Camera")]
    [Range(1f, 5f)] [SerializeField] private float noiseFrequencyValueWhileRunning = 2.5f;
    public float NoiseFrequencyValueWhileRunning => noiseFrequencyValueWhileRunning;

    [BoxGroup("Camera")]
    [Range(0.1f, 4)] [SerializeField] private float cameraTilt;
    public float CameraTilt => cameraTilt;

    [BoxGroup("Camera")]
    [Range(1, 20)] [SerializeField] private float cameraTiltSpeed;
    public float CameraTiltSpeed => cameraTiltSpeed;

    [BoxGroup("Camera")]
    [Range(40, 60)] [SerializeField] private float defaultFOV = 50;
    public float DefaultFOV => defaultFOV;

    [BoxGroup("Camera")]
    [Range(40, 60)] [SerializeField] private float fovWhileRunning = 54;
    public float FOVWhileRunning => fovWhileRunning;

    [BoxGroup("Camera/Shake")]
    [Range(0.1f, 0.5f)] [SerializeField] private float cameraShakeTime = 0.3f;
    public float CameraShakeTime => cameraShakeTime;

    [BoxGroup("Camera/Shake")]
    [Range(1f, 30f)] [SerializeField] private float cameraShakeForce = 15f;
    public float CameraShakeForce => cameraShakeForce;

    [BoxGroup("Camera/Shake")]
    [Range(0f, 1f)] [SerializeField] private float defaultNoiseFrequencyValue = 0.8f;
    public float DefaultNoiseFrequencyValue => defaultNoiseFrequencyValue;
}
