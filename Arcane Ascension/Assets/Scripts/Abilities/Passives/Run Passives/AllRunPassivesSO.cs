using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptable object with all in-game run passives scriptable objects.
/// </summary>
[InlineEditor]
[CreateAssetMenu(menuName = "Create Once/Passives/All Run Passives", fileName = "All Run Passives")]
public class AllRunPassivesSO : ScriptableObject
{
    [SerializeField] private bool inspectorReadOnlyList = true;

    [DisableIf("inspectorReadOnlyList", true)]
    [SerializeField] private List<RunPassiveSO> passives;
    public List<RunPassiveSO> PassivesList => passives;

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
}
