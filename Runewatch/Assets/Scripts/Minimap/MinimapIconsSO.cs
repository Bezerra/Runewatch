using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptable object responsible for containing mini map icons information.
/// </summary>
[InlineEditor]
[CreateAssetMenu(menuName = "Create Once/Minimap Icons", fileName = "Minimap Icons")]
public class MinimapIconsSO : ScriptableObject
{
    [SerializeField] private bool minimapPlayerDirection = true;
    [Range(40f, 80f)] [SerializeField] private float camSize = 53f;

    [SerializeField] private List<MinimapIconInformation> iconsInformation;

    public List<MinimapIconInformation> IconsInformation => iconsInformation;
    public float CamSize => camSize;
    public bool MinimapPlayerDirection => minimapPlayerDirection;
}
