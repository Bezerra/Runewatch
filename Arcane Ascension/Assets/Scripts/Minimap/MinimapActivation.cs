using UnityEngine;

/// <summary>
/// Class resposnsible for activating a minimap icon.
/// </summary>
public class MinimapActivation : MonoBehaviour
{
    [SerializeField] private GameObject minimapIcon;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Layers.PlayerLayerNum ||
            other.gameObject.layer == Layers.InvisiblePlayerLayerNum)
        {
            minimapIcon.SetActive(true);
            Destroy(gameObject);
        }
    }
}
