using UnityEngine;

/// <summary>
/// Disables this gameobject when the object is disabled.
/// </summary>
public class DisableGameObjectOnDisable : MonoBehaviour
{
    private void OnDisable()
    {
        gameObject.SetActive(false);
    }
}
