using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMPTOCHANGEINPUT : MonoBehaviour
{
    [SerializeField] private bool switchToInput;

    private void Start()
    {
        if (switchToInput)
            FindObjectOfType<PlayerInputCustom>().SwitchActionMapToUI();
    }
}
