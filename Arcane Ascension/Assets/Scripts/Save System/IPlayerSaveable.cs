using System.Collections;

/// <summary>
/// Interface implemented by player character.
/// </summary>
public interface IPlayerSaveable : ISaveable
{
    /// <summary>
    /// Coroutine that defines which data is to be loaded.
    /// </summary>
    /// <param name="saveData">SaveData class.</param>
    /// <returns>Null.</returns>
    IEnumerator LoadData(RunSaveData saveData);
}
