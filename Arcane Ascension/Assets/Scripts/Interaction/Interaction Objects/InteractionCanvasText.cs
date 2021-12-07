using UnityEngine;
using TMPro;

/// <summary>
/// Class responsible for interectables with canvas.
/// </summary>
public class InteractionCanvasText : MonoBehaviour, IInteractableWithCanvas
{
    [Range(1f, 15f)][SerializeField] private float rangeToActivate;
    [SerializeField] private string textOnCanvas;

    // Components
    private Camera cam;
    private GameObject canvas;
    public TextMeshProUGUI TextMeshP { get; private set; }

    public bool CurrentlyActive => canvas.activeSelf;

    private void Awake()
    {
        cam = Camera.main;
        canvas = GetComponentInChildren<Canvas>().gameObject;
        TextMeshP = GetComponentInChildren<TextMeshProUGUI>();
        UpdateInformation(textOnCanvas);
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
        TextMeshP.text = text;
    }

    public void UpdateRotation()
    {
        canvas.transform.LookAt(cam.transform);
    }
}
