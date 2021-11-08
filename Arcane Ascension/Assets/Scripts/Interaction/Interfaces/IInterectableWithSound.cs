/// <summary>
/// Interface implemented by gameobjects that are interectables and play sounds.
/// </summary>
public interface IInterectableWithSound
{
    /// <summary>
    /// Sound this interectable plays when interected with.
    /// </summary>
    public LootAndInteractionSoundType LootAndInteractionSoundType { get; }
}
