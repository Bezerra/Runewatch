/// <summary>
/// Interface implemented by things that block passages, like a door.
/// </summary>
public interface IPassageBlock
{
    /// <summary>
    /// Property to know if the passage can open.
    /// </summary>
    bool CanOpen { get; set; }

    /// <summary>
    /// Property to know if the piece behind the door was loaded.
    /// </summary>
    bool IsDoorRoomFullyLoaded { get; set; }

    /// <summary>
    /// Opens passage.
    /// </summary>
    void Open();

    /// <summary>
    /// Enables block symbol.
    /// </summary>
    void EnableBlockSymbol();

    /// <summary>
    /// Disables block symbol.
    /// </summary>
    void DisableBlockSymbol();

    /// <summary>
    /// Closes passage.
    /// </summary>
    void Close();
}
