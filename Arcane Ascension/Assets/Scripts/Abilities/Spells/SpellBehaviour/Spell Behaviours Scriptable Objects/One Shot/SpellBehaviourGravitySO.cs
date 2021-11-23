using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Scriptable object responsible for moving the spell forward on start.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Gravity", 
    fileName = "Spell Behaviour Gravity")]
sealed public class SpellBehaviourGravitySO : SpellBehaviourAbstractOneShotSO
{
    [Range(1, 10f)] [SerializeField] private float downForce = 5f;

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
        if (parent.DisableSpellAfterCollision == false)
        {
            parent.Rb.velocity += new Vector3(0, -downForce, 0) * Time.fixedDeltaTime;
            parent.transform.rotation = Quaternion.LookRotation(parent.Rb.velocity);
        }
    }

    public override void HitTriggerBehaviour(Collider other, SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
    }
}
