using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptable object with all in-game run passives scriptable objects.
/// </summary>
[InlineEditor]
[CreateAssetMenu(menuName = "Create Once/All Passives", fileName = "All Passives")]
public class AllRunPassivesSO : ScriptableObject
{
    [SerializeField] private bool inspectorReadOnlyList = true;

    [DisableIf("inspectorReadOnlyList", true)]
    [SerializeField] private List<RunPassiveSO> passives;
    public List<RunPassiveSO> PassivesList => passives;
}
