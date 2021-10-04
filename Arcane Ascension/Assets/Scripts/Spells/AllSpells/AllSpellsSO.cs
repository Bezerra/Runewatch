using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptable object with all spells scriptable objects.
/// </summary>
[InlineEditor]
[CreateAssetMenu(menuName = "Create Once/All Spells", fileName = "All Spells")]
public class AllSpellsSO : ScriptableObject
{
    [SerializeField] private bool inspectorReadOnlyList = true;

    [DisableIf("inspectorReadOnlyList", true)]
    [SerializeField] private List<SpellSO> spells;
    public List<SpellSO> SpellList { get => spells; set => spells = value; }
}
