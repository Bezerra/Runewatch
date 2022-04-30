using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCutscene : MonoBehaviour
{
    private Canvas playerUICanvas;
    private PlayerHandEffect playerHandEffect;
    private SkinnedMeshRenderer[] playerArm;
    [SerializeField] private GameObject bossHealthBar;

    private Vector3 defaultHandEffectPosition;

    private void Awake()
    {
        playerUICanvas = FindObjectOfType<PlayerUI>().GetComponent<Canvas>();
        playerHandEffect = FindObjectOfType<PlayerHandEffect>();
        playerArm = playerHandEffect.GetComponentsInChildren<SkinnedMeshRenderer>();

        defaultHandEffectPosition = playerHandEffect.transform.localPosition;
    }

    public void BossCutsceneStarted()
    {
        playerUICanvas.enabled = false;
        foreach (SkinnedMeshRenderer smr in playerArm)
            smr.enabled = false;
        bossHealthBar.SetActive(false);
        playerHandEffect.transform.localPosition = new Vector3(10000, 10000, 10000);
    }

    public void BossCutsceneEnded()
    {
        playerUICanvas.enabled = true;
        foreach (SkinnedMeshRenderer smr in playerArm)
            smr.enabled = true;
        bossHealthBar.SetActive(true);
        playerHandEffect.transform.localPosition = defaultHandEffectPosition;
    }
}
