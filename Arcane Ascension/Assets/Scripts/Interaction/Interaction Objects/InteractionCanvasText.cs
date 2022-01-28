using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

/// <summary>
/// Class responsible for interectables with canvas.
/// </summary>
public class InteractionCanvasText : MonoBehaviour, IInteractableWithCanvas
{
    [Range(1f, 15f)][SerializeField] private float rangeToActivate;
    [SerializeField] private InputActionReference interactionActionAsset;

    // Components
    private Camera cam;
    [SerializeField] private GameObject canvas;
    [SerializeField] public TextMeshProUGUI textToDipslay;

    public bool CurrentlyActive => canvas.activeSelf;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void OnEnable()
    {
        cam = Camera.main;
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

    public void UpdateInformation(string text = null)
    {
        if (text == null)
        {
            InputControlPath.ToHumanReadableString(
                interactionActionAsset.action.bindings[0].effectivePath,
                InputControlPath.HumanReadableStringOptions.OmitDevice);
        }
        else
        {
            textToDipslay.text = text;
        }
    }

    public void UpdateRotation()
    {
        canvas.transform.LookAt(cam.transform);
    }
}
