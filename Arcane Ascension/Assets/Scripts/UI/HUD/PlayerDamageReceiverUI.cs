using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ExtensionMethods;

public class PlayerDamageReceiverUI : MonoBehaviour
{
    [SerializeField] private RectTransform imagePosition;

    private PlayerStats playerStats;

    private void Awake()
    {
        playerStats = GetComponentInParent<PlayerStats>();
        imagePosition.anchoredPosition = new Vector2(0, 180);
    }

    private void OnEnable()
    {
        playerStats.EventTakeDamage += UpdateUI;
    }

    private void OnDisable()
    {
        playerStats.EventTakeDamage -= UpdateUI;
    }

    private void UpdateUI(float emptyFloat)
    {

    }

    private void Update()
    {
        Vector3 toPosition = playerStats.transform.Direction(enemy.transform);
        float angleToPosition = Vector3.SignedAngle(playerStats.transform.forward, toPosition, Vector3.up);
        transform.localEulerAngles = new Vector3(0, 0, -angleToPosition);
    }
}
