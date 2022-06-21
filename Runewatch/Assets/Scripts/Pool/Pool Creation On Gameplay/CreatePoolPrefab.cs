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
            DontDestroyOnLoad(this);
        }
        else
        {
            // If there is another pool already, it will disable all enabled
            // objects and destroy this new instance

            CreatePoolPrefab.Instance.
                GetComponentInChildren<PoolPrefabDisable>().DisableAllPrefabs();

            Destroy(gameObject);
        }
    }
}
