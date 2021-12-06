using UnityEngine;

/// <summary>
/// Class resposnsible for activating a minimap icon.
/// </summary>
public class MinimapActivation : MonoBehaviour
{
    [SerializeField] private MinimapIcon minimapIcon;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Layers.PlayerLayerNum ||
            other.gameObject.layer == Layers.InvisiblePlayerLayerNum)
        {
            minimapIcon.SetIconActive(true);
            Destroy(gameObject);
        }
    }
}
