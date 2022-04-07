using System;

/// <summary>
/// Struct responsible for keeping achievments save data.
/// </summary>
[Serializable]
public struct AchievementsSaveData 
{
    // Public fields for JSON
    public int EnemiesKilled;
    public int ArcanePowerObtained;
    public float DamageDone;
    public float DamageTaken;
    public int[] RunTime;
}
