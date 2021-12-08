using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// TEMPORARY CLASS FOR DELIVERY.
/// </summary>
public class MouseSensitivity : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private void Start()
    {
        slider.value = PlayerPrefs.GetFloat("Mouse", 1);
    }

    public void Sensitivity(float value)
    {
        PlayerPrefs.SetFloat("Mouse", value);
    }
}
