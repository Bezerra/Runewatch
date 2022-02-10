using System.Linq;
using UnityEngine;

/// <summary>
/// Class responsible for disabling prefabs created by a prefab creator.
/// </summary>
public class PoolPrefabDisable : MonoBehaviour
{
    public void DisableAllPrefabs()
    {
        foreach(Transform directChild in transform)
        {
            foreach (IReset reset in GetComponentsInChildren<IReset>(true))
                reset.ResetAfterPoolDisable();

            Transform[] objects = 
                directChild.GetComponentsInChildren<Transform>().Where(
                    i => i.gameObject != directChild.gameObject).ToArray();

            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i].gameObject.activeSelf)
                {
                    if (objects[i].parent == directChild)
                    {
                        objects[i].gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
