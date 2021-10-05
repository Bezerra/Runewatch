using System;
using UnityEngine;

/// <summary>
/// Class for all transitions. Has a decision and two possible states.
/// </summary>
[Serializable]
public class FSMTransition
{
    [SerializeField] [TextArea] private string notes;

    [SerializeField] private FSMDecision decision;
    [SerializeField] private FSMState ifTrue;
    [SerializeField] private FSMState ifFalse;

    public FSMDecision Decision => decision;
    public FSMState IfTrue => ifTrue;
    public FSMState IfFalse => ifFalse;
}
