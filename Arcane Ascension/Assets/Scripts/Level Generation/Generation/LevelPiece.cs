using UnityEngine;

/// <summary>
/// Class used by every level piece.
/// </summary>
public class LevelPiece : MonoBehaviour
{
    public string Name => concreteType.ToString();

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

    [SerializeField] private RoomWeightsSO roomWeights;
    public int RoomWeight
    {
        get
        {
            foreach (Weight weight in roomWeights.PiecesWithWeight)
            {
                if (weight.Name == Name)
                {
                    return weight.RoomWeight;
                }
            
            }
            return 0;
        }
    }
}
