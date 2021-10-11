using UnityEngine;
using System;

/// <summary>
/// Serializable class for spell hits and muzzles prefabs.
/// </summary>
[Serializable]
public class SpellSecondPrefabPool : BasePool
{
    public override string Name => spellPrefab.GetComponent<SpellBehaviourAbstract>().Spell.Name;

    public override GameObject Prefab => prefab;
    public override int Size => size;

    // Used only to get the spell name
    private GameObject spellPrefab;

    public void Initialize(GameObject prefab, GameObject spellPrefab, int size)
    {
        this.prefab = prefab;
        this.size = size;
        this.spellPrefab = spellPrefab;
    }
}
