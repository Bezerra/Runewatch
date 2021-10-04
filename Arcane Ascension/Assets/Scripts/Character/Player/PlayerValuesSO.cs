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

    [Range(1, 15)] [SerializeField] private float handsMovementSpeed;
    public float HandsMovementSpeed => handsMovementSpeed;

    [Range(10, 40)] [SerializeField] private float delayedCameraRotationSpeed;
    public float DelayedCameraRotationSpeed => delayedCameraRotationSpeed;

    [RangeMinMax(-200, 200)] [SerializeField] private Vector2 cameraRange;
    public Vector2 CameraRange => cameraRange;

    [Range(5, 25)] [SerializeField] private float cameraSpeed;
    public float CameraSpeed => cameraSpeed;

    [Range(1, 4)] [SerializeField] private float cameraTilt;
    public float CameraTilt => cameraTilt;

    [Range(1, 20)] [SerializeField] private float cameraTiltSpeed;
    public float CameraTiltSpeed => cameraTiltSpeed;
}
