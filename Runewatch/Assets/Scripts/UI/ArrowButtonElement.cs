using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
public class ArrowButtonElement : MonoBehaviour
{
    public UnityEvent arrowSelect;

    private ButtonGroup buttonGroup;

    public void Awake()
    {
        buttonGroup = GetComponentInParent<ButtonGroup>();
    }
    public void ScriptTrigger()
    {
        buttonGroup.audioSourceClick.Play();
        if (arrowSelect != null)
        {
            arrowSelect.Invoke();
        }

        
    }

}
