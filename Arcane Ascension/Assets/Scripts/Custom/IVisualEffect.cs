/// <summary>
/// Interface implemented by classes that use both VFX and particle system.
/// </summary>
public interface IVisualEffect
{
    /// <summary>
    /// Plays effect.
    /// </summary>
    void EffectPlay();

    /// <summary>
    /// Stops effect.
    /// </summary>
    void EffectStop();

    /// <summary>
    /// Gets remaining alive particles of the effect.
    /// </summary>
    int EffectGetAliveParticles { get; }

    /// <summary>
    /// Checks if any of the effects exists.
    /// </summary>
    bool EffectNotNull { get; }
}
