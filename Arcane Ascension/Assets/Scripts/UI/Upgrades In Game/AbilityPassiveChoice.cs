using UnityEngine;

/// <summary>
/// Class responsible for three random abilities canvas and logic.
/// </summary>
public class AbilityPassiveChoice : MonoBehaviour
{
    // Scriptable object with random abilities
    [SerializeField] private RandomAbilitiesToChooseSO randomAbilities;

    // Panels with 3 spells
    private AbilityPassiveCard[] passiveCards;

    // Components
    private PlayerInputCustom input;

    private void Awake()
    {
        passiveCards = GetComponentsInChildren<AbilityPassiveCard>();
        input = FindObjectOfType<PlayerInputCustom>();
    }

    private void OnEnable()
    {
        // Updates spell cards with random spells obtained
        for (int i = 0; i < passiveCards.Length; i++)
        {
            if (randomAbilities.PassiveResult[i] != null)
            {
                passiveCards[i].PassiveOnCard = randomAbilities.PassiveResult[i];
            }
        }
    }
}
