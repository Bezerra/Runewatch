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

    /// <summary>
    /// Calculates damage.
    /// </summary>
    /// <param name="damageSource">Damage source.</param>
    /// <param name="receivingSource">What is receiving the damage.</param>
    /// <returns>Returns a float with the final percentage of damage received.</returns>
    public static float CalculateDamage(ElementType damageSource, ElementType receivingSource) =>
        instance.elementsDamage.CalculateDamage(damageSource, receivingSource);
}
