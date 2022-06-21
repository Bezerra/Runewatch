using System.Collections;

/// <summary>
/// Class for player character.
/// </summary>
public class Player : Character
{
    /// <summary>
    /// Values of player.
    /// </summary>
    public PlayerValuesSO Values => allValues.CharacterValues as PlayerValuesSO;

    /// <summary>
    /// All values of player.
    /// </summary>
    public PlayerCharacterSO AllValues => allValues as PlayerCharacterSO;

    public RoomOcclusion InCurrentRoom { get; set; }

    private void OnDisable()
    {
        IInput player = FindObjectOfType<PlayerInputCustom>();
        if (player != null) player.SwitchActionMapToUI();
        InCurrentRoom = null;
    }
}
