using System.Collections;
using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Class responsible for player interaction.
/// </summary>
public class PlayerInteraction : MonoBehaviour
{
    private Ray forwardRay;
    private RaycastHit objectHit;

    private YieldInstruction wfs;
    private Transform eyes;
    private PlayerInputCustom input;

    private void Awake()
    {
        eyes = GetComponent<Player>().Eyes;
        input = FindObjectOfType<PlayerInputCustom>();
        wfs = new WaitForSeconds(0.2f);
    }

    private void OnEnable()
    {
        input.Interact += Interact;
    }

    private void OnDisable()
    {
        input.Interact -= Interact;
    }

    private IEnumerator Start()
    {
        while (true)
        {
            forwardRay = new Ray(eyes.position, eyes.forward);
            if (Physics.Raycast(forwardRay, out objectHit, 2, Layers.Interectable))
            {
                // Show something on ui in here
                Debug.Log("hitting");
            }
            yield return wfs;
        }
    }

    private void Interact()
    {
        if (objectHit.collider.TryGetComponentInParent(out IInterectable interectable))
            interectable.Execute();
    }
}
