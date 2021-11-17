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

    [BoxGroup("Passive General Attributes")]
    [Range(0, 500)][SerializeField] private int cost;

    [BoxGroup("Passive General Attributes")]
    [Range(1, 255)] [SerializeField] private byte amountToAdd;

    public override SkillTreePassiveType PassiveType => passiveType;
    public int Cost => cost;
    public byte Amount => amountToAdd;
}
