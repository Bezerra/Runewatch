using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class responsible for updating minimap icon images, sizes and canvas order.
/// </summary>
public class MinimapIcon : MonoBehaviour
{
    [SerializeField] private MinimapIconsSO allIcons;
    [SerializeField] private MinimapIconType iconType;
    [SerializeField] private bool disableOnStart;

    private Image iconImage;
    private RectTransform rectTransform;
    private Canvas canvas;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        iconImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();

        // Compares this icon with a list of all icons
        // Updates icon's imagine depending on the image set in that list
        foreach (MinimapIconInformation iconInfo in allIcons.IconsInformation)
        {
            if (iconInfo.MinimapIconType == iconType)
            {
                iconImage.sprite = iconInfo.MinimapIconTexture;
                rectTransform.localScale = new Vector3(iconInfo.Scale, iconInfo.Scale, iconInfo.Scale);
                canvas.sortingOrder = iconInfo.CanvasOrder;
            }
        }
    }

    private void OnEnable()
    {
        if (disableOnStart)
            SetIconActive(false);
    }

    public void SetIconActive(bool condition) => 
        gameObject.transform.parent.gameObject.SetActive(condition);
}
