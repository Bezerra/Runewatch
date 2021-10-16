using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable object with tesselation control values.
/// </summary>
[CreateAssetMenu(menuName = "Create Once/Tesselation Control", fileName = "Tesselation Control")]
public class TesselationControlSO : ScriptableObject
{
    [Header("Low Quality")]
    [Range(0f, 1f)] [SerializeField] private float heightStrengthLowQuality;
    [Range(1f, 32f)] [SerializeField] private float tessValueLowQuality;

    [Header("Medium Quality")]
    [Range(0f, 1f)] [SerializeField] private float heightStrengthMidQuality;
    [Range(1f, 32f)] [SerializeField] private float tessValueMidQuality;

    [Header("High Quality")]
    [Range(0f, 1f)] [SerializeField] private float heightStrengthHighQuality;
    [Range(1f, 32f)] [SerializeField] private float tessValueHighQuality;

    [SerializeField] private string terrainShaderPath;
    [SerializeField] private string texturesPath;

    public float HeightStrengthLowQuality   => heightStrengthLowQuality;
    public float TessValueLowQuality        => tessValueLowQuality;
    public float HeightStrengthMidQuality   => heightStrengthMidQuality;
    public float TessValueMidQuality        => tessValueMidQuality;
    public float HeightStrengthHighQuality  => heightStrengthHighQuality;
    public float TessValueHighQuality   => tessValueHighQuality;
    public string TerrainShaderPath  => terrainShaderPath;
    public string TexturesPath      => texturesPath;
}
