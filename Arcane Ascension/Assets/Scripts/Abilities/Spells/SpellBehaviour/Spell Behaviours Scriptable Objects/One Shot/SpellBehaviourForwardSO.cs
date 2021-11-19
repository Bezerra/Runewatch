using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Scriptable object responsible for moving the spell forward on start.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Forward", 
    fileName = "Spell Behaviour Forward")]
sealed public class SpellBehaviourForwardSO : SpellBehaviourAbstractOneShotSO
{
    [Header("In this spell, this variable only checks the direction of the spell")]
    [Range(15, 50)] [SerializeField] private float spellDistance;
    [SerializeField] private LayerMask layersToCheck;

    public override void StartBehaviour(SpellBehaviourOneShot parent)
    {
        // If it's the player shooting
        if (parent.AICharacter == null)
        {
            // Direction of the spell
            Ray forward = new Ray(parent.Eyes.position, parent.Eyes.forward);

            // Creates a raycast to see if eyes are hiting something
            if (Physics.Raycast(forward, out RaycastHit objectHit, spellDistance, layersToCheck))
            {
                parent.transform.LookAt(objectHit.point);
            }
            else
            {
                Vector3 finalDirection = parent.Eyes.position + parent.Eyes.forward * spellDistance;
                parent.transform.LookAt(finalDirection);
            }
        }
        else // Else if it's the enemy
        {
            if (parent.AICharacter.CurrentTarget != null)
            {
                Vector3 finalDirection =
                parent.Hand.position + parent.Hand.position.Direction(parent.AICharacter.CurrentTarget.position);
                parent.transform.LookAt(finalDirection);
            }
        }

        // Moves the spell forward and sets important variables
        parent.Rb.velocity = parent.transform.forward * parent.Spell.Speed;
        parent.SpellStartedMoving = true;
        parent.ColliderTrigger.enabled = true;
        parent.TimeSpawned = Time.time;

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
