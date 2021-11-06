using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for controlling minimap camera rotation.
/// </summary>
public class MinimapCamera : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    private void FixedUpdate()
    {
        Vector3 newPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.position = newPosition;
    }
}
