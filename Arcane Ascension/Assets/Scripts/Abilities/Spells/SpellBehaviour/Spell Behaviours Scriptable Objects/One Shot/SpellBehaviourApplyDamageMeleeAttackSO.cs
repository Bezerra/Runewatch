using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Scriptable object responsible for melee attack behaviours.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Apply Damage Melee Attack", 
    fileName = "Spell Behaviour Apply Damage Melee Attack")]
sealed public class SpellBehaviourApplyDamageMeleeAttackSO : SpellBehaviourAbstractOneShotSO
{
    /// <summary>
    /// Sets variables used to do damage. Applies damage behaviour.
    /// </summary>
    /// <param name="parent">Parent spell.</param>
    public override void StartBehaviour(SpellBehaviourOneShot parent)
    {
        parent.transform.position = parent.Hand.position;

        // If it's an enemy
        if (parent.AICharacter)
        {
            // Moves the final position a little to the back so it won't be 100% in the position of the player.
            // This corrects a bug where the collision wasn't detecting if this point was exactly in the same
            // position as the player.
            parent.PositionOnHit = parent.Hand.position +
                parent.transform.position.Direction(parent.WhoCast.transform.position) * 0.75f;
        }
        else
        // If it's the player
        {
            // Moves collider a little to the front to help with player's accuracy
            parent.PositionOnHit = parent.Hand.position +
                parent.WhoCast.transform.position.Direction(parent.Hand.position) * 1.5f;
        }

        parent.SpellStartedMoving = true;
        parent.TimeSpawned = Time.time;
        parent.Spell.DamageBehaviour.Damage(parent);
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
        // Left blank on purpose
    }
}
