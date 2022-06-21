using UnityEngine;

/// <summary>
/// Scriptable Object responsible for playing a sound after time set on spell Speed (for area spells that take time to explode).
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Hit Behaviour/One Shot/Spell Hit Behaviour Play " +
    "Sound Spell Hit Loop", 
    fileName = "Spell Hit Play Sound Sound Spell Hit Loop")]
sealed public class SpellOnHitBehaviourPlaySoundSpellHitLoopSO : 
    SpellOnHitBehaviourAbstractSO
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
        if (Time.timeScale == 0)
        {
            parent.AudioS.Pause();
            parent.IsSoundPaused = true;
        }
        else
        {
            if (parent.IsSoundPaused == true)
            {
                parent.IsSoundPaused = false;
                parent.AudioS.Play();
            }
        }

        if (Time.time - parent.TimeSpawned > parent.Spell.DelayToDoDamage)
        {
            if (parent.PlayedSound == false)
            {
                if (parent.Spell.Sounds.Hit != null)
                {
                    parent.AudioS.loop = true;
                    parent.Spell.Sounds.Hit.SetOnAudioSource(parent.AudioS);
                    parent.StartCoroutine(parent.FadeInCoroutine());
                }
                parent.PlayedSound = true;
            }

            if (parent.PlayedSound)
            {
                if (parent.Spell.Sounds.Hit != null)
                {
                    if (parent.FadingOutSound == false)
                    {
                        if (Time.time - parent.TimeSpawned > parent.Spell.MaxTime - 0.5f)
                        {
                            parent.StartCoroutine(parent.FadeOutCoroutine());
                            parent.FadingOutSound = true;
                        }
                    }
                }
            }
        }
    }
}
