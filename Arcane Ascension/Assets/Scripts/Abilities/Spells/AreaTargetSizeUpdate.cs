using UnityEngine;

/// <summary>
/// Class responsible for updating area target size.
/// </summary>
public class AreaTargetSizeUpdate : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] circles;
    [SerializeField] private ParticleSystem[] cylinders;

    private ParticleSystem.MainModule[] circleMain;
    private ParticleSystem.MainModule[] cylinderMain;
    private float[] initialCirclesSizes;
    private float[] initialCilindersSizesX;
    private float[] initialCilindersSizesZ;

    private void Awake()
    {
        circleMain = new ParticleSystem.MainModule[circles.Length];
        cylinderMain = new ParticleSystem.MainModule[cylinders.Length];
        initialCirclesSizes = new float[circles.Length];
        initialCilindersSizesX = new float[cylinders.Length];
        initialCilindersSizesZ = new float[cylinders.Length];

        for (int i = 0; i < circles.Length; i++)
        {
            circleMain[i] = circles[i].main;
            initialCirclesSizes[i] = circleMain[i].startSize.constant;
        }

        for (int i = 0; i < cylinders.Length; i++)
        {
            cylinderMain[i] = cylinders[i].main;
            initialCilindersSizesX[i] = cylinderMain[i].startSizeX.constant;
            initialCilindersSizesZ[i] = cylinderMain[i].startSizeZ.constant;
        }
    }

    public void UpdateAreaTargetSize(float radius)
    {
        for (int i = 0; i < circleMain.Length; i++)
        {
            circleMain[i].startSize = radius * initialCirclesSizes[i];
        }

        for (int i = 0; i < cylinderMain.Length; i++)
        {
            cylinderMain[i].startSizeX = radius * initialCilindersSizesX[i];
            cylinderMain[i].startSizeZ = radius * initialCilindersSizesZ[i];
        }
            
    }
}
