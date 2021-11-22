using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class with a getter for all spells scriptable object.
/// </summary>
public class AllSpells : MonoBehaviour
{
    [SerializeField] private AllSpellsSO allSpells;

    public List<SpellSO> SpellList =>
        allSpells.SpellList;

    public SpellSO DefaultSpell => allSpells.DefaultSpell;
}
