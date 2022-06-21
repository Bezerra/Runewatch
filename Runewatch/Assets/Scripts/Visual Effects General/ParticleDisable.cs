using UnityEngine;

/// <summary>
/// Class responsible for keeping information about particles disable type.
/// </summary>
public class ParticleDisable : MonoBehaviour, IDisable
{
    [SerializeField] private DisableType disableType;

    public DisableType DisableType => disableType;
}
