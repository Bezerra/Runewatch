/// <summary>
/// Interface implemented by burnable objects.
/// </summary>
public interface IInteractionWithSpell
{
    /// <summary>
    /// Method to start interaction.
    /// </summary>
    /// <param name="element">Element to check against.</param>
    void ExecuteInteraction(ElementType element);
}
