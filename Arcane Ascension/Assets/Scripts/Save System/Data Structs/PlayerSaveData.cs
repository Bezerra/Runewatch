using System;

/// <summary>
/// Struct with player saved data.
/// </summary>
[Serializable]
public struct PlayerSaveData
{
    // Public fields for JSON
    public float Health;
    public float Mana;

    public byte[] CurrentSpells;
    public byte CurrentSpellIndex;

    public byte[] CurrentPassives;

    public int Gold;
}
