using UnityEngine;
using System;

/// <summary>
/// Struct with player saved data.
/// </summary>
[Serializable]
public struct PlayerSaveData
{
    // Public fields for JSON
    public float Health;
    public float Armor;
    public float Mana;
    public int DashCharge;

    public Vector3 Position;
    public Quaternion Rotation;

    public byte[] CurrentSpells;
    public byte CurrentSpellIndex;

    public byte[] CurrentPassives;
    public byte[] CurrentSkillTreePassives;

    public int Gold;
    public int ArcanePower;

}
