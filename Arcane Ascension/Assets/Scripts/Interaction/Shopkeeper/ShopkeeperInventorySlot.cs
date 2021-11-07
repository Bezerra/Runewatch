using UnityEngine;
using System.Collections;
using TMPro;

/// <summary>
/// Class responsible for a shopkeeper slot.
/// </summary>
public class ShopkeeperInventorySlot : MonoBehaviour, IInterectableWithCanvas
{
    [SerializeField] private Transform itemModelParent;

    /// <summary>
    /// Parent transform of the prefab that will be spawned.
    /// </summary>
    public Transform ItemModelParent => itemModelParent;

    // Components
    private GameObject canvas;
    private TextMeshProUGUI textMeshPro;
    private Animator anim;

    private void Awake()
    {
        canvas = GetComponentInChildren<Canvas>().gameObject;
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
        anim = GetComponentInChildren<Animator>();
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(Random.Range(0f, 2f));
        anim.SetTrigger("Execute");
    }

    public void ItemBought() =>
        canvas.SetActive(false);

    public void UpdateInformation(string text) =>
        textMeshPro.text = text;


}
