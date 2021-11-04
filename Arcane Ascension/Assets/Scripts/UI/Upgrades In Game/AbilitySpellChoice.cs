using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Class responsible for three random abilities canvas and logic.
/// </summary>
public class AbilitySpellChoice : MonoBehaviour
{
    // Scriptable object with random abilities
    [SerializeField] private RandomAbilitiesToChooseSO randomAbilities;

    [SerializeField] private List<TextMeshProUGUI> spellPanels;

    private void OnEnable()
    {
        for (int i = 0; i < spellPanels.Count; i++)
        {
            if (randomAbilities.SpellResult[i] != null)
            {
                spellPanels[i].text = randomAbilities.SpellResult[i].Name;
            }
            else
            {
                spellPanels[i].text = "No spell found";
            }
        }
    }


    public void TEMPBACKTOGAME()
    {
        FindObjectOfType<PlayerInputCustom>().SwitchActionMapToGameplay();
        Time.timeScale = 1;
    }
}
