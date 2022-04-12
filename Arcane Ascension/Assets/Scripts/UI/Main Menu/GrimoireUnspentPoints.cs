using UnityEngine;

/// <summary>
/// Controls an icon. Turns it on or off depending if new arcane power points
/// were obtained.
/// </summary>
public class GrimoireUnspentPoints : MonoBehaviour
{
    [SerializeField] private GameObject icon;
    [SerializeField] private GameObject grimoireMenu;

    private CharacterSaveDataController characterSaveData;

    [SerializeField] private GameObject nodesParent;
    [SerializeField] private CurrencySO currency;

    private void Awake()
    {
        characterSaveData = FindObjectOfType<CharacterSaveDataController>();
    }

    private void OnEnable()
    {
        return;

        icon.SetActive(false);

        SkillTreePassiveNode[] allNodes = 
            nodesParent.GetComponentsInChildren<SkillTreePassiveNode>(true);

        foreach(SkillTreePassiveNode node in allNodes)
        {
            if (node.NodePassive == null) continue;

            if (node.NodePassive.Tier == node.NodePassives.Length) continue;

            // Checks if the player has enough arcane power
            if (currency.
                CanSpend(CurrencyType.ArcanePower, node.NodePassiveNext.Cost))
            {
                icon.SetActive(true);
                Debug.Log(node.NodePassive.Name);
            }
        }

        /*
        if (characterSaveData.SaveData.ArcanePower > 
            PlayerPrefs.GetInt("ArcanePowerOnLastEnable", 0))
        {
            icon.SetActive(true);
        }
        else
        {
            icon.SetActive(false);
        }

        PlayerPrefs.SetInt("ArcanePowerOnLastEnable", 
            characterSaveData.SaveData.ArcanePower);
        */
    }

    private void Update()
    {
        if (grimoireMenu.activeSelf)
        {
            if (icon.activeSelf)
            {
                icon.SetActive(false);
            }
        }
    }
}
