using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Class responsible for controlling tesselation depending on current quality level.
/// </summary>
public class TesselationControl : MonoBehaviour
{
    private IList<Material> tesselationMaterials;

    private Shader shader;

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

    private void Awake()
    {
        Object terrainShader = Resources.Load(terrainShaderPath);
        this.shader = terrainShader as Shader;

        if (shader != null)
        {
            tesselationMaterials = new List<Material>();

            Object[] allMaterials = Resources.LoadAll(texturesPath, typeof(Material));
            foreach (Object material in allMaterials)
            {
                Material convertedMaterial = material as Material;
                if (convertedMaterial != null)
                {
                    if (convertedMaterial.shader == shader)
                        tesselationMaterials.Add(convertedMaterial);
                }
            }
        }
        else
        {
            print("Shader is null.");
        }
    }

    private void Start()
    {
        Debug.Log(QualitySettings.GetQualityLevel());
        foreach (Material material in tesselationMaterials)
        {
            float MaxTesselationAmount = material.GetFloat("_MaxTesselationAmount");

            switch (QualitySettings.GetQualityLevel())
            {
                case 0:
                    material.SetFloat("_HeightStrength", heightStrengthLowQuality);
                    material.SetFloat("_TessValue", Mathf.Min(tessValueLowQuality, MaxTesselationAmount));
                    break;
                case 1:
                    material.SetFloat("_HeightStrength", heightStrengthMidQuality);
                    material.SetFloat("_TessValue", Mathf.Min(tessValueMidQuality, MaxTesselationAmount));
                    break;
                case 2:
                    material.SetFloat("_HeightStrength", heightStrengthHighQuality);
                    material.SetFloat("_TessValue", Mathf.Min(tessValueHighQuality, MaxTesselationAmount));
                    break;
            }
        }
    }
}
