using UnityEngine;

/// <summary>
/// Class responsible for controlling minimap camera position and rotation.
/// </summary>
public class MinimapCamera : MonoBehaviour, IFindPlayer
{
    // Minimap information
    [SerializeField] private MinimapIconsSO allIcons;

    // Components
    private Player player;
    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        player = FindObjectOfType<Player>();

        cam.orthographicSize = allIcons.CamSize;
    }

    private void FixedUpdate()
    {
        if (player != null)
        {
            Vector3 newPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            Quaternion newRotation;

            if (allIcons.MinimapPlayerDirection)
            {
                newRotation = Quaternion.Euler(90, -90, -90 + -player.transform.eulerAngles.y);
            }
            else
            {
                newRotation = Quaternion.Euler(90, -90, 0);
            }

            transform.SetPositionAndRotation(newPosition, newRotation);
        }
    }

    public void FindPlayer()
    {
        player = FindObjectOfType<Player>();
    }

    public void PlayerLost()
    {
        // Left blank on purpose
    }
}
