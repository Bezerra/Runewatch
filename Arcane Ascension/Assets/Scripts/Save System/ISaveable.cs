
/// <summary>
/// Interface implemented by classes with saveable variables.
/// </summary>
public interface ISaveable
{
    /// <summary>
    /// Method that defines which data is to be saved.
    /// </summary>
    /// <param name="saveData">SaveData class.</param>
    void SaveCurrentData(RunSaveData saveData);
}
