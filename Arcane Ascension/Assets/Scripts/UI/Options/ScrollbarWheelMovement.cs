using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollbarWheelMovement : MonoBehaviour
{
    [SerializeField] private Scrollbar scrollbar;

    private PlayerInputCustom input;

    private void OnEnable()
    {
        if (input == null)
        {
            input = FindObjectOfType<PlayerInputCustom>();
            input.
        }
    }
}
