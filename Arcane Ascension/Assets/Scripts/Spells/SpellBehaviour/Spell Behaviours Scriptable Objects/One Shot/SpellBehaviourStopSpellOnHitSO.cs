using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable object responsible for seting projectile speed to zero after collision.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Stop Spell On Hit", fileName = "Spell Behaviour Stop Spell On Hit")]
public class SpellBehaviourStopSpellOnHitSO : SpellBehaviourAbstractOneShotSO
{
    [SerializeField] private string description;
    [SerializeField] private List<int> layersToStopTheSpell;

    public override void StartBehaviour(SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
    }

    public override void ContinuousUpdateBehaviour(SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
    }

    public override void ContinuousFixedUpdateBehaviour(SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
    }

    public override void HitTriggerBehaviour(Collider other, SpellBehaviourOneShot parent)
    {
        foreach(int i in layersToStopTheSpell)
        {
            if (other.gameObject.layer == i)
                parent.Rb.velocity = Vector3.zero;
        }
    }
}
