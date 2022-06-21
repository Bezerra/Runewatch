using UnityEngine;

/// <summary>
/// Scriptable object responsible for containing information and logic about player control,
/// like spawns, prefabs, etc.
/// </summary>
[CreateAssetMenu(menuName = "Create Once/Player Controller", fileName = "Player Controller")]
public class PlayerControllerSO : ScriptableObject
{
    [SerializeField] private GameObject playerPrefab;

    public void Instantiate(Vector3 position, Quaternion rotation)
    {
        Instantiate(playerPrefab, position, rotation);
    }
}
