using System;
using System.IO;
using UnityEngine;

/// <summary>
/// Writes and reades files.
/// </summary>
public struct FileManager
{
    /// <summary>
    /// Writes to a file.
    /// </summary>
    /// <param name="fileName">File name,</param>
    /// <param name="fileContent">Content of the file.</param>
    /// <returns>True if succeed.</returns>
    public bool WriteToFile(string fileName, string fileContent)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);

        try
        {
            File.WriteAllText(path, fileContent);
            return true;
        }
        catch (Exception excep)
        {
            Debug.LogError($"Failed to write to {path} with exception {excep}");
        }
        return false;
    }

    /// <summary>
    /// Reads a file.
    /// </summary>
    /// <param name="fileName">File name,</param>
    /// <param name="result">Content of the file.</param>
    /// <returns>True if succeed.</returns>
    public bool ReadFile(string fileName, out string result)
    {
        if (File.Exists(Path.Combine(Application.persistentDataPath, fileName)))
        {
            string path = Path.Combine(Application.persistentDataPath, fileName);

            try
            {
                result = File.ReadAllText(path);
                return true;
            }
            catch (Exception excep)
            {
                Debug.LogError($"Failed to read from {path} with exception {excep}");
                result = "";
                return false;
            }
        }
        result = "";
        return false;
    }
}
