/// <summary>
/// Interface implemented by characters with Mana stats.
/// </summary>
public interface IMana
{
    float Mana { get; }
    void ReduceMana(float amount);
}
