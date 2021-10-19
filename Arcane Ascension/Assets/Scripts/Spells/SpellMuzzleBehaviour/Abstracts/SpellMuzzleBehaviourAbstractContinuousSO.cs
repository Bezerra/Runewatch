/// <summary>
/// Abstract scriptable object responsible for executing a one shot spell muzzle behaviour.
/// </summary>
public abstract class SpellMuzzleBehaviourAbstractContinuousSO : SpellMuzzleBehaviourAbstractSO
{
    /// <summary>
    /// Executes when prefab is enabled.
    /// </summary>
    public override abstract void StartBehaviour(SpellMuzzleBehaviourContinuous parent);

    /// <summary>
    /// Executes on update.
    /// </summary>
    public override abstract void ContinuousUpdateBehaviour(SpellMuzzleBehaviourContinuous parent);
}
