using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable object responsible for seting projectile speed to zero after collision.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/Spell Behaviour Common Behaviours", fileName = "Spell Behaviour Common Behaviours")]
public class SpellBehaviourCommonBehavioursSO : SpellBehaviourAbstractOneShotSO
{
    [SerializeField] private List<SpellBehaviourAbstractSO> commonBehaviours;

    public override void StartBehaviour(SpellBehaviourOneShot parent)
    {
        foreach (SpellBehaviourAbstractOneShotSO behaviour in commonBehaviours)
            behaviour.StartBehaviour(parent);
    }

    public override void ContinuousUpdateBehaviour(SpellBehaviourOneShot parent)
    {
        foreach (SpellBehaviourAbstractOneShotSO behaviour in commonBehaviours)
            behaviour.ContinuousUpdateBehaviour(parent);
    }

    public override void ContinuousFixedUpdateBehaviour(SpellBehaviourOneShot parent)
    {
    foreach (SpellBehaviourAbstractOneShotSO behaviour in commonBehaviours)
        behaviour.ContinuousFixedUpdateBehaviour(parent);
    }

    public override void HitTriggerBehaviour(Collider other, SpellBehaviourOneShot parent)
    {
    foreach (SpellBehaviourAbstractOneShotSO behaviour in commonBehaviours)
        behaviour.HitTriggerBehaviour(other, parent);
    }
}
