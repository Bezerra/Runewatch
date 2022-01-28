using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;

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
        UpdateInformation();
    }

    /// <summary>
    /// Updates rotation and activated/deactivates itself if in player's range.
    /// </summary>
    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, cam.transform.position) < rangeToActivate)
        {
            if (canvas.activeSelf == false)
            {
                canvas.SetActive(true);
                canvas.transform.LookAt(cam.transform);
            }

            canvas.transform.LookAt(cam.transform);
        }
        else
        {
            if (canvas.activeSelf)
            {
                canvas.SetActive(false);
            }
        }
    }

    public void UpdateInformation(string text = null)
    {
        if (text == null)
        {
            textToDipslay.text = InputControlPath.ToHumanReadableString(
                interactionActionAsset.action.bindings[0].effectivePath,
                InputControlPath.HumanReadableStringOptions.OmitDevice);
        }
        else
        {
            textToDipslay.text = text;
        }
    }
}
