/// <summary>
/// Interface implemented by skill tree passives.
/// </summary>
public interface ISkillTreePassive : IPassive<SkillTreePassiveType>
{
    /// <summary>
    /// Cost of the ability.
    /// </summary>
    int Cost { get; }

    /// <summary>
    /// Amount of the effect.
    /// </summary>
    byte Amount { get; }
}
