using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

/// <summary>
/// Scriptable object with all spells scriptable objects.
/// </summary>
[InlineEditor]
[CreateAssetMenu(menuName = "Create Once/All Spells", fileName = "All Spells")]
public class AllSpellsSO : ScriptableObject
{
    [SerializeField] private bool inspectorReadOnlyList = true;

    [DisableIf("inspectorReadOnlyList", true)]
    [OnValueChanged("UpdateID")]
    [SerializeField] private List<SpellSO> spells;
    public List<SpellSO> SpellList => spells;

    private void OnEnable()
    {
        for (int i = 0; i < spells.Count; i++)
        {
            spells[i].ID = (byte)i;
        }
        spells = spells.OrderBy(i => i.ID).ToList();
    }

    private void OnValidate()
    {
        for (int i = 0; i < spells.Count; i++)
        {
            spells[i].ID = (byte)i;
        }
        spells = spells.OrderBy(i => i.ID).ToList();
    }

    /// <summary>
    /// Called on spells value changed.
    /// </summary>
    private void UpdateID()
    {
        for (int i = 0; i < spells.Count; i++)
        {
            spells[i].ID = (byte)i;
        }
    }
}
