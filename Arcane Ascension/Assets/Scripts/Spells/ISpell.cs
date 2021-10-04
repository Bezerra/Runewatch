using UnityEngine;

/// <summary>
/// Interface for all spells.
/// </summary>
public interface ISpell : ISpellBehaviour, ISpellDamage
{
    Texture Icon { get; }
    string Name { get; }
    byte SpellID { get; }
    float ManaCost { get; }
    GameObject Prefab { get; }
}
