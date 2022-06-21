using System.Collections;

/// <summary>
/// Interface implemented by classes with saveable variables.
/// </summary>
public interface IBossRaidSaveable
{
    /// <summary>
    /// Method that defines which data is to be saved.
    /// </summary>
    /// <param name="saveData">SaveData class.</param>
    void SaveCurrentData(BossRaidRunSaveData saveData);

    /// <summary>
    /// Coroutine that defines which data is to be loaded.
    /// </summary>
    /// <param name="saveData">SaveData class.</param>
    /// <returns>Null.</returns>
    IEnumerator LoadData(BossRaidRunSaveData saveData);
}
