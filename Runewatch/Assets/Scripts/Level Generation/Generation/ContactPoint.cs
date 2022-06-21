using System.Collections.Generic;
using UnityEngine;

public class ContactPoint : MonoBehaviour
{
    [SerializeField] private PieceConcreteType[] incompatiblePieceConcreteTypes;
    public HashSet<PieceConcreteType> IncompatiblePieces { get; set; }

    /// <summary>
    /// Parent room of this contact point.
    /// </summary>
    public LevelPiece ParentRoom { get; private set; }

    /// <summary>
    /// Room this contact point created.
    /// </summary>
    public LevelPiece OriginatedRoom { get; set; }

    private PointState state;
    private enum PointState { Opened, Closed, }

    /// <summary>
    /// Sets contact point gizmos to green.
    /// </summary>
    public void Open() => state = PointState.Opened;

    /// <summary>
    /// Sets contact point gizmos to red.
    /// </summary>
    public void Close() => state = PointState.Closed;

    private void Awake()
    {
        ParentRoom = GetComponentInParent<LevelPiece>();
        IncompatiblePieces = new HashSet<PieceConcreteType>();

        if (incompatiblePieceConcreteTypes.Length > 0)
        {
            foreach (PieceConcreteType piece in incompatiblePieceConcreteTypes)
                IncompatiblePieces.Add(piece);
        }
    }

    private void OnDrawGizmos()
    {
        if (state == PointState.Opened)
            Gizmos.color = Color.green;

        else if (state == PointState.Closed)
            Gizmos.color = Color.red;

        Gizmos.DrawSphere(transform.position, 0.1f);
    }
}
