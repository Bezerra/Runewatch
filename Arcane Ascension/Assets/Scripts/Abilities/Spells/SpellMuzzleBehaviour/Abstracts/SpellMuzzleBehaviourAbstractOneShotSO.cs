/// <summary>
/// Abstract scriptable object responsible for executing a one shot spell muzzle behaviour.
/// </summary>
public abstract class SpellMuzzleBehaviourAbstractOneShotSO : SpellMuzzleBehaviourAbstractSO
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
    /// Executes on fixed update.
    /// </summary>
    public abstract void ContinuousFixedUpdateBehaviour(SpellMuzzleBehaviourOneShot parent);
}
