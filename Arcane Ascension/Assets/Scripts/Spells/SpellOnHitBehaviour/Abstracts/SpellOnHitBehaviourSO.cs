/// <summary>
/// Abstract class responsible for executing a spell hit behaviour.
/// </summary>
public abstract class SpellOnHitBehaviourSO : SpellBehaviourAbstractSO
{
    /// <summary>
    /// Executes when prefab is enabled.
    /// </summary>
    public abstract void StartBehaviour(SpellOnHitBehaviour parent);

    /// <summary>
    /// Executes on update.
    /// </summary>
    public abstract void ContinuousUpdateBehaviour(SpellOnHitBehaviour parent);
}
