using UnityEngine;

/// <summary>
/// Scriptable Object responsible for playing a sound after time set on spell Speed (for area spells that take time to explode).
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Hit Behaviour/One Shot/Spell Hit Behaviour Play Sound After Time With Speed", 
    fileName = "Spell Hit Play Sound After Time With Speed")]
sealed public class SpellOnHitBehaviourPlaySoundAfterTimeAoEReleaseSO : SpellOnHitBehaviourAbstractOneShotSO
{
    /// <summary>
    /// Executed when the spell is enabled.
    /// </summary>
    /// <param name="parent">Parent SpellOnHit monobehaviour.</param>
    public override void StartBehaviour(SpellOnHitBehaviourOneShot parent)
    {
        // Left blank on purpose
    }

    /// <summary>
    /// Executed every update.
    /// </summary>
    /// <param name="parent">Parent SpellOnHit monobehaviour.</param>
    public override void ContinuousUpdateBehaviour(SpellOnHitBehaviourOneShot parent)
    {
        if (Time.time - parent.TimeSpawned > parent.Spell.Speed)
        {
            if (parent.PlayedSound == false)
            {
                if (parent.Spell.Sounds.Hit != null)
                {
                    parent.Spell.Sounds.Hit.PlaySound(parent.AudioS);
                }
                parent.PlayedSound = true;
            }
        }
    }
}
