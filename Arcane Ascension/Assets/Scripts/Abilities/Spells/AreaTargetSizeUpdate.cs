using UnityEngine;

/// <summary>
/// Class responsible for updating area target size.
/// </summary>
public class AreaTargetSizeUpdate : MonoBehaviour
{
    private Stats whoCast;

    [SerializeField] private ParticleSystem[] circles;
    [SerializeField] private ParticleSystem[] cylinders;

    private ParticleSystem.MainModule[] circleMain;
    private ParticleSystem.MainModule[] cylinderMain;
    private float[] initialCirclesSizes;
    private float[] initialCylindersSizesX;
    private float[] initialCylindersSizesZ;

    private void Awake()
    {
        circleMain = new ParticleSystem.MainModule[circles.Length];
        cylinderMain = new ParticleSystem.MainModule[cylinders.Length];
        initialCirclesSizes = new float[circles.Length];
        initialCylindersSizesX = new float[cylinders.Length];
        initialCylindersSizesZ = new float[cylinders.Length];

        for (int i = 0; i < circles.Length; i++)
        {
            circleMain[i] = circles[i].main;
            initialCirclesSizes[i] = circleMain[i].startSize.constant;
        }

        for (int i = 0; i < cylinders.Length; i++)
        {
            cylinderMain[i] = cylinders[i].main;
            initialCylindersSizesX[i] = cylinderMain[i].startSizeX.constant;
            initialCylindersSizesZ[i] = cylinderMain[i].startSizeZ.constant;
        }
    }

    /// <summary>
    /// Updates current particle sizes.
    /// </summary>
    /// <param name="radius">Radius to update size.</param>
    public void UpdateAreaTargetSize(float radius, Stats whoCast)
    {
        this.whoCast = whoCast;

        for (int i = 0; i < circleMain.Length; i++)
        {
            circleMain[i].startSize = radius * initialCirclesSizes[i];
        }

        for (int i = 0; i < cylinderMain.Length; i++)
        {
            cylinderMain[i].startSizeX = radius * initialCylindersSizesX[i];
            cylinderMain[i].startSizeZ = radius * initialCylindersSizesZ[i];
        }  
    }

    private void Update()
    {
        if (whoCast == null) gameObject.SetActive(false);
    }
}
