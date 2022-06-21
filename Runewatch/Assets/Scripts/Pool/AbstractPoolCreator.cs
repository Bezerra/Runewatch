using UnityEngine;

/// <summary>
/// Empty abstract pool creator in order to have a common script.
/// </summary>
public abstract class AbstractPoolCreator : MonoBehaviour
{
    public static int AllPrefabsToInstantiate { get; set; }
    public static int InstantiatedPrefabs { get; set; }
}
