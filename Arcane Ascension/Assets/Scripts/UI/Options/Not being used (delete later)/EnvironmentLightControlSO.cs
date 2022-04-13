using UnityEngine;
using UnityEngine.Rendering;
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptable object responsible for updating element scene lights.
/// </summary>
[CreateAssetMenu(menuName = "Create Once/Element Scene Lights", fileName = "Element Scene Lights")]
[InlineEditor]
public class EnvironmentLightControlSO : ScriptableObject
{
    [SerializeField] private List<ElementsLightColor> elementLightColors;

    /// <summary>
    /// Updates current color.
    /// </summary>
    public void UpdateColor(ElementType elementType)
    {
        RenderSettings.ambientMode = AmbientMode.Trilight;

        if (elementLightColors.Count > 0)
        {
            IDictionary<ElementType, Color[]> elementColors = new Dictionary<ElementType, Color[]>();
            foreach (ElementsLightColor elementLightColor in elementLightColors)
            {
                elementColors.Add(
                    elementLightColor.Element, 
                    new Color[3]
                        {
                            elementLightColor.SkyColor,
                            elementLightColor.EquatorColor,
                            elementLightColor.GroundColor
                        });

            }

            if (elementColors.ContainsKey(elementType))
            {
                RenderSettings.ambientSkyColor = elementColors[elementType][0];
                RenderSettings.ambientEquatorColor = elementColors[elementType][1];
                RenderSettings.ambientGroundColor = elementColors[elementType][2];
            }
        }
    }

    [Serializable]
    private struct ElementsLightColor
    {
        [ColorUsage(true, true)]
        [SerializeField] private Color skyColor;
        [SerializeField] private Color equatorColor;
        [SerializeField] private Color groundColor;
        [SerializeField] private ElementType element;

        public Color SkyColor => skyColor;
        public Color EquatorColor => equatorColor;
        public Color GroundColor => groundColor;
        public ElementType Element => element;
    }
}
