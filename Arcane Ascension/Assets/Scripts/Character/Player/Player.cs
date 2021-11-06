using System.Collections;
using UnityEngine;

/// <summary>
/// Class for player character.
/// </summary>
public class Player : Character, ISaveable
{
    /// <summary>
    /// Values of player.
    /// </summary>
    public PlayerValuesSO Values => allValues.CharacterValues as PlayerValuesSO;

    /// <summary>
    /// All values of player.
    /// </summary>
    public PlayerCharacterSO AllValues => allValues as PlayerCharacterSO;

    /// <summary>
    /// Saves player position and rotation.
    /// </summary>
    /// <param name="saveData">Saved data class.</param>
    /// <returns>Null.</returns>
    public void SaveCurrentData(SaveData saveData)
    {
        if (this != null) // Do not remove <
        {
            saveData.PlayerSavedData.Position = transform.position;
            saveData.PlayerSavedData.Rotation = transform.rotation;
        }
    }

    /// <summary>
    /// Null.
    /// </summary>
    /// <param name="saveData">Saved data class.</param>
    /// <returns>Null.</returns>
    public IEnumerator LoadData(SaveData saveData)
    {
        yield return null;
    }
}
