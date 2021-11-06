/// <summary>
/// Interface implemented by characters that can be healed.
/// </summary>
public interface IHealable
{
    /// <summary>
    /// Amount of heal.
    /// </summary>
    /// <param name="amountOfHeal">Amount of heal.</param>
    /// <param name="typeOfHeal">Stats to heal.</param>
    void Heal(float amountOfHeal, StatsType typeOfHeal);
}
