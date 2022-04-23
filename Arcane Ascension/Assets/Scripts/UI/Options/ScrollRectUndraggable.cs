using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Class used to prevent drag on scroll rect.
/// </summary>
public class ScrollRectUndraggable : ScrollRect
{
    public override void OnBeginDrag(PointerEventData eventData) { }
    public override void OnDrag(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData) { }
 
}
