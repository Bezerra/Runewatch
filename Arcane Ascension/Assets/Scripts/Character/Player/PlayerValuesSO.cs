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
    [Tooltip("Running speed.")]
    [Range(5f, 15)] [SerializeField] private float runningSpeed = 10f;
    public float RunningSpeed => runningSpeed;

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
    [Range(5, 10)] [SerializeField] private float dashDefaultValue = 7.5f;
    public float DashDefaultValue => dashDefaultValue;

    [BoxGroup("Dash")]
    [Tooltip("Default value = 5f. Time to get a dash charge.")]
    [Range(1, 10)] [SerializeField] private float timeToGetCharge = 5f;
    public float TimeToGetCharge => timeToGetCharge;

    [BoxGroup("Arms")]
    [Range(1, 15)] [SerializeField] private float handsMovementSpeed;
    public float HandsMovementSpeed => handsMovementSpeed;

    [BoxGroup("Camera")]
    [Range(2, 10f)] [SerializeField] private float cameraForceOnDash = 4f;
    public float CameraForceOnDash => cameraForceOnDash;

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
    [Range(1f, 10f)] [SerializeField] private float noiseFrequencyValueWhileRunning = 2.5f;
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
    [Header("Not being used atm, can delete later.")]
    [Range(0f, 10f)] [SerializeField] private float amplitudeGain = 0.3f;
    public float AmplitudeGain => amplitudeGain;

    [BoxGroup("Camera/Shake")]
    [Header("Not being used atm, can delete later.")]
    [Range(0f, 10f)] [SerializeField] private float amplitudeForce = 15f;
    public float AmplitudeForce => amplitudeForce;

    [BoxGroup("Camera/Shake")]
    [Range(0f, 2f)] [SerializeField] private float defaultNoiseFrequencyValue = 0.8f;
    public float DefaultNoiseFrequencyValue => defaultNoiseFrequencyValue;

    [BoxGroup("Interaction")]
    [Range(0f, 1f)] [SerializeField] private float defaultCheckInteractionDelay = 0.2f;
    public float DefaultCheckInteractionDelay => defaultCheckInteractionDelay;

    [BoxGroup("Interaction")]
    [Range(1f, 5f)] [SerializeField] private float defaultInteractionRayLength = 2f;
    public float DefaultInteractionRayLength => defaultInteractionRayLength;
}
