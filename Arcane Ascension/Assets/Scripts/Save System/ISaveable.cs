using System.Collections;

/// <summary>
/// Interface implemented by classes with saveable variables.
/// </summary>
public interface ISaveable
{
    /// <summary>
    /// Method that defines which data is to be saved.
    /// </summary>
    /// <param name="saveData">SaveData class.</param>
    void SaveCurrentData(SaveData saveData);

    /// <summary>
    /// Coroutine that defines which data is to be loaded.
    /// </summary>
    /// <param name="saveData">SaveData class.</param>
    /// <returns>Null.</returns>
    IEnumerator LoadData(SaveData saveData);
}
