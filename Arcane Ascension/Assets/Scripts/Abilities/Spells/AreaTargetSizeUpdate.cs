using UnityEngine;

/// <summary>
/// Class responsible for updating area target size.
/// </summary>
public class AreaTargetSizeUpdate : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] circles;
    [SerializeField] private ParticleSystem[] cylinders;

    ParticleSystem.MainModule[] circleMain;
    ParticleSystem.MainModule[] cylinderMain;

    private void Awake()
    {
        circleMain = new ParticleSystem.MainModule[circles.Length];
        cylinderMain = new ParticleSystem.MainModule[cylinders.Length];

        for (int i = 0; i < circles.Length; i++)
            circleMain[i] = circles[i].main;

        for (int i = 0; i < cylinders.Length; i++)
            cylinderMain[i] = cylinders[i].main;
    }

    public void UpdateAreaTargetSize(float radius)
    {
        for (int i = 0; i < circleMain.Length; i++)
        {
            circleMain[i].startSize = radius * circleMain[i].startSize.constant;
        }

        for (int i = 0; i < cylinderMain.Length; i++)
        {
            cylinderMain[i].startSizeX = radius * cylinderMain[i].startSizeX.constant;
            cylinderMain[i].startSizeZ = radius * cylinderMain[i].startSizeZ.constant;
        }
            
    }
}
