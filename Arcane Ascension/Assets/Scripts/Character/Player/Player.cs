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
    /// All values of enemy.
    /// </summary>
    public PlayerCharacterSO AllValues => allValues as PlayerCharacterSO;

    /// <summary>
    /// Saves player stats.
    /// </summary>
    /// <param name="saveData">Saved data class.</param>
    /// <returns>Null.</returns>
    public void SaveCurrentData(SaveData saveData)
    {
        // Stats
        if (this != null) // Do not remove <
        {
            PlayerStats playerStats = GetComponent<PlayerStats>();
            saveData.PlayerSavedData.Health = playerStats.Health;
            saveData.PlayerSavedData.Armor = playerStats.Armor;
            saveData.PlayerSavedData.Mana = playerStats.Mana;
            saveData.PlayerSavedData.Position = transform.position;
            saveData.PlayerSavedData.Rotation = transform.rotation;
        }
    }

    /// <summary>
    /// Loads player stats.
    /// </summary>
    /// <param name="saveData">Saved data class.</param>
    /// <returns>Null.</returns>
    public IEnumerator LoadData(SaveData saveData)
    {
        yield return new WaitForFixedUpdate();

        // Stats
        if (this != null) // Do not remove <
        {
            PlayerStats playerStats = GetComponent<PlayerStats>();

            // Loads stats
            playerStats.SetStats(saveData.PlayerSavedData.Health, saveData.PlayerSavedData.Armor, saveData.PlayerSavedData.Mana);
        }
    }
}
