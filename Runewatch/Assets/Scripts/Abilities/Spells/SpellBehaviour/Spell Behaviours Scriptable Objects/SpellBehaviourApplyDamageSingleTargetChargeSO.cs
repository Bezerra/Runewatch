using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable object responsible for applying normal damage.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Apply Damage Single Target Charge",
    fileName = "Spell Behaviour Apply Damage Single Target Charge")]
public class SpellBehaviourApplyDamageSingleTargetChargeSO : SpellBehaviourAbstractSO
{
    private IList<int> layersToDamage;

    public override void StartBehaviour(SpellBehaviourOneShot parent)
    {
        layersToDamage = new List<int>
        {
            Layers.EnemyLayerNum,
            Layers.EnemySensiblePointNum,
            Layers.PlayerLayerNum,
            Layers.WallsNum, // Don't delete (for spell interaction with objects on this layer)
            Layers.InteractionForSpellsNum, // Don't delete (for spell interaction with objects in this layer)
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
            Debug.Log((parent.Spell.AttackBehaviour as AttackBehaviourChargeSO).ChargeMult);
            parent.Spell.DamageBehaviour.Damage(parent, other: other, damageMultiplier: (parent.Spell.AttackBehaviour as AttackBehaviourChargeSO).ChargeMult);
        }
        else if (layerNumber == Layers.EnemyImmuneLayerNum)
        {
            parent.Spell.DamageBehaviour.Damage(parent, other: other, damageMultiplier: 0);
        }
    }
}
