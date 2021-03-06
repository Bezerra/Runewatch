using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

#pragma warning disable 0414 // variable assigned but not used.

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

    [SerializeField] private List<SpellSO> monsterExclusiveSpells;
    public List<SpellSO> MonsterExclusiveSpells => monsterExclusiveSpells;

    [SerializeField] private SpellSO defaultSpell;
    public SpellSO DefaultSpell => defaultSpell;

    [SerializeField] private SpellSO meleeAttack;
    public SpellSO MeleeAttack => meleeAttack;

    private void OnEnable()
    {
        for (int i = 0; i < spells.Count; i++)
        {
            spells[i].ID = (byte)i;
        }
    }

    [ContextMenu("Order Alphabetically")]
    public void OrderAlphabetically()
    {
        spells = spells.OrderBy(i => i.Name).ToList();
    }

    private void OnValidate()
    {
        // Uncomment to order by name, and comment again to disable it
        //spells = spells.OrderBy(i => i.Name).ToList();

        for (int i = 0; i < spells.Count; i++)
        {
            spells[i].ID = (byte)i;
        }
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
