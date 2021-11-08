/// <summary>
/// Interface implemented by gameobjects that are ability interectables.
/// </summary>
public interface IAbilityInterectable
{
    /// <summary>
    /// Sound this interectable plays when interected with.
    /// </summary>
    public LootAndInteractionSoundType LootAndInteractionSoundType { get; }
}
