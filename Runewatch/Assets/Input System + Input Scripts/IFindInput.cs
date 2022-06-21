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
    /// <param name="input">Input reference.</param>
    void FindInput(PlayerInputCustom input = null);

    /// <summary>
    /// Used to unsubscribe stuff from input.
    /// </summary>
    /// <param name="input">Input reference.</param> 
    void LostInput(PlayerInputCustom input = null);
}
