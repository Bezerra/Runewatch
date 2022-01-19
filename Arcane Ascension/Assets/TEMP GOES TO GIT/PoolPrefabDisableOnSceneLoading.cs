using UnityEngine;
using UnityEngine.SceneManagement;

public class PoolPrefabDisableOnSceneLoading : MonoBehaviour
{
    private void OnEnable() =>
        SceneManager.sceneUnloaded += DisableAllPrefabs;

    private void OnDisable() =>
        SceneManager.sceneUnloaded -= DisableAllPrefabs;

    private void DisableAllPrefabs(Scene arg0)
    {
        if (arg0.name != "ProceduralGeneration") return;

        foreach(Transform directChild in transform)
        {
            foreach (MonoBehaviour child in 
                    directChild.GetComponentsInChildren<MonoBehaviour>(true))
            {
                if (child.gameObject.activeSelf)
                    child.gameObject.SetActive(false);
            }
            directChild.gameObject.SetActive(true);
        }
    }
}
