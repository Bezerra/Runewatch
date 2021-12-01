/// <summary>
/// Interface implemented by things that block passages, like a door.
/// </summary>
public interface IPassageBlock
{
    /// <summary>
    /// Opens passage.
    /// </summary>
    void Open();

    /// <summary>
    /// Closes passage.
    /// </summary>
    void Close();
}
