using UnityEngine;

/// <summary>
/// Class with elements damage scriptable object method to calculate damage.
/// </summary>
public class ElementsDamage : MonoBehaviour
{
    [SerializeField] private ElementsDamageSO elementsDamage;

    private static ElementsDamage instance;

    private void Awake() =>
        instance = this;

    public static float CalculateDamage(ElementType damageSource, ElementType receivingSource) =>
        instance.elementsDamage.CalculateDamage(damageSource, receivingSource);
}
