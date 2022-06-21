using UnityEngine;

/// <summary>
/// Scriptable object responsible for self healing.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Self Target Spell", 
    fileName = "Spell Behaviour Self Target Spell")]
sealed public class SpellBehaviourSelfTargetSpellSO : SpellBehaviourAbstractSO
{
    public override void StartBehaviour(SpellBehaviourOneShot parent)
    {
        // Common spells variables
        parent.TimeSpawned = Time.time;
        parent.SpellStartedMoving = true;
      
        if (parent.Spell.StatusBehaviour != null)
        {
            GameObject statusGO =
                StatusBehaviourPoolCreator.Pool.InstantiateFromPool("Status Behaviour");

            if (statusGO.TryGetComponent(out StatusBehaviour statusBehaviour))
            {
                statusBehaviour.Initialize(parent.Spell, parent.WhoCast, parent.WhoCast, parent);
                statusBehaviour.TriggerStartBehaviour();
            }
        }

        if (parent.Spell.Sounds.Projectile != null)
        {
            parent.Spell.Sounds.Projectile.PlaySound(parent.AudioS);
        }
    }

    public override void ContinuousUpdateBeforeSpellBehaviour(SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
    }

    public override void ContinuousUpdateBehaviour(SpellBehaviourOneShot parent)
    {
        // Disables spell after it reached max time
        if (Time.time - parent.TimeSpawned > parent.Spell.MaxTime)
            parent.gameObject.SetActive(false);
    }

    public override void ContinuousFixedUpdateBehaviour(SpellBehaviourOneShot parent)
    {
        if (parent.WhoCast != null)
            parent.transform.position = parent.WhoCast.transform.position;
    }

    public override void HitTriggerBehaviour(Collider other, SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
    }
}
