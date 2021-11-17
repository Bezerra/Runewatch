using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Class responsible for three random abilities canvas and logic.
/// </summary>
public class AbilitySpellChoice : MonoBehaviour
{
    // Scriptable object with random abilities
    [SerializeField] private RandomAbilitiesToChooseSO randomAbilities;

    // Panels with 3 spells
    private AbilitySpellCard[] spellCards;

    // Ui
    [SerializeField] private GameObject backButton;
    [SerializeField] private Button rerollButton;
    [SerializeField] private TextMeshProUGUI rerollText;

    private CharacterSaveDataController stpData;

    private void Awake()
    {
        spellCards = GetComponentsInChildren<AbilitySpellCard>();
        stpData = FindObjectOfType <CharacterSaveDataController>();
    }

    private void OnEnable()
    {
        if (stpData.SaveData.Destiny > 0)
        {
            rerollText.text = "Rerolls: " + stpData.SaveData.Destiny.ToString() + " / 2";
            rerollText.gameObject.SetActive(true);
            rerollButton.gameObject.SetActive(true);
        }
        else
        {
            rerollText.gameObject.SetActive(false);
            rerollButton.gameObject.SetActive(false);
        }
        
        UpdateChildInformation();
    }

    /// <summary>
    /// Updates child cards information.
    /// </summary>
    private void UpdateChildInformation()
    {
        backButton.SetActive(false);

        if (randomAbilities.SpellResult != null)
        {
            // Updates spell cards with random spells obtained
            for (int i = 0; i < spellCards.Length; i++)
            {
                if (randomAbilities.SpellResult[i] != null)
                {
                    spellCards[i].SpellOnCard = randomAbilities.SpellResult[i];
                }
                spellCards[i].UpdateInformation();
            }

            // Enables a back button if there are no spells to choose
            bool deactivateCanvas = true;
            for (int i = 0; i < randomAbilities.SpellResult.Length; i++)
            {
                if (randomAbilities.SpellResult[i] != null)
                    deactivateCanvas = false;
            }
            if (deactivateCanvas)
            {
                backButton.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Reroll logic. Called with a UI button.
    /// </summary>
    public void Reroll()
    {
        UpdateChildInformation();

        if (stpData.SaveData.Destiny > 0)
        {
            stpData.SaveData.Destiny -= 1;
        }

        if (stpData.SaveData.Destiny > 0)
        {
            rerollText.text = "Rerolls: " + stpData.SaveData.Destiny.ToString() + " / 2";
            rerollText.gameObject.SetActive(true);
            rerollButton.gameObject.SetActive(true);
        }
        else
        {
            rerollText.gameObject.SetActive(false);
            rerollButton.gameObject.SetActive(false);
        }
    }
}
