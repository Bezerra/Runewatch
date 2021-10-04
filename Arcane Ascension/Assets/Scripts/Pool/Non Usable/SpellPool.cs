using UnityEngine;
using System;

/// <summary>
/// Serializable class for spell pools.
/// </summary>
[Serializable]
sealed public class SpellPool: BasePool
{
    public override string Name => prefab.GetComponent<SpellBehaviourAbstract>().Spell.Name;

    public override GameObject Prefab => prefab;
    public override int Size => size;

    public void Initialize(GameObject prefab, int size)
    {
        this.prefab = prefab;
        this.size = size;
    }
}
