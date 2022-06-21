using UnityEngine.UI;

/// <summary>
/// Struct for an image of a status effect.
/// </summary>
public struct StatusEffectImage
{
    public Image Image { get; }
    public StatusEffectType Type { get; }
    public float Duration { get; }
    public StatusEffectImage(Image image, StatusEffectType type, float duration)
    {
        Image = image;
        Type = type;
        Duration = duration;
    }
}
