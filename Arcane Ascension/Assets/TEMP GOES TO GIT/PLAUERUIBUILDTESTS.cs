using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PLAUERUIBUILDTESTS : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    public void UpdateText(string str)
    {
        text.text = str;
    }
}
