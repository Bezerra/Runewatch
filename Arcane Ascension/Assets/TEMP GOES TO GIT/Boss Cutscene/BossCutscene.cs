using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BossCutscene : MonoBehaviour, IFindPlayer, IFindInput
{
    private Canvas playerUICanvas;
    private PlayerHandEffect playerHandEffect;
    private SkinnedMeshRenderer[] playerArm;
    [SerializeField] private GameObject bossHealthBar;
    private IInput input;
    private CinemachineBrain cam;

    private Vector3 defaultHandEffectPosition;

    private void Awake()
    {
        playerUICanvas = FindObjectOfType<PlayerUI>()?.GetComponent<Canvas>();
        playerHandEffect = FindObjectOfType<PlayerHandEffect>();
        playerArm = playerHandEffect?.GetComponentsInChildren<SkinnedMeshRenderer>();
        input = FindObjectOfType<PlayerInputCustom>();
        cam = Camera.main.GetComponent<CinemachineBrain>();

        if (playerHandEffect != null)
            defaultHandEffectPosition = playerHandEffect.transform.localPosition;

        cam.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.EaseInOut;
    }

    public void BossCutsceneStarted()
    {
        if (playerUICanvas == null) FindPlayer(null);
        playerUICanvas.enabled = false;
        foreach (SkinnedMeshRenderer smr in playerArm)
            smr.enabled = false;
        bossHealthBar.SetActive(false);
        playerHandEffect.transform.localPosition = new Vector3(10000, 10000, 10000);

        if (input == null) FindInput();
            input.SwitchActionMapToNone();

        cam.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut;
    }

    public void BossCutsceneEnded()
    {
        playerUICanvas.enabled = true;
        foreach (SkinnedMeshRenderer smr in playerArm)
            smr.enabled = true;
        bossHealthBar.SetActive(true);
        playerHandEffect.transform.localPosition = defaultHandEffectPosition;
        input.SwitchActionMapToGameplay();
    }

    public void FindPlayer(Player player)
    {
        if (player != null)
        {
            playerUICanvas = player.GetComponentInChildren<PlayerUI>().GetComponent<Canvas>();
            playerHandEffect = player.GetComponentInChildren<PlayerHandEffect>();
            playerArm = playerHandEffect.GetComponentsInChildren<SkinnedMeshRenderer>();
            defaultHandEffectPosition = playerHandEffect.transform.localPosition;
        }
        else
        {
            playerUICanvas = FindObjectOfType<PlayerUI>()?.GetComponent<Canvas>();
            playerHandEffect = FindObjectOfType<PlayerHandEffect>();
            playerArm = playerHandEffect?.GetComponentsInChildren<SkinnedMeshRenderer>();
            defaultHandEffectPosition = playerHandEffect.transform.localPosition;
        }
    }

    public void PlayerLost(Player player)
    {
        //
    }

    public void FindInput(PlayerInputCustom input = null)
    {
        this.input = FindObjectOfType<PlayerInputCustom>();
    }

    public void LostInput(PlayerInputCustom input = null)
    {
        //
    }
}
