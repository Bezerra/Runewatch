using UnityEngine;

/// <summary>
/// Interface for all spells.
/// </summary>
public interface ISpell: IAbility, ISpellBehaviour, ISpellDamage, ISpellSound
{
    float ManaCost { get; }
    /// <summary>
    /// Item1 is spell Prefab. Item2 is spell hit prefab. 
    /// Item 3 is spell muzzle prefab. Item 4 is hand prefab. 
    /// Item 5 is area over prefab.
    /// </summary>
    (GameObject, GameObject, GameObject, GameObject, GameObject) Prefab { get; }
}
