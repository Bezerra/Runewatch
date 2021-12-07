using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

#pragma warning disable 0414 // variable assigned but not used.

/// <summary>
/// Scriptable object with permanent skill tree passives scriptable objects.
/// </summary>
[InlineEditor]
[CreateAssetMenu(menuName = "Create Once/Passives/All Skill Tree Passives", fileName = "All Skill Tree Passives")]
public class AllSkillTreePassivesSO : ScriptableObject
{
    [OnValueChanged("UpdateID")]
    [SerializeField] private bool inspectorReadOnlyList = true;

    // List with all skill tree passives ingame
    [DisableIf("inspectorReadOnlyList", true)]
    [OnValueChanged("UpdateID")]
    [SerializeField] private List<SkillTreePassiveSO> passives;
    public List<SkillTreePassiveSO> PassivesList => passives;

    private void OnEnable()
    {
        for (int i = 0; i < passives.Count; i++)
        {
            passives[i].ID = (byte)i;
        }
    }

    private void OnValidate()
    {
        for (int i = 0; i < passives.Count; i++)
        {
            passives[i].ID = (byte)i;
        }
    }

    public void UpdateID()
    {
        for (int i = 0; i < passives.Count; i++)
        {
            passives[i].ID = (byte)i;
        }
    }
}
