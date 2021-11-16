using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptable object with permanent skill tree passives scriptable objects.
/// </summary>
[InlineEditor]
[CreateAssetMenu(menuName = "Create Once/Passives/All Skill Tree Passives", fileName = "All Skill Tree Passives")]
public class AllSkillTreePassivesSO : ScriptableObject
{
    [OnValueChanged("UpdateID")]
    [SerializeField] private bool inspectorReadOnlyList = true;

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

    private void UpdateID()
    {
        for (int i = 0; i < passives.Count; i++)
        {
            passives[i].ID = (byte)i;
            Debug.Log(passives[i].Name + " - ID: " + passives[i].ID);
        }
    }
}
