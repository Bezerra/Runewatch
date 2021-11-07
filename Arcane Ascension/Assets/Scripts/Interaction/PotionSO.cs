using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptable object with potions information.
/// </summary>
[InlineEditor]
[CreateAssetMenu(menuName = "Loot/Potion", fileName ="Potion")]
public class PotionSO : ScriptableObject
{
    [SerializeField] private PotionType potionType;
    [Range(0f, 100f)] [SerializeField] private float percentage;

    public PotionType PotionType => potionType;
    public float Percentage => percentage;
}
