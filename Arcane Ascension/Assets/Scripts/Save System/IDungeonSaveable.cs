/// <summary>
/// Interface implemented by dungeons.
/// </summary>
public interface IDungeonSaveable : ISaveable
{
    /// <summary>
    /// Coroutine that defines which data is to be loaded.
    /// </summary>
    /// <param name="saveData">SaveData class.</param>
    /// <returns>Null.</returns>
    void LoadData(RunSaveData saveData);
}
