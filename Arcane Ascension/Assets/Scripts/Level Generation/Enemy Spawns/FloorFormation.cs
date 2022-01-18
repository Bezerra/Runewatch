using UnityEngine;

/// <summary>
/// Class responsible for containing floor formation information.
/// </summary>
public class FloorFormation : MonoBehaviour
{
    [SerializeField] private FloorFormationType floorFormation;
    public FloorFormationType FloorFormationType => floorFormation;
}
