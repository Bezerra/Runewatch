/// <summary>
/// Interface implemented by IInterectables that use a canvas.
/// </summary>
public interface IInteractableWithCanvas
{
    /// <summary>
    /// Updates canvas information.
    /// </summary>
    /// <param name="text">Text to update.</param>
    void UpdateInformation(string text = null);
}
