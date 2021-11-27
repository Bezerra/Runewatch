using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RebindingManager : MonoBehaviour
{
    private RebindingKey[] rebindKeys;

    private void Awake()
    {
        rebindKeys = GetComponentsInChildren<RebindingKey>(true);
    }

    private void Start()
    {
        foreach (RebindingKey rebinding in rebindKeys)
            rebinding.UpdateKey();
    }
}
