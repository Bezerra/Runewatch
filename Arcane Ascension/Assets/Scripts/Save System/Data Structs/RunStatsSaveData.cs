using System;

/// <summary>
/// Struct responsible for keeping run stats save data.
/// </summary>
[Serializable]
public struct RunStatsSaveData 
{
    // Public fields for JSON
    public int EnemiesKilled;
    public int ArcanePowerObtained;
    public int GoldObtained;
    public int DamageDone;
    public int MostDamageDone;
    public int DamageTaken;
    public int RunTime;
    public int ShotsHit;
    public int ShotsFired;
    public int Accuracy;
}
