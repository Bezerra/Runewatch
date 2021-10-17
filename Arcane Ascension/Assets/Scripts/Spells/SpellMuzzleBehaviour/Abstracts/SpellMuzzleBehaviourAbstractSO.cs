using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Abstract scriptable object responsible for executing a spell muzzle behaviour.
/// </summary>
[InlineEditor]
public abstract class SpellMuzzleBehaviourAbstractSO : ScriptableObject
{
    /// <summary>
    /// Executes when prefab is enabled.
    /// </summary>
    public abstract void StartBehaviour(SpellMuzzleBehaviourOneShot parent);

    /// <summary>
    /// Executes on update.
    /// </summary>
    public abstract void ContinuousUpdateBehaviour(SpellMuzzleBehaviourOneShot parent);

    /// <summary>
    /// Disables spell Muzzle.
    /// </summary>
    /// <param name="parent"></param>
    public void DisableMuzzleSpell(SpellMuzzleBehaviourOneShot parent)
    {
        parent.gameObject.SetActive(false);
    }
}
