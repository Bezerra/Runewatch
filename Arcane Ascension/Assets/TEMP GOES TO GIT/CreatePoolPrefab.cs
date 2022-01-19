using UnityEngine;

/// <summary>
/// Class responsible for creating pools prefab in first playthrough.
/// </summary>
public class CreatePoolPrefab : MonoBehaviour
{
    public static CreatePoolPrefab Instance { get; private set; }

    [SerializeField] private GameObject poolsPrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            GameObject prefab = Instantiate(poolsPrefab);
            prefab.transform.parent = this.transform;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
