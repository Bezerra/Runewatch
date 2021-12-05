using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Scriptable object responsible for applying normal damage and regenerate leaf shield.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Apply Damage Single Target Leaf Shield",
    fileName = "Spell Behaviour Apply Damage Single Target Leaf Shield")]
public class SpellBehaviourApplyDamageSingleTargetLeafShieldSO : SpellBehaviourAbstractOneShotSO
{
    private IList<int> layersToDamage;

    public override void StartBehaviour(SpellBehaviourOneShot parent)
    {
        layersToDamage = new List<int>
        {
            Layers.EnemyLayerNum,
            Layers.EnemySensiblePointNum,
            Layers.PlayerLayerNum,
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
            parent.Spell.DamageBehaviour.Damage(parent, other);

            if (parent.WhoCast.CommonAttributes.Type == CharacterType.Player)
            {
                if (parent.WhoCast.TryGetComponent(out IHealable leafShield))
                {
                    leafShield.Heal(
                        parent.Spell.Damage(parent.WhoCast.CommonAttributes.Type), 
                        StatsType.Armor);
                }
            }
        }
        else if (layerNumber == Layers.EnemyImmuneLayerNum)
        {
            parent.Spell.DamageBehaviour.Damage(parent, other, 0);
        }
    }
}
