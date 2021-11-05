using UnityEngine;

/// <summary>
/// Class responsible for destroying an item after X seconds.
/// </summary>
public class DestroyAfterSeconds : MonoBehaviour
{
    [Range(10, 60)] [SerializeField] private float timeToDestroy;

    private void Awake()
    {
        Destroy(gameObject, timeToDestroy);
    }
}
