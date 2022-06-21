using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMPAHHHH : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (FindObjectOfType<PlayerStats>() != null)
            Debug.Log("AH");
    }
}
