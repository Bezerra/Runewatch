using UnityEngine;

/// <summary>
/// Basic singleton.
/// </summary>
public class SingletonBasic : MonoBehaviour
{
    public static SingletonBasic Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
