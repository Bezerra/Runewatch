using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptable object with general ENEMY values.
/// </summary>
[InlineEditor]
[CreateAssetMenu(menuName = "Values/Enemy Values", fileName = "Enemy Values")]
public class EnemyValuesSO : CharacterValuesSO
{
    [Tooltip("Random Time to wait after reaching final position")]
    [RangeMinMax(0.5f, 10f)] [SerializeField] private Vector2 waitingTime;
    public Vector2 WaitingTime => waitingTime;

    [Tooltip("Random distance allowed to move")]
    [RangeMinMax(1f, 20f)] [SerializeField] private Vector2 distance;
    public Vector2 Distance => distance;

    [Tooltip("Maximum distance of vision")]
    [Range(1, 20f)] [SerializeField] private float visionRange = 15f;
    public float VisionRange => visionRange;

    [Tooltip("Minimum and maximum random delay of attack")]
    [RangeMinMax(1, 20f)] [SerializeField] private Vector2 attackDelay;
    public Vector2 AttackDelay => attackDelay;

    [Tooltip("How much does the enemy move")]
    [Range(1, 25f)] [SerializeField] private float rollMovementQuantity = 11f;
    public float RollMovementQuantity => rollMovementQuantity;

    [Tooltip("How long does the roll last")]
    [Range(0.01f, 0.5f)] [SerializeField] private float rollMovementTime = 0.175f;
    public float RollMovementTime => rollMovementTime;

    [Tooltip("Roll every X seconds (X is defined by a random number between these values")]
    [RangeMinMax(3, 15f)] [SerializeField] private Vector2 rollDelay;
    public Vector2 RollDelay => rollDelay;
}
