public interface IHealable
{
    /// <summary>
    /// Amount of heal.
    /// </summary>
    /// <param name="amountOfHeal">Amount of heal.</param>
    /// <param name="typeOfHeal">Heal health or armor.</param>
    void Heal(float amountOfHeal, StatsType typeOfHeal);
}
