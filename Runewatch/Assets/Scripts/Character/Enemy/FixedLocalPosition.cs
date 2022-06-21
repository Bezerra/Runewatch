using UnityEngine;

/// <summary>
/// Fixes local position.
/// </summary>
public class FixedLocalPosition : MonoBehaviour
{
    private Vector3 pos;

    private void Awake()
    {
        pos = new Vector3(0, -2, 0);
    }

    private void FixedUpdate()
    {
        transform.localPosition = pos;
    }
}
