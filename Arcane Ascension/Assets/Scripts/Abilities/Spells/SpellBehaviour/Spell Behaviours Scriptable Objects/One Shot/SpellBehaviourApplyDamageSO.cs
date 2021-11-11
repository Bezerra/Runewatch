using UnityEngine;

/// <summary>
/// Scriptable object responsible for applying normal damage.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Apply Damage",
    fileName = "Spell Behaviour Apply Damage")]
public class SpellBehaviourApplyDamageSO : SpellBehaviourAbstractOneShotSO
{
    public override void StartBehaviour(SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
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
        if (layerNumber == Layers.EnemyLayerNum ||
            layerNumber == Layers.EnemySensiblePointNum ||
            layerNumber == Layers.PlayerLayerNum)
        {
            parent.Spell.DamageBehaviour.Damage(parent, other);
        }
        else if (layerNumber == Layers.EnemyImmuneLayerNum)
        {
            parent.Spell.DamageBehaviour.Damage(parent, other, 0);
        }
    }
}
