using System.Collections;
using UnityEngine;

/// <summary>
/// Class responsible for enabling a gameobject.
/// </summary>
public class MouseHoverDisplay : MonoBehaviour
{
    [SerializeField] private GameObject hoverGameobject;
    [SerializeField] private float timeToDisplay = 0.5f;

    private float timer;
    private bool onHover;

    private void Start() =>
        timer = 0;    

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
            if (timer > timeToDisplay)
            {
                hoverGameobject.SetActive(true);
                break;
            }
            yield return null;
        }
    }

    public void TurnOff()
    {
        onHover = false;
        timer = 0;
        hoverGameobject.SetActive(false);
    }
}
