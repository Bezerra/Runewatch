using System.Collections.Generic;
using UnityEngine;

public class ContactPoint : MonoBehaviour
{
    public LevelPiece ParentRoom { get; private set; }
    private PointState state;

    public ICollection<PieceConcreteType> IncompatiblePieces { get; set; }

    private void Awake()
    {
        ParentRoom = GetComponentInParent<LevelPiece>();
        IncompatiblePieces = new List<PieceConcreteType>();
    }

    /// <summary>
    /// Sets contact point gizmos to green.
    /// </summary>
    public void Open() => state = PointState.Opened;

    /// <summary>
    /// Sets contact point gizmos to red.
    /// </summary>
    public void Close() => state = PointState.Closed;

    private enum PointState { Opened, Closed, }

    private void OnDrawGizmos()
    {
        if (state == PointState.Opened)
            Gizmos.color = Color.green;

        else if (state == PointState.Closed)
            Gizmos.color = Color.red;

        Gizmos.DrawSphere(transform.position, 0.1f);
    }
}
