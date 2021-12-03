/// <summary>
/// Interface with a method to find input. Executes a method every time
/// input is spawned. Must be applied by every class
/// that has input dependencies.
/// </summary>
public interface IFindInput
{
    /// <summary>
    /// Finds input and updates important variables.
    /// </summary>
    void FindInput();

    /// <summary>
    /// Used to unsubscribe stuff from input.
    /// </summary>
    void LostInput();
}
