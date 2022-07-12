using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCanvasUI : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuButtons;
    [SerializeField] private GameObject difficultyMenu;
    [SerializeField] private GameObject skillTreeMenu;

    public void Start()
    {
        StartCoroutine(UpdateAllMenuVariables());
    }

    public IEnumerator UpdateAllMenuVariables()
    {

        MainMenuButtonsDisable();
        DifficultyMenuEnable();
        SkillTreeMenuDisable();
        

        yield return null;

        MainMenuButtonsEnable();
        DifficultyMenuDisable();
        SkillTreeMenuEnable();
    }

    public void MainMenuButtonsEnable() => mainMenuButtons.SetActive(true);
    public void DifficultyMenuEnable() => difficultyMenu.SetActive(true);
    public void SkillTreeMenuEnable() => skillTreeMenu.SetActive(true);

    public void MainMenuButtonsDisable() => mainMenuButtons.SetActive(false);
    public void DifficultyMenuDisable() => difficultyMenu.SetActive(false);
    public void SkillTreeMenuDisable() => skillTreeMenu.SetActive(false);
}
