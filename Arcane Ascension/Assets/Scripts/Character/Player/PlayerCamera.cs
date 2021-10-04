using UnityEngine;

/// <summary>
/// Class responsible for handling camera movement and rotation.
/// </summary>
public class PlayerCamera : MonoBehaviour
{
    // Components
    private Player player;
    private PlayerInputCustom input;
    [SerializeField] private Transform finalDirection;
    public Transform FinalDirection => finalDirection;

    // Camera variables
    private float cameraX;
    private float zRotation;
    private float currentZRotation;

    private void Awake()
    {
        player = GetComponent<Player>();
        input = FindObjectOfType<PlayerInputCustom>();
        zRotation = 0;
        cameraX = 0;
    }

    private void FixedUpdate()
    {
        CameraControl();
        CameraTilt();
    }

    private void CameraControl()
    {
        // Rotates camera on X axis (vertical)
        cameraX -= input.Camera.y * player.Values.CameraSpeed * Time.fixedDeltaTime;
        cameraX = Mathf.Clamp(cameraX, player.Values.CameraRange.x, player.Values.CameraRange.y);
            finalDirection.transform.eulerAngles =
            new Vector3(cameraX, finalDirection.transform.eulerAngles.y, finalDirection.transform.eulerAngles.z);

        // Rotates the player on Y axis (horizontal)
        transform.Rotate(Vector3.up * input.Camera.x * player.Values.CameraSpeed * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Tilts the camera while strafing.
    /// </summary>
    private void CameraTilt()
    {
        if (input.Movement.x > 0)
        {
            zRotation = Mathf.SmoothDamp(
                zRotation, -player.Values.CameraTilt, ref currentZRotation, Time.fixedDeltaTime * player.Values.CameraTiltSpeed);
        }
        else if (input.Movement.x < 0)
        {
            zRotation = Mathf.SmoothDamp(
                zRotation, player.Values.CameraTilt, ref currentZRotation, Time.fixedDeltaTime * player.Values.CameraTiltSpeed);
        }
        else
        {
            zRotation = Mathf.SmoothDamp(
                zRotation, 0, ref currentZRotation, Time.fixedDeltaTime * player.Values.CameraTiltSpeed);
        }

        finalDirection.transform.eulerAngles = 
            new Vector3(finalDirection.transform.eulerAngles.x, finalDirection.transform.eulerAngles.y, zRotation);
    }
}
