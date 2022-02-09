using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ExtensionMethods;

/// <summary>
/// Class responsible for updating player's received damage UI.
/// </summary>
public class PlayerDamageReceiverUI : MonoBehaviour
{
    [SerializeField] private List<RectTransform> damageReceivedParent;
    [SerializeField] private List<Image> damageReceivedImage;

    private PlayerStats playerStats;
    private YieldInstruction wffu;

    private void Awake()
    {
        playerStats = GetComponentInParent<PlayerStats>();
        wffu = new WaitForFixedUpdate();
    }

    private void OnEnable()
    {
        playerStats.EventTakeDamagePosition += UpdateUI;
    }

    private void OnDisable()
    {
        playerStats.EventTakeDamagePosition -= UpdateUI;
    }

    private void UpdateUI(Vector3 damageDirection)
    {
        if (PlayerPrefs.GetFloat(PPrefsOptions.DamageIndicator.ToString(), 1) == 1)
            StartCoroutine(UpdateUICoroutine(damageDirection));
    }

    /// <summary>
    /// Updates damage receives UI depending on the damage direction.
    /// </summary>
    /// <param name="damageDirection">Damage direction.</param>
    /// <returns>WFFU.</returns>
    private IEnumerator UpdateUICoroutine(Vector3 damageDirection)
    {
        if (damageDirection != Vector3.zero)
        {
            // Calculates direction and angle
            Vector3 toPosition = playerStats.transform.Direction(damageDirection);
            float angleToPosition =
                Vector3.SignedAngle(toPosition, playerStats.transform.forward, Vector3.up);

            // Gets a disabled image
            int i = 1000;
            for (int j = 0; j < damageReceivedParent.Count; j++)
            {
                if (damageReceivedImage[j].gameObject.activeSelf == false)
                {
                    i = j;
                    break;
                }
            }
            if (i == 1000) i = 0;

            damageReceivedImage[i].gameObject.SetActive(true);

            // Sets parent rotation
            damageReceivedParent[i].transform.localEulerAngles = new Vector3(0, 0, angleToPosition);

            float currentTime = Time.time;
            float imageAlpha = 1;
            while (true)
            {
                if (imageAlpha <= 0)
                {
                    damageReceivedImage[i].gameObject.SetActive(false);
                    break;
                }

                if (currentTime > 0.25f)
                {
                    imageAlpha -= Time.fixedDeltaTime;
                }

                damageReceivedImage[i].color = new Color(1, 1, 1, imageAlpha);

                currentTime += Time.fixedDeltaTime;

                yield return wffu;
            }
        }
    }
}
