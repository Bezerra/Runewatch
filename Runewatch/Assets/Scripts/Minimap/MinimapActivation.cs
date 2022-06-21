using UnityEngine;

/// <summary>
/// Class resposnsible for activating a minimap icon.
/// </summary>
public class MinimapActivation : MonoBehaviour
{
    [SerializeField] private MinimapIcon minimapIcon;

    private LevelPiece parentLevelPiece;

    private void Awake() =>
        parentLevelPiece = GetComponentInParent<LevelPiece>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Layers.PlayerLayerNum ||
            other.gameObject.layer == Layers.InvisiblePlayerLayerNum)
        {
            // Activates level piece on minimap
            minimapIcon.SetIconActive(true);

            // Activates level piece's doors
            foreach (Door contactPoint in parentLevelPiece.GetComponentsInChildren<Door>())
            {
                contactPoint.GetComponentInChildren<MinimapIcon>(true).SetIconActive(true);
            }

            Destroy(gameObject);
        }
    }
}
