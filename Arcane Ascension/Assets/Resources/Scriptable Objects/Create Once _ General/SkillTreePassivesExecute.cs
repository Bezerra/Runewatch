using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Executes skill tree passives effects.
/// </summary>
public class SkillTreePassivesExecute : MonoBehaviour
{
    [SerializeField] private AllSkillTreePassivesSO skillTreePassives;

    private void Awake()
    {
        skillTreePassives.UpdateID();
    }
}
