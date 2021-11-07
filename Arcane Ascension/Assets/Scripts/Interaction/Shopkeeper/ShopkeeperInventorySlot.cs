using UnityEngine;
using TMPro;

/// <summary>
/// Class responsible for a shopkeeper slot.
/// </summary>
public class ShopkeeperInventorySlot : MonoBehaviour, IInterectableWithCanvas
{
    [SerializeField] private Transform itemModelParent;
    public Transform ItemModelParent => itemModelParent;

    private GameObject canvas;
    private TextMeshProUGUI textMeshPro;

    private void Awake()
    {
        canvas = GetComponentInChildren<Canvas>().gameObject;
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void ItemBought() =>
        canvas.SetActive(false);

    public void UpdateInformation(string text)
    {
        textMeshPro.text = text;
    }
}
