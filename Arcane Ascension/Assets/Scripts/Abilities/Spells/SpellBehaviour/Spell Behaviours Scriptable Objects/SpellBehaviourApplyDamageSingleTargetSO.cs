using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable object responsible for applying normal damage.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Apply Damage Single Target",
    fileName = "Spell Behaviour Apply Damage Single Target")]
public class SpellBehaviourApplyDamageSingleTargetSO : SpellBehaviourAbstractSO
{
    private IList<int> layersToDamage;

    public override void StartBehaviour(SpellBehaviourOneShot parent)
    {
        layersToDamage = new List<int>
        {
            Layers.EnemyLayerNum,
            Layers.EnemySensiblePointNum,
            Layers.PlayerLayerNum,
            Layers.WallsNum // Don't delete (for spell interaction with objects on this layer)
        };
    }

    public override void ContinuousUpdateBeforeSpellBehaviour(SpellBehaviourOneShot parent)
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
        int layerNumber = other.gameObject.layer;

        if (layersToDamage.Contains(layerNumber))
        {
            parent.Spell.DamageBehaviour.Damage(parent, other: other);
        }
        else if (layerNumber == Layers.EnemyImmuneLayerNum)
        {
            parent.Spell.DamageBehaviour.Damage(parent, other: other, damageMultiplier: 0);
        }
    }
}
