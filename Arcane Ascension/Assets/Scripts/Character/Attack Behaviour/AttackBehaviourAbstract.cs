using UnityEngine;

/// <summary>
/// Scriptable object for attack behaviours.
/// </summary>
public abstract class AttackBehaviourAbstract : ScriptableObject
{
    public abstract void Attack(ISpell spell, Character character, Stats characterStats);
}
