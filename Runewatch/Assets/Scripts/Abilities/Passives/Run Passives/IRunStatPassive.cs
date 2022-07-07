/// <summary>
/// Interface implemented by run passive abilities.
/// </summary>
public interface IRunStatPassive: IPassive<RunStatPassiveType>
{
    /// <summary>
    /// Executes a passive effect.
    /// </summary>
    /// <param name="playerStats">Player stats.</param>
    void Execute(PlayerStats playerStats);
}
