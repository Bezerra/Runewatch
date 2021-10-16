using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for controlling tesselation depending on current quality level.
/// </summary>
public class TesselationControl : MonoBehaviour
{
    [SerializeField] private TesselationControlSO values;
    private IList<Material> tesselationMaterials;
    private Shader shader;

    private void Awake()
    {
        Object terrainShader = Resources.Load(values.TerrainShaderPath);
        this.shader = terrainShader as Shader;

        if (shader != null)
        {
            tesselationMaterials = new List<Material>();

            Object[] allMaterials = Resources.LoadAll(values.TexturesPath, typeof(Material));
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
        UpdateTesselation();
    }

    private void UpdateTesselation()
    {
        foreach (Material material in tesselationMaterials)
        {
            float MaxTesselationAmount = material.GetFloat("_MaxTesselationAmount");

            switch (QualitySettings.GetQualityLevel())
            {
                case 0:
                    material.SetFloat("_HeightStrength", values.HeightStrengthLowQuality);
                    material.SetFloat("_TessValue", Mathf.Min(values.TessValueLowQuality, MaxTesselationAmount));
                    break;
                case 1:
                    material.SetFloat("_HeightStrength", values.HeightStrengthMidQuality);
                    material.SetFloat("_TessValue", Mathf.Min(values.TessValueMidQuality, MaxTesselationAmount));
                    break;
                case 2:
                    material.SetFloat("_HeightStrength", values.HeightStrengthHighQuality);
                    material.SetFloat("_TessValue", Mathf.Min(values.TessValueHighQuality, MaxTesselationAmount));
                    break;
            }
        }
    }
}