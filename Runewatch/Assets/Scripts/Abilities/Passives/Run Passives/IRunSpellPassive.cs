/// <summary>
/// Interface implemented by run passive abilities.
/// </summary>
public interface IRunSpellPassive: IPassive<RunSpellPassiveType>
{
    /// <summary>
    /// Executes a passive effect.
    /// </summary>
    /// <param name="playerSpells">Player stats.</param>
    void Execute(PlayerSpells playerSpells);
}
