using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Disables a gameobject if it's stopped for a time interval.
/// </summary>
public class DisableIfStopped : MonoBehaviour, IReset
{
    private readonly float maxTime = 1f;
    private float lastTimeChecked;
    private Vector3 lastPosition;

    public void ResetAfterPoolDisable()
    {
        lastPosition = transform.position;
        lastTimeChecked = Time.time;
    }

    private void OnEnable()
    {
        lastPosition = transform.position;
        lastTimeChecked = Time.time;
    }

    private void Update()
    {
        if (Time.time > lastTimeChecked + maxTime)
        {
            if (transform.position.Similiar(lastPosition))
            {
                gameObject.SetActive(false);
                return;
            }
        }

        lastPosition = transform.position;
    }
}
