/// <summary>
/// Abstract scriptable object responsible for executing a spell hit behaviour.
/// </summary>
public abstract class SpellOnHitBehaviourAbstractOneShotSO : SpellOnHitBehaviourAbstractSO
{
    /// <summary>
    /// Executes when prefab is enabled.
    /// </summary>
    public abstract void StartBehaviour(SpellOnHitBehaviourOneShot parent);

    /// <summary>
    /// Executes on update.
    /// </summary>
    public abstract void ContinuousUpdateBehaviour(SpellOnHitBehaviourOneShot parent);
}