using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

#pragma warning disable 0414 // variable assigned but not used.

/// <summary>
/// Scriptable object with all in-game run passives scriptable objects.
/// </summary>
[InlineEditor]
[CreateAssetMenu(menuName = "Create Once/Passives/All Run Passives", fileName = "All Run Passives")]
public class AllRunPassivesSO : ScriptableObject
{
    [SerializeField] private bool inspectorReadOnlyList = true;

    [DisableIf("inspectorReadOnlyList", true)]
    [OnValueChanged("UpdateID")]
    [SerializeField] private List<RunStatPassiveSO> passives;
    public List<RunStatPassiveSO> PassivesList => passives;

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

    /// <summary>
    /// Called on passives value changed.
    /// </summary>
    private void UpdateID()
    {
        for (int i = 0; i < passives.Count; i++)
        {
            passives[i].ID = (byte)i;
        }
    }
}
