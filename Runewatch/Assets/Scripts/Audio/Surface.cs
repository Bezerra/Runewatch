using UnityEngine;

/// <summary>
/// Surface of this gameobject.
/// </summary>
public class Surface : MonoBehaviour, ISurface
{
    [SerializeField] private SurfaceType surfaceType;

    public SurfaceType SurfaceType => surfaceType;
}
