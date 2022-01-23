using UnityEngine;
using UnityEngine.SceneManagement;

public class PoolPrefabDisable : MonoBehaviour
{
    public void DisableAllPrefabs()
    {
        foreach(Transform directChild in transform)
        {
            foreach (MonoBehaviour child in 
                    directChild.GetComponentsInChildren<MonoBehaviour>())
            {
                if (child.gameObject.activeSelf)
                {
                    if (child.TryGetComponent(out AbstractPoolCreator poolCreator) == false)
                    {
                        child.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
