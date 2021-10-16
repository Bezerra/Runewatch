using UnityEngine;

/// <summary>
/// Class used by every level piece.
/// </summary>
public class LevelPiece : MonoBehaviour
{
    [SerializeField] private PieceType type;
    public PieceType Type => type;

    [SerializeField] private PieceConcreteType concreteType;
    public PieceConcreteType ConcreteType => concreteType;

    [SerializeField] private ContactPoint[] contactPoints;
    public ContactPoint[] ContactPoints => contactPoints;

    public ContactPoint ConnectedContactPoint { get; set; }

    [SerializeField] private GameObject boxCollidersParent;
    public GameObject BoxCollidersParent => boxCollidersParent;

    [SerializeField] private BoxCollider[] boxColliders;
    public BoxCollider[] BoxColliders => boxColliders;

    [SerializeField] private int roomWeight;
    public int RoomWeight => roomWeight;
}
