using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Class responsible for handling information of an ability passive card.
/// </summary>
public class AbilityPassiveCard : MonoBehaviour
{
    /// <summary>
    /// Property to know which passive this card contains.
    /// </summary>
    public IPassive PassiveOnCard { get; set; }

    // Components
    private PlayerStats playerStats;
    private AbilitiesCanvas abilitiesCanvas;
    private PlayerInteraction playerInteraction;
    private AbilityPassiveCardText thisCardInformation;

    private void Awake()
    {
        playerInteraction = FindObjectOfType<PlayerInteraction>();
        abilitiesCanvas = GetComponentInParent<AbilitiesCanvas>();
        playerStats = FindObjectOfType<PlayerStats>();
        thisCardInformation = GetComponentInChildren<AbilityPassiveCardText>();
    }

    private void OnDisable()
    {
        PassiveOnCard = null;
    }

    /// <summary>
    /// Updates card info.
    /// </summary>
    public void UpdateInformation()
    {
        if (PassiveOnCard != null)
        {
            thisCardInformation.ShowEmptyCard(false);
            thisCardInformation.UpdateInfo(PassiveOnCard);
        }
        else
        {
            thisCardInformation.ShowEmptyCard(true);
        }
    }

    /// <summary>
    /// This method is called with button press if the player is selecting a passive.
    /// </summary>
    public void AddPassive()
    {
        if (PassiveOnCard != null)
        {
            if (playerInteraction.LastObjectInteracted != null)
            {
                LootSoundPoolCreator.Pool.InstantiateFromPool(
                    LootAndInteractionSoundType.ObtainPassiveOrb.ToString(),
                    playerInteraction.LastObjectInteracted.transform.position, Quaternion.identity);

                if (playerInteraction.LastObjectInteracted.TryGetComponent(out Chest chest) == false)
                {
                    // Deactivates the passive orb
                    playerInteraction.LastObjectInteracted.SetActive(false);
                }
            }

            PassiveOnCard.Execute(playerStats);
            playerStats.CurrentPassives.Add(PassiveOnCard);
            abilitiesCanvas.DisableAll();
        }
    }
}
