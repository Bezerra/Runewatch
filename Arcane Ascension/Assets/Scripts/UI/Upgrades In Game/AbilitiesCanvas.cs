using UnityEngine;

/// <summary>
/// Class responsible for enabling child ability canvas.
/// </summary>
public class AbilitiesCanvas : MonoBehaviour
{
    [SerializeField] private GameObject threeSpellCanvas;
    [SerializeField] private GameObject oneSpellCanvas;
    [SerializeField] private GameObject threePassiveCanvas;

    public void EnableThreeSpellCanvas() =>
        threeSpellCanvas.SetActive(true);

    public void EnableOneSpellCanvas() =>
        oneSpellCanvas.SetActive(true);

    public void EnableThreePassiveCanvas() =>
        threePassiveCanvas.SetActive(true);
}
