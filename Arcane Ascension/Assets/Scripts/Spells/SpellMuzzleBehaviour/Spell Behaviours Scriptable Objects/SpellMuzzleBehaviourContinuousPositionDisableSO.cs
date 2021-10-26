using UnityEngine;

/// <summary>
/// Scriptable Object responsible for disabling the spell muzzle gameobject.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Muzzle Behaviour/Continuous/Spell Muzzle Behaviour Position And Disable", 
    fileName = "Spell Muzzle Behaviour Position And Disable")]
public class SpellMuzzleBehaviourContinuousPositionDisableSO : SpellMuzzleBehaviourAbstractContinuousSO
{
    public override void StartBehaviour(SpellMuzzleBehaviourContinuous parent)
    {
        // Left blank on purpose
    }

    /// <summary>
    /// While spell ray is active, it will update muzzle position.
    /// Else it will stop the effect (for ex: when the player stops pressing attack key).
    /// </summary>
    /// <param name="parent"></param>
    public override void ContinuousUpdateBehaviour(SpellMuzzleBehaviourContinuous parent)
    {
        if (parent.SpellMonoBehaviour != null)
        {
            // If parent spell mono behaviour is false
            if (parent.SpellMonoBehaviour.gameObject.activeSelf == false)
            {
                if (parent.EffectNotNull)
                {
                    parent.EffectStop();

                    if (parent.EffectGetAliveParticles == 0)
                        parent.DisableMuzzleSpell();
                }
                else
                {
                    parent.DisableMuzzleSpell();
                }
            }
            else
            {
                if (parent.SpellMonoBehaviour.WhoCast != null)
                {
                    parent.transform.SetPositionAndRotation(
                       parent.SpellMonoBehaviour.Hand.position,
                       parent.SpellMonoBehaviour.Hand.rotation);
                }
                parent.TimeSpawned = Time.time;
            }
        }
    }
}
