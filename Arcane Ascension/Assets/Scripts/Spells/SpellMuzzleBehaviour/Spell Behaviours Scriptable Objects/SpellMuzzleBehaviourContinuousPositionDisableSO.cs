using UnityEngine;
using UnityEngine.VFX;

/// <summary>
/// Scriptable Object responsible for disabling the spell muzzle gameobject.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Muzzle Behaviour/Continuous/Spell Muzzle Behaviour Position And Disable", 
    fileName = "Spell Muzzle Behaviour Position And Disable")]
public class SpellMuzzleBehaviourContinuousPositionDisableSO : SpellMuzzleBehaviourAbstractContinuousSO
{
    [Range(0, 20)] [SerializeField] private byte disableAfterSeconds;

    public override void StartBehaviour(SpellMuzzleBehaviourContinuous parent)
    {
        //throw new System.NotImplementedException();
    }

    public override void ContinuousUpdateBehaviour(SpellMuzzleBehaviourContinuous parent)
    {
        if (parent.SpellMonoBehaviour != null)
        {
            // If parent spell mono behaviour is false
            if (parent.SpellMonoBehaviour.gameObject.activeSelf == false)
            {
                parent.MuzzleEffect.Stop();

                if (parent.GetComponentInChildren<VisualEffect>().aliveParticleCount == 0)
                    parent.DisableMuzzleSpell();
            }
            else
            {
                parent.transform.SetPositionAndRotation(
                    parent.SpellMonoBehaviour.Hand.position, 
                    parent.SpellMonoBehaviour.Hand.rotation);

                parent.TimeSpawned = Time.time;
            }
        }
    }
}
