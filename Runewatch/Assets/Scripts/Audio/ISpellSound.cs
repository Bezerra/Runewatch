using System.Collections.Generic;

/// <summary>
/// Interface for spell sounds.
/// </summary>
public interface ISpellSound
{
    /// <summary>
    /// Sounds for spell events.
    /// </summary>
    SpellSound Sounds { get; }

    /// <summary>
    /// Dictionary with a sound for each surface type.
    /// </summary>
    IDictionary<SurfaceType, AbstractSoundSO> SurfaceSounds { get; }
}
