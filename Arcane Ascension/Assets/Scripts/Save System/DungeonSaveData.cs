using System;

/// <summary>
/// Struct with dungeon saved data.
/// </summary>
[Serializable]
public struct DungeonSaveData
{
    // Public fields for JSON
    public int Seed;
    public int HorizontalMaximumLevelSize;
    public int ForwardMaximumLevelSize;
    public int MinimumNumberOfRooms;
    public int MaximumNumberOfRooms;
    public ElementType Element;
}
