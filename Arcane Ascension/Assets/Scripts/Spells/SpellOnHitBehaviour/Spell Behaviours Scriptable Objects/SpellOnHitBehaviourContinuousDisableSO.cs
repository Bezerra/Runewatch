using UnityEngine;

/// <summary>
/// Scriptable Object responsible for disabling the spell muzzle gameobject.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Hit Behaviour/Continuous/Spell On Hit Behaviour Disable", 
    fileName = "Spell On Hit Behaviour Disable")]
public class SpellOnHitBehaviourContinuousDisableSO : SpellOnHitBehaviourAbstractContinuousSO
{
    public override void StartBehaviour(SpellOnHitBehaviourContinuous parent)
    {
        //throw new System.NotImplementedException();
    }

    /// <summary>
    /// While spell ray is active, it will update muzzle position.
    /// Else it will stop the effect (for ex: when the player stops pressing attack key).
    /// </summary>
    /// <param name="parent"></param>
    public override void ContinuousUpdateBehaviour(SpellOnHitBehaviourContinuous parent)
    {
        //if (parent.SpellMonoBehaviour != null)
        //{
        //    // If parent spell mono behaviour is false
        //    if (parent.SpellMonoBehaviour.gameObject.activeSelf == false)
        //    {
        //        if (parent.MuzzleEffect != null)
        //        {
        //            parent.MuzzleEffect.Stop();
        //
        //            if (parent.MuzzleEffect.aliveParticleCount == 0)
        //                parent.DisableMuzzleSpell();
        //        }
        //        else
        //        {
        //            parent.DisableMuzzleSpell();
        //        }
        //    }
        //    else
        //    {
        //        parent.transform.SetPositionAndRotation(
        //            parent.SpellMonoBehaviour.Hand.position, 
        //            parent.SpellMonoBehaviour.Hand.rotation);
        //
        //        parent.TimeSpawned = Time.time;
        //    }
        //}
    }
}
