using UnityEngine;

/// <summary>
/// Scriptable object responsible for spawning a status effect of the spell.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Spawn Status Behaviour",
    fileName = "Spell Behaviour Spawn Status Behaviour")]
public class SpellBehaviourSpawnStatusSO : SpellBehaviourAbstractOneShotSO
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

    /// <summary>
    /// Spawns status behaviour.
    /// </summary>
    /// <param name="other">Collider.</param>
    /// <param name="parent">Parent spell.</param>
    public override void HitTriggerBehaviour(Collider other, SpellBehaviourOneShot parent)
    {
        if (parent.Spell.StatusBehaviour != null)
        {
            GameObject statusGO = 
                StatusBehaviourPoolCreator.Pool.InstantiateFromPool("Status Behaviour");

            if (statusGO.TryGetComponent(out StatusBehaviour statusBehaviour))
            {
                statusBehaviour.Spell = parent.Spell;
                statusBehaviour.WhoCast = parent.WhoCast;
                statusBehaviour.CharacterHit = parent.CharacterHit;
                Debug.Log(parent.CharacterHit);
                statusBehaviour.TriggerStartBehaviour();
            }
        }
    }
}
