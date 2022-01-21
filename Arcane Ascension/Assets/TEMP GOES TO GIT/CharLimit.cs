using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class CharLimit : MonoBehaviour
{
    [SerializeField] private RectTransform r;
    [SerializeField] private Transform rectCorner;


    private void Start()
    {
        r.pivot = new Vector2(0, 1);


    }

    private void OnValidate()
    {
        Rect screen = new Rect(Vector2.zero, new Vector3(Screen.currentResolution.width, Screen.currentResolution.height));

        if (screen.Contains(rectCorner.position))
            Debug.Log("Contains");
    }

    private void Update()
    {
        Rect screen = new Rect(Vector2.zero, new Vector3(Screen.currentResolution.width, Screen.currentResolution.height));
        Vector3[] corners = new Vector3[4];
        r.GetWorldCorners(corners);


        if (screen.Contains(corners[2]) == false)
            r.pivot = new Vector2(1, 1);

    }
}
