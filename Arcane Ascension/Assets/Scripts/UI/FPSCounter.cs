using UnityEngine;

/// <summary>
/// Fps counter.
/// </summary>
public class FPSCounter : MonoBehaviour
{
    [SerializeField] private float timeToUpdateCounter = 1f;
    private float time;
    private int frameCounter;

    public int FrameRate { get; private set; }

    private void Update()
    {
        // Adds deltaTime every update
        time += Time.deltaTime;
        // Increments frame counter every update
        frameCounter++;

        // If current time as reached time to calculate fps
        if(time >= timeToUpdateCounter)
        {
            // Checks how many frames have been counted in X time
            FrameRate = Mathf.RoundToInt(frameCounter / time);

            // Resets variables
            time -= timeToUpdateCounter;
            frameCounter = 0;
        }
    }
}
