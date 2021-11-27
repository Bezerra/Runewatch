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
        RenderSettings.ambientMode = AmbientMode.Flat;

        if (elementLightColors.Count > 0)
        {
            IDictionary<ElementType, Color> elementColors = new Dictionary<ElementType, Color>();
            foreach (ElementsLightColor elementLightColor in elementLightColors)
                elementColors.Add(elementLightColor.Element, elementLightColor.Color);

            if (elementColors.ContainsKey(elementType))
                RenderSettings.ambientLight = elementColors[elementType];
        }
    }

    [Serializable]
    private struct ElementsLightColor
    {
        [SerializeField] private Color color;
        [SerializeField] private ElementType element;

        public Color Color => color;
        public ElementType Element => element;
    }
}
