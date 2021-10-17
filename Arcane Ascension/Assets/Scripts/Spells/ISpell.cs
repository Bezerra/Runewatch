using UnityEngine;

/// <summary>
/// Interface for all spells.
/// </summary>
public interface ISpell: ISpellBehaviour, ISpellDamage
{
    Texture Icon { get; }
    string Name { get; }
    byte SpellID { get; }
    float ManaCost { get; }
    /// <summary>
    /// Item1 is spell Prefab. Item2 is spell hit prefab. Item 3 is spell muzzle prefab.
    /// </summary>
    (GameObject, GameObject, GameObject, GameObject) Prefab { get; }
}
