using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Class responsible for updating level intro text information.
/// </summary>
public class LevelIntroInformation : MonoBehaviour
{
    // Components
    private RunSaveDataController runSaveData;
    private Animator anim;

    [Header("Element levels description")]
    [SerializeField] private string ignisName;
    [SerializeField] private string fireDemonRaidName;

    [Header("Text variables")]
    [SerializeField] private TextMeshProUGUI elementLevelDescription;
    [SerializeField] private TextMeshProUGUI floorDescription;

    private void Awake()
    {
        runSaveData = FindObjectOfType<RunSaveDataController>();
        anim = GetComponent<Animator>();
    }

    public void PlayAnimation()
    {
        string dungeonElementText = "";
        ElementType element = FindObjectOfType<LevelGenerator>().Element;
        floorDescription.text = "";

        // If it's boss raid mode
        if (SceneManager.GetActiveScene().name == SceneEnum.BossRaid.ToString())
        {
            switch (element)
            {
                case ElementType.Ignis:
                    dungeonElementText = fireDemonRaidName.ToString();
                    break;
            }
        }
        else
        {
            // If it's normal gameplay
            switch (element)
            {
                case ElementType.Ignis:
                    dungeonElementText = ignisName.ToString();
                    break;
            }

            floorDescription.text = "Floor " + runSaveData.SaveData.DungeonSavedData.Floor;
        }

        elementLevelDescription.text = dungeonElementText;
        anim.enabled = true;
    }
}
