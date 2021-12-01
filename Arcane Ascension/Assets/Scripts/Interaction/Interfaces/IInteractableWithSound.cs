/// <summary>
/// Interface implemented by gameobjects that are interectables and play sounds
/// in the exact moment of interaction.
/// </summary>
public interface IInteractableWithSound
{
    /// <summary>
    /// Sound this interectable plays when interected with.
    /// </summary>
    public LootAndInteractionSoundType LootAndInteractionSoundType { get; }
}
