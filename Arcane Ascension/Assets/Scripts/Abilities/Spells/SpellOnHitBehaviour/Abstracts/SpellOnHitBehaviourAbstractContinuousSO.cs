/// <summary>
/// Abstract scriptable object responsible for executing a spell hit behaviour.
/// </summary>
public abstract class SpellOnHitBehaviourAbstractContinuousSO : SpellOnHitBehaviourAbstractSO
{
    /// <summary>
    /// Executes when prefab is enabled.
    /// </summary>
    public abstract void StartBehaviour(SpellOnHitBehaviourContinuous parent);

    /// <summary>
    /// Executes on update.
    /// </summary>
    public abstract void ContinuousUpdateBehaviour(SpellOnHitBehaviourContinuous parent);
}
