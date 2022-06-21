using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModalWindow : MonoBehaviour
{
    [Header("Header")]
    [SerializeField] private Transform headerArea;
    [SerializeField] private TextMeshProUGUI titleField;

    [Header("Content")]
    [SerializeField] private Transform horizontalLayoutArea;
    [SerializeField] private Transform imageContainer;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI imageText;

    [Header("Footer")]
    [SerializeField] private Transform footerArea;
    [SerializeField] private Button confirmButton;

    private Action onConfirmCallback;

    public void Confirm()
    {
        onConfirmCallback?.Invoke();

    }

    public void ShowTutorialMessage(string title, Sprite imageToShow,
        string message, Action confirmAction)
    {
        //Hides Title if string null
        bool hasTitle = string.IsNullOrEmpty(title);
        headerArea.gameObject.SetActive(hasTitle);
        titleField.text = title;

        image.sprite = imageToShow;
        imageText.text = message;

        onConfirmCallback = confirmAction;
    }
}
