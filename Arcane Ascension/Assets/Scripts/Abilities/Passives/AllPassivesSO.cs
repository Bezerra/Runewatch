using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptable object with all in-game passives scriptable objects.
/// </summary>
[InlineEditor]
[CreateAssetMenu(menuName = "Create Once/All Passives", fileName = "All Passives")]
public class AllPassivesSO : ScriptableObject
{
    [SerializeField] private bool inspectorReadOnlyList = true;

    [DisableIf("inspectorReadOnlyList", true)]
    [SerializeField] private List<PassiveSO> passives;
    public List<PassiveSO> PassivesList => passives;
}
