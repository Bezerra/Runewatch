using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Selects a gameobject permanently, while the gameobject is active.
/// </summary>
public class SelectBackButtonPermanent : MonoBehaviour
{
    private void Update()
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }
}
