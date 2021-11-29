using UnityEngine;

/// <summary>
/// Scriptable Object responsible for updating muzzle position and rotation..
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Muzzle Behaviour/One Shot/Spell Muzzle Behaviour Position Rotation Update", 
    fileName = "Spell Muzzle Behaviour Position Rotation Update")]
public class SpellMuzzleBehaviourPositionRotationUpdateSO : SpellMuzzleBehaviourAbstractOneShotSO
{
    /// <summary>
    /// Executed when the spell is enabled.
    /// </summary>
    /// <param name="parent">Parent Spell Muzzle monobehaviour.</param>
    public override void StartBehaviour(SpellMuzzleBehaviourOneShot parent)
    {
        // Left blank on purpose
    }

    /// <summary>
    /// Executed every update.
    /// </summary>
    /// <param name="parent">Parent Spell Muzzle monobehaviour.</param>
    public override void ContinuousUpdateBehaviour(SpellMuzzleBehaviourOneShot parent)
    {
        // Left blank on purpose
    }

    public override void ContinuousFixedUpdateBehaviour(SpellMuzzleBehaviourOneShot parent)
    {
        if (parent.Eyes != null)
        {
            if (parent.WhoCast.CommonAttributes.Type == CharacterType.Player)
            {
                parent.transform.position = parent.Eyes.position +
                    (parent.Eyes.right * parent.MuzzlePlayerSpawnOffset.x) +
                    (parent.Eyes.up * parent.MuzzlePlayerSpawnOffset.y) +
                    (parent.Eyes.forward * parent.MuzzlePlayerSpawnOffset.z);

                parent.transform.rotation = Quaternion.LookRotation(parent.Eyes.forward);
            }
            else
            {
                parent.transform.position = parent.Hand.position;
                parent.transform.rotation = Quaternion.LookRotation(parent.Eyes.forward);
            }
        }
    }
}
