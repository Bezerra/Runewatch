using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Class responsible for updating element scene lights.
/// </summary>
public class EnvironmentLightControl : MonoBehaviour
{
    [SerializeField] private EnvironmentLightControlSO environmentLightsControlSO;

    [OnValueChanged("UpdateColor")]
    [SerializeField] private ElementType element;

    private void Awake()
    {
        UpdateColor();
    }

    private void UpdateColor() =>
        environmentLightsControlSO.UpdateColor(element);
}
