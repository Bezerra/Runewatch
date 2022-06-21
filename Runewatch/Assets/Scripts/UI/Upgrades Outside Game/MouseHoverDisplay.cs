using System.Collections;
using UnityEngine;

/// <summary>
/// Class responsible for enabling a gameobject.
/// </summary>
public class MouseHoverDisplay : MonoBehaviour
{
    [SerializeField] private GameObject hoverGameobject;
    [SerializeField] private float timeToDisplay = 0.35f;

    // Variables
    private float timer;
    private bool onHover;

    // Components
    private UpdateHoverWindowInformation windowInformation;
    private SkillTreePassiveNode myPassiveNode;

    private void Awake()
    {
        windowInformation = GetComponentInChildren<UpdateHoverWindowInformation>(true);
        myPassiveNode = GetComponent<SkillTreePassiveNode>();
    }

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
                windowInformation.UpdateWindowDetails(myPassiveNode);
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
