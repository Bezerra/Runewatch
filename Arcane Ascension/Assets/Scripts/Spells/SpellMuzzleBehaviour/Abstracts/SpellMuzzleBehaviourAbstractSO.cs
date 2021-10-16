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
    public abstract void StartBehaviour(SpellMuzzleBehaviour parent);

    /// <summary>
    /// Executes on update.
    /// </summary>
    public abstract void ContinuousUpdateBehaviour(SpellMuzzleBehaviour parent);

    /// <summary>
    /// Disables spell Muzzle.
    /// </summary>
    /// <param name="parent"></param>
    public void DisableMuzzleSpell(SpellMuzzleBehaviour parent)
    {
        parent.gameObject.SetActive(false);
    }
}
