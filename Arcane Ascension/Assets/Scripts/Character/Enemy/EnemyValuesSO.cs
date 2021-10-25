using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptable object with general ENEMY values.
/// </summary>
[InlineEditor]
[CreateAssetMenu(menuName = "Values/Enemy Values", fileName = "Enemy Values")]
public class EnemyValuesSO : CharacterValuesSO
{
    [BoxGroup("Movement and Rotation")]
    [Tooltip("Random Time to wait after reaching final position")]
    [RangeMinMax(0.5f, 10f)] [SerializeField] private Vector2 waitingTime;
    public Vector2 WaitingTime => waitingTime;

    [BoxGroup("Movement and Rotation")]
    [Tooltip("Random distance allowed to move")]
    [RangeMinMax(1f, 20f)] [SerializeField] private Vector2 distance;
    public Vector2 Distance => distance;

    [BoxGroup("Movement and Rotation")]
    [Tooltip("Speed of enemy rotation")]
    [Range(1, 7f)] [SerializeField] private float rotationSpeed = 3f;
    public float RotationSpeed => rotationSpeed;

    [BoxGroup("Target and Chase")]
    [Tooltip("Random minim distance to keep from target")]
    [RangeMinMax(1f, 23f)] [SerializeField] private Vector2 distanceToKeepFromTarget;
    public Vector2 DistanceToKeepFromTarget => distanceToKeepFromTarget;

    [BoxGroup("Target and Chase")]
    [Tooltip("Maximum distance of vision")]
    [Range(10, 30f)] [SerializeField] private float visionRange = 20f;
    public float VisionRange => visionRange;

    [BoxGroup("Attack")]
    [Tooltip("Minimum angle to cast a spell")]
    [Range(5, 25f)] [SerializeField] private float fireAllowedAngle = 20f;
    public float FireAllowedAngle => fireAllowedAngle;

    [BoxGroup("Attack")]
    [Tooltip("Minimum and maximum random delay of attack")]
    [RangeMinMax(1, 20f)] [SerializeField] private Vector2 attackDelay;
    public Vector2 AttackDelay => attackDelay;

    [BoxGroup("Roll")]
    [Tooltip("How much does the enemy move")]
    [Range(1, 25f)] [SerializeField] private float rollMovementQuantity = 11f;
    public float RollMovementQuantity => rollMovementQuantity;

    [BoxGroup("Roll")]
    [Tooltip("How long does the roll last")]
    [Range(0.01f, 0.5f)] [SerializeField] private float rollMovementTime = 0.175f;
    public float RollMovementTime => rollMovementTime;

    [BoxGroup("Roll")]
    [Tooltip("Roll every X seconds (X is defined by a random number between these values")]
    [RangeMinMax(3, 15f)] [SerializeField] private Vector2 rollDelay;
    public Vector2 RollDelay => rollDelay;
}
