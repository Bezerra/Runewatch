using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Scriptable object responsible for playing Projectile audio on start.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Play Projectile Audio On Start", 
    fileName = "Spell Behaviour Play Projectile Audio On Start")]
sealed public class SpellBehaviourPlayProjectileAudioOnStartSO : SpellBehaviourAbstractOneShotSO
{
    public override void StartBehaviour(SpellBehaviourOneShot parent)
    {
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
