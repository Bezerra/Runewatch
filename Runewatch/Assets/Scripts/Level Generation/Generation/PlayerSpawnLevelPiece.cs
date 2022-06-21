using UnityEngine;

/// <summary>
/// Piece with player spawn transform information.
/// </summary>
public class PlayerSpawnLevelPiece : MonoBehaviour
{
    [SerializeField] private Transform playerSpawnTransform;
    public Transform PlayerSpawnTransform => playerSpawnTransform;
}
