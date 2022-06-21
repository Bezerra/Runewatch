using UnityEngine;

/// <summary>
/// Scriptable object responsible for fire demon immolate mechanic.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Fire Demon Immolate", 
    fileName = "Spell Behaviour Fire Demon Immolate")]
sealed public class SpellBehaviourFireDemonImmolateSO : SpellBehaviourAbstractSO
{
    public override void StartBehaviour(SpellBehaviourOneShot parent)
    {
        parent.transform.position = parent.WhoCast.transform.position;
        parent.SpellStartedMoving = true; // So update can run
        parent.Spell.AreaOfEffect = 0;
        parent.TimeSpawned = Time.time;
    }

    public override void ContinuousUpdateBeforeSpellBehaviour(SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
    }

    public override void ContinuousUpdateBehaviour(SpellBehaviourOneShot parent)
    {
        parent.Spell.AreaOfEffect += parent.Spell.Speed * Time.deltaTime;

        if (Time.time > parent.LastTimeDamaged + parent.Spell.DelayToDoDamage)
        {
            parent.Spell.DamageBehaviour.Damage(parent, parent.transform);
            parent.LastTimeDamaged = Time.time;
        }
    }

    public override void ContinuousFixedUpdateBehaviour(SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
    }

    public override void HitTriggerBehaviour(Collider other, SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
    }
}
