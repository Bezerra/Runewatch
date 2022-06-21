using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class with a getter for all spells scriptable object.
/// </summary>
public class AllSpells : MonoBehaviour
{
    [SerializeField] private bool destroyMeOnAwake = false;

    private void Awake()
    {
        // THIS CODE DESTROYS THE GAMEOBJECT IF IT'S ON PROCEDURAL GENERATION SCENE
        // BECAUSE THIS GAMEOBJECT IS ALREADY BEING CREATED ON CREATEPOOLPREFAB SCRIPT
        // ON PROC GEN SCENE
        if (destroyMeOnAwake) Destroy(gameObject);
    }

    [SerializeField] private AllSpellsSO allSpells;

    public List<SpellSO> SpellList =>
        allSpells.SpellList;

    public List<SpellSO> MonsterExclusiveSpellList =>
        allSpells.MonsterExclusiveSpells;

    public SpellSO DefaultSpell => allSpells.DefaultSpell;

    public SpellSO MeleeAttack => allSpells.MeleeAttack;
}
