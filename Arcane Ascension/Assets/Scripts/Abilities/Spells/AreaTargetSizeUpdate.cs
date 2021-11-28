using UnityEngine;

/// <summary>
/// Class responsible for updating area target size.
/// </summary>
public class AreaTargetSizeUpdate : MonoBehaviour
{
    [SerializeField] private ParticleSystem circle;
    [SerializeField] private ParticleSystem cilinder;

    ParticleSystem.MainModule circleMain;
    ParticleSystem.MainModule cilinderMain;

    private void Awake()
    {
        circleMain = circle.main;
        cilinderMain = cilinder.main;
    }

    public void UpdateAreaTargetSize(float radius)
    {
        circleMain.startSize = radius * 3;
        cilinderMain.startSizeX = radius;
        cilinderMain.startSizeZ = radius;
    }
}
