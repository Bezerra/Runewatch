using UnityEngine;
using TMPro;

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
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI description;

    private PlayerStats playerStats;
    private PlayerInteraction playerInteraction;
    private AbilitiesCanvas abilitiesCanvas;

    private void Awake()
    {
        abilitiesCanvas = GetComponentInParent<AbilitiesCanvas>();
        playerInteraction = FindObjectOfType<PlayerInteraction>();
        playerStats = FindObjectOfType<PlayerStats>();
    }

    private void OnEnable()
    {
        UpdateInformation();
    }

    /// <summary>
    /// Updates card info.
    /// </summary>
    public void UpdateInformation()
    {
        if (PassiveOnCard != null)
        {
            title.text = PassiveOnCard.Name;
            description.text = PassiveOnCard.Description;
        }
    }

    /// <summary>
    /// This method is called with button press if the player is selecting a passive.
    /// </summary>
    public void AddPassive()
    {
        // Destroys the spell scroll
        Destroy(playerInteraction.LastObjectInteracted.gameObject);

        PassiveOnCard.Execute(playerStats);
        playerStats.CurrentPassives.Add(PassiveOnCard);
        abilitiesCanvas.DisableAll();
    }
}
