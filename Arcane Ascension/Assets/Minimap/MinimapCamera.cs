using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for controlling minimap camera rotation.
/// </summary>
public class MinimapCamera : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.eulerAngles = new Vector3(90, -90, 0);
    }
}
