using UnityEngine;

/// <summary>
/// Controls an icon. Turns it on or off depending if new arcane power points
/// were obtained.
/// </summary>
public class GrimoireUnspentPoints : MonoBehaviour
{
    [SerializeField] private GameObject icon;
    [SerializeField] private GameObject grimoireMenu;

    [SerializeField] private GameObject nodesParent;
    [SerializeField] private CurrencySO currency;

    public GameObject NodesParent { get; set; }

    private bool enableIcon;

    private void Awake()
    {
        NodesParent = nodesParent;

        PlayerPrefs.SetInt("SceneLoadedTimes", +1);
        Debug.Log(PlayerPrefs.GetInt("SceneLoadedTimes"));
    }

    public void OnEnable()
    {
        // Loads the last icon state from when the game was closed
        if (PlayerPrefs.GetInt("UnspentPoints") == 1)
        {
            enableIcon = true;
        }
        else
        {
            enableIcon = false;
        }
        if (PlayerPrefs.GetInt("SceneLoadedTimes") > 1) return;

        icon.SetActive(false);

        SkillTreePassiveNode[] allNodes =
            NodesParent.GetComponentsInChildren<SkillTreePassiveNode>(true);

        enableIcon = false;

        foreach (SkillTreePassiveNode node in allNodes)
        {
            // If it's default spell
            if (node.NodePassives[0].Cost == 0)
            {
                // For next nodes on default spell
                foreach (SkillTreePassiveNode nextNode in node.NextConnectionNodes)
                {
                    // Ignores if already unlocked
                    if (nextNode.IsUnlocked) continue;

                    // If all these nodes have the previous connections unlocked
                    foreach (SkillTreePassiveNode previousNode in nextNode.PreviousConnectionNodes)
                    {
                        // Checks if the player has enough arcane power for next passives
                        // adjacent to unlocked ones
                        if (currency.
                            CanSpend(CurrencyType.ArcanePower, nextNode.NodePassives[0].Cost))
                        {
                            enableIcon = true;
                        }
                    }
                }
            }

            // Ignores null or maxed passives
            if (node.NodePassive == null) continue;
            if (node.NodePassive.Tier == node.NodePassives.Length) continue;

            // Checks if the player has enough arcane power for current unlocked passives
            // next tier
            if (currency.
                CanSpend(CurrencyType.ArcanePower, node.NodePassiveNext.Cost))
            {
                enableIcon = true;
            }

            if (node.NextConnectionNodes == null ||
                node.NextConnectionNodes.Count < 1)
                continue;

            // For the ALREADY UNLOCKED NEXT nodes
            foreach (SkillTreePassiveNode nextNode in node.NextConnectionNodes)
            {
                // Ignores null or maxed passives
                if (nextNode.NodePassive == null) continue;
                if (nextNode.NodePassive.Tier == nextNode.NodePassives.Length) continue;

                // Checks if the player has enough arcane power for next passives
                // adjacent to unlocked ones
                if (currency.
                    CanSpend(CurrencyType.ArcanePower, nextNode.NodePassiveNext.Cost))
                {
                    enableIcon = true;
                }
            }

            // For nodes that are on next connections but are NOT UNLOCKED YET
            foreach (SkillTreePassiveNode nextNodeBlocked in node.NextConnectionNodes)
            {
                if (nextNodeBlocked.IsUnlocked) continue;

                bool nodeCanBeUnlocked = true;
                foreach (SkillTreePassiveNode previousNode in nextNodeBlocked.PreviousConnectionNodes)
                {
                    if (previousNode.IsUnlocked == false)
                    {
                        nodeCanBeUnlocked = false;
                    }
                }

                if (nodeCanBeUnlocked == false) continue;

                // Checks if the player has enough arcane power for next passives
                // adjacent to unlocked ones
                if (currency.
                    CanSpend(CurrencyType.ArcanePower, nextNodeBlocked.NodePassives[0].Cost))
                {
                    enableIcon = true;
                }
            }
        }

        PlayerPrefs.SetInt("UnspentPoints", enableIcon == true ? 1 : 0);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("UnspentPoints", enableIcon == true ? 1 : 0);
    }

    private void Update()
    {
        if (grimoireMenu.activeSelf == false)
        {
            if (enableIcon)
            {
                icon.SetActive(true);
            }
            else
            {
                icon.SetActive(false);
            }
        }
    }
}
