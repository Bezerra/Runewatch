/// <summary>
/// Abstract scriptable object responsible for executing a spell hit behaviour.
/// </summary>
public abstract class SpellOnHitBehaviourAbstractContinuousSO : SpellOnHitBehaviourAbstractSO
{
    /// <summary>
    /// Executes when prefab is enabled.
    /// </summary>
    public override abstract void StartBehaviour(SpellOnHitBehaviourOneShot parent);

    /// <summary>
    /// Executes on update.
    /// </summary>
    public override abstract void ContinuousUpdateBehaviour(SpellOnHitBehaviourOneShot parent);
}
