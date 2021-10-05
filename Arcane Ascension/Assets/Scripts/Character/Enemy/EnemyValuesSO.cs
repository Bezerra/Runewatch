using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptable object with general ENEMY values.
/// </summary>
[InlineEditor]
[CreateAssetMenu(menuName = "Values/Enemy Values", fileName = "Enemy Values")]
public class EnemyValuesSO : CharacterValuesSO
{
    [Range(1, 20f)] [SerializeField] private float visionRange;
    public float VisionRange => visionRange;

    [Range(1, 20f)] [SerializeField] private float attackDelay;
    public float AttackDelay => attackDelay;
}
