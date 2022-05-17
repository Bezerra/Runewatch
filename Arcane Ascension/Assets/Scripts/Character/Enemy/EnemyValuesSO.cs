using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptable object with general ENEMY values.
/// </summary>
[InlineEditor]
[CreateAssetMenu(menuName = "Character/Values/Enemy Values", fileName = "Enemy Values")]
public class EnemyValuesSO : CharacterValuesSO
{
    [BoxGroup("Movement and Rotation")]
    [Tooltip("Random Time to wait after reaching final position")]
    [RangeMinMax(0.5f, 10f)] [SerializeField] private Vector2 waitingTime;
    public Vector2 WaitingTime => waitingTime;

    [BoxGroup("Movement and Rotation")]
    [Tooltip("Speed of enemy rotation")]
    [Range(1, 7f)] [SerializeField] private float rotationSpeed = 3f;
    public float RotationSpeed => rotationSpeed;

    [BoxGroup("Movement and Rotation")]
    [Header("Acceleration > ONLY USED < for enemies that dodge on attack")]
    [Range(1, 50f)][SerializeField] private float acceleration;
    public float Acceleration => acceleration;

    [BoxGroup("Movement and Rotation")]
    [Tooltip("Speed of agent rotation")]
    [Range(1, 7f)] [SerializeField] private float agentAngularSpeed = 500f;
    public float AgentAngularSpeed => agentAngularSpeed;

    [BoxGroup("Movement On Patrol")]
    [Tooltip("Random distance allowed to move")]
    [RangeMinMax(1f, 20f)] [SerializeField] private Vector2 distance;
    public Vector2 Distance => distance;

    [BoxGroup("Target and Chase")]
    [Tooltip("Random minim distance to keep from target")]
    [RangeMinMax(1f, 23f)] [SerializeField] private Vector2 distanceWhileRunningBackwards;
    public Vector2 DistanceWhileRunningBackwards => distanceWhileRunningBackwards;

    [BoxGroup("Target and Chase")]
    [Tooltip("Maximum distance of vision")]
    [Range(10, 30f)] [SerializeField] private float visionRange = 20f;
    public float VisionRange => visionRange;

    [BoxGroup("Attack")]
    [Tooltip("Minimum angle to cast a spell")]
    [Range(5, 25f)] [SerializeField] private float fireAllowedAngle = 20f;
    public float FireAllowedAngle => fireAllowedAngle;

    [BoxGroup("Side Movement")]
    [Tooltip("Uses dodge when player attacks")]
    [SerializeField] private bool usesDodge;
    public bool UsesDodge => usesDodge;

    [BoxGroup("Side Movement")]
    [Tooltip("Waits X seconds until the enemy moves to the side again " +
        "(X is defined by a random number between these values")]
    [RangeMinMax(0, 15f)] [SerializeField] private Vector2 sideMovementDelay;
    public Vector2 SideMovementDelay => sideMovementDelay;
}
