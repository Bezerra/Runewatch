using System;
using UnityEngine;

/// <summary>
/// Struct responsible for containing a minimap icon information.
/// Contains the icon type and its respective texture.
/// </summary>
[Serializable]
public struct MinimapIconInformation
{
    [SerializeField] private MinimapIconType minimapIconType;
    [SerializeField] private Sprite minimapIconTexture;
    [Range(0f,20f)] [SerializeField] private float scale;
    [Range(0, 10)] [SerializeField] private int canvasOrder;

    public MinimapIconType MinimapIconType => minimapIconType;
    public Sprite MinimapIconTexture => minimapIconTexture;
    public float Scale => scale;
    public int CanvasOrder => canvasOrder;
}
