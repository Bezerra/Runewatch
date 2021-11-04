using UnityEngine;
using TMPro;

/// <summary>
/// Class responsible for interectables with canvas.
/// </summary>
public class InterectableCanvasText : MonoBehaviour, IInterectableWithCanvas
{
    [SerializeField] private GameObject canvas;
    [Range(1f, 15f)][SerializeField] private float rangeToActivate;
    [SerializeField] private TextMeshProUGUI textMeshPro;

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
        UpdateInformation("???");
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, cam.transform.position) < rangeToActivate)
        {
            Enable();
            UpdateRotation();
        }
        else
        {
            Disable();
        }
    }

    public void Disable()
    {
        if (canvas.activeSelf)
        {
            canvas.SetActive(false);
        }
    }

    public void Enable()
    {
        if (canvas.activeSelf == false)
        {
            canvas.SetActive(true);
            UpdateRotation();
        }
    }

    public void UpdateInformation(string text)
    {
        textMeshPro.text = text;
    }

    public void UpdateRotation()
    {
        canvas.transform.LookAt(cam.transform);
    }
}
