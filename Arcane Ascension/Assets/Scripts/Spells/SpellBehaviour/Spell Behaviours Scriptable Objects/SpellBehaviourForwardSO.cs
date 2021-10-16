using UnityEngine;

/// <summary>
/// Scriptable object responsible for creating a spell's behaviour.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/Spell Behaviour Forward", fileName = "Spell Behaviour Forward")]
sealed public class SpellBehaviourForwardSO : SpellBehaviourAbstractOneShotSO
{
    [Header("In this spell, this variable only checks the direction of the spell")]
    [Range(15, 50)] [SerializeField] private float spellDistance;

    public override void StartBehaviour(SpellBehaviourOneShot parent)
    {
        base.StartBehaviour(parent);

        // Direction of the spell
        Ray forward = new Ray(parent.Eyes.position, parent.Eyes.forward);

        if (Physics.Raycast(forward, out RaycastHit objectHit, spellDistance)) // Creates a raycast to see if eyes are hiting something
        {
            parent.transform.LookAt(objectHit.point);
        }
        else
        {
            Vector3 finalDirection = parent.Eyes.position + parent.Eyes.forward * 15f;
            parent.transform.LookAt(finalDirection);
        }

        // Moves the spell forward
        parent.Rb.velocity = parent.transform.forward * parent.Spell.Speed;
    }

    public override void ContinuousFixedUpdateBehaviour(SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
    }

    public override void HitTriggerBehaviour(Collider other, SpellBehaviourOneShot parent)
    {
        base.DamageBehaviour(other, parent);

        base.HitTriggerBehaviour(other, parent);

        StopSpellSpeed(parent);
    }
}
