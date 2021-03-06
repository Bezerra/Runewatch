using UnityEngine;

/// <summary>
/// Class responsible for controlling a child passage.
/// </summary>
public class ContactPointDoor : MonoBehaviour
{
    private IPassageBlock passageBlock;

    private void Awake() =>
        passageBlock = GetComponentInChildren<IPassageBlock>();

    private void Start() =>
        ClosePassage();

    /// <summary>
    /// Opens the passage.
    /// </summary>
    public void OpenPassage() =>
        passageBlock.Open();

    /// <summary>
    /// Closes a passage.
    /// </summary>
    public void ClosePassage() =>
        passageBlock.Close();

    /// <summary>
    /// Blocks a passage.
    /// </summary>
    public void BlockPassage()
    {
        passageBlock?.EnableBlockSymbol();
        passageBlock.CanOpen = false;
    }

    /// <summary>
    /// Unblocks a passage.
    /// </summary>
    public void UnblockPassage()
    {
        passageBlock?.DisableBlockSymbol();
        passageBlock.CanOpen = true;
    }

    /// <summary>
    /// Sets passage block room as not loaded.
    /// </summary>
    public void PieceNotLoaded() =>
        passageBlock.IsDoorRoomFullyLoaded = false;

    /// <summary>
    /// Sets passage  block room as loaded.
    /// </summary>
    public void PieceLoaded() =>
        passageBlock.IsDoorRoomFullyLoaded = true;
}
