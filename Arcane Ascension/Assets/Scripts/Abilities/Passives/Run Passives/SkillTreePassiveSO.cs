using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptable object responsible for creating skill tree passive abilities.
/// </summary>
[CreateAssetMenu(menuName = "Passive/Run Passive", fileName = "Passive")]
public class SkillTreePassiveSO : AbstractPassiveSO<SkillTreePassiveType>, ISkillTreePassive
{
    [BoxGroup("Passive General Attributes")]
    [SerializeField] private SkillTreePassiveType passiveType;

    //[BoxGroup("Passive General Attributes")]
    //[SerializeField] private List<UpgradeStats> upgrades;

    public override SkillTreePassiveType PassiveType => throw new NotImplementedException();
}
