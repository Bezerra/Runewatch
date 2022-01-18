using UnityEngine;

/// <summary>
/// Scriptable object responsible for applying damage while the projectile is enabled.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Play Loop Sound" +
    " On Projectile",
    fileName = "Spell Behaviour Play Loop Sound On Projectile")]
public class SpellBehaviourPlayLoopSoundOnProjectileSO : SpellBehaviourAbstractSO
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
        if (Time.time - parent.TimeSpawned > parent.Spell.DelayToDoDamage)
        {
            if (parent.PlayingSound == false)
            {
                if (parent.Spell.Sounds.Projectile != null)
                {
                    parent.AudioS.loop = true;
                    parent.Spell.Sounds.Projectile.SetOnAudioSource(parent.AudioS);
                    parent.StartCoroutine(parent.FadeInCoroutine());
                }
                parent.PlayingSound = true;
            }

            if (parent.PlayingSound)
            {
                if (parent.Spell.Sounds.Projectile != null)
                {
                    if (parent.FadingOutSound == false)
                    {
                        if (Time.time - parent.TimeSpawned > parent.Spell.MaxTime - 1f)
                        {
                            parent.StartCoroutine(parent.FadeOutCoroutine());
                            parent.FadingOutSound = true;
                        }
                    }
                }
            }
        }
    }

    public override void ContinuousFixedUpdateBehaviour(SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
    }

    public override void HitTriggerBehaviour(Collider other, SpellBehaviourOneShot parent)
    {
        if (parent.PlayingSound)
        {
            if (parent.Spell.Sounds.Projectile != null)
            {
                if (parent.FadingOutSound == false)
                {
                    parent.StartCoroutine(parent.FadeOutCoroutine());
                    parent.FadingOutSound = true;
                }
            }
        }
    }
}
