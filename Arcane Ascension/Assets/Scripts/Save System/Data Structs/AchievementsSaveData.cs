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
    public int GoldObtained;
    public int DamageDone;
    public int MostDamageDone;
    public int DamageTaken;
    public int[] RunTime;
}
