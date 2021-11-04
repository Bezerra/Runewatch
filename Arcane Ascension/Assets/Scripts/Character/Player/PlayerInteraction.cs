using System.Collections;
using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Class responsible for player interaction.
/// </summary>
public class PlayerInteraction : MonoBehaviour
{
    // Target variables
    private Ray forwardRay;
    private RaycastHit objectHit;
    private bool objectTargeted;
    public GameObject LastObjectInteracted { get; set; }

    // Components
    private YieldInstruction wfs;
    private Transform eyes;
    private PlayerInputCustom input;


    private void Awake()
    {
        eyes = GetComponent<Player>().Eyes;
        input = FindObjectOfType<PlayerInputCustom>();
        wfs = new WaitForSeconds(0.2f);
        objectTargeted = false;
    }

    private void OnEnable()
    {
        input.Interact += Interact;
    }

    private void OnDisable()
    {
        input.Interact -= Interact;
    }

    /// <summary>
    /// Every WFS casts a ray to check if the player is aiming towards some interectable object.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Start()
    {
        while (true)
        {
            forwardRay = new Ray(eyes.position, eyes.forward);
            if (Physics.Raycast(forwardRay, out objectHit, 2, Layers.Interectable))
            {
                Debug.Log("hitting");
                objectTargeted = true;
            }
            else
            {
                objectTargeted = false;
            }
            yield return wfs;
        }
    }

    /// <summary>
    /// Exected when the player clicks the object on the crosshair.
    /// </summary>
    private void Interact()
    {
        if (objectTargeted)
        {
            if (objectHit.collider.TryGetComponentInParent(out IInterectable interectable))
            {
                interectable.Execute();
            }
        }
    }
}
