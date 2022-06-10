using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMessageTrigger : MonoBehaviour
{
    [SerializeField] private string title;
    [SerializeField] private Sprite sprite;
    [SerializeField] private string message;
    [SerializeField] private bool triggerOnEnable;

    [SerializeField] private GameObject tutController;

    public void OnEnable()
    {
        if(!triggerOnEnable) return;


    }

}
