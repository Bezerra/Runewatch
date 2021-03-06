using UnityEngine;
using System;

/// <summary>
/// Script responsible for checking if this gameobject collided with another on RoomCollisionLayer.
/// </summary>
public class PieceIntersectionCheck : MonoBehaviour
{
    private bool intersecting;

    public bool CanCheckCollision { private get; set; }

    private void Start() =>
        intersecting = false;

    private void OnTriggerStay(Collider other)
    {
        if (CanCheckCollision)
        {
            if (other.gameObject.layer == Layers.RoomCollisionLayerNum)
            {
                intersecting = true;
            }
            OnCollisionEvent(intersecting);
        }
    }

    protected virtual void OnCollisionEvent(bool didItCollide) => CollisionEvent?.Invoke(didItCollide);
    public event Action<bool> CollisionEvent;
}
