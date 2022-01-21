using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverMouse : MonoBehaviour
{
    [SerializeField] private GameObject hover;

    private float timer;
    private bool onHover;

    private void Awake()
    {
        timer = 0;    
    }

    public void TurnOn()
    {
        timer += Time.deltaTime;
        onHover = true;
        StartCoroutine(StartTimer());
    }

    private IEnumerator StartTimer()
    {
        while (onHover)
        {
            timer += Time.deltaTime;
            if (timer > 0.5f) hover.SetActive(true);
            yield return null;
        }
    }

    public void TurnOff()
    {
        onHover = false;
        timer = 0;
        hover.SetActive(false);
    }
}
