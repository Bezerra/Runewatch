using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
public class ArrowButtonElement : MonoBehaviour, ISelectHandler
{
    public UnityEvent arrowSelect;
    public void OnSelect(BaseEventData eventData)
    {
        if (arrowSelect != null)
        {
            arrowSelect.Invoke();
        }

        EventSystem.current.SetSelectedGameObject(this.transform.parent.gameObject);
    }

}
