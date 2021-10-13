using UnityEngine;

/// <summary>
/// Scriptable object for attack behaviours.
/// </summary>
public abstract class AttackBehaviourAbstractSO : ScriptableObject
{
    public abstract void Attack(ISpell spell, Character character, Stats characterStats);
}
