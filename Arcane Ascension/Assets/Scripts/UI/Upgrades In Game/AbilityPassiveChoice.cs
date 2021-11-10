using UnityEngine;

/// <summary>
/// Class responsible for three random abilities canvas and logic.
/// </summary>
public class AbilityPassiveChoice : MonoBehaviour
{
    // Scriptable object with random abilities
    [SerializeField] private RandomAbilitiesToChooseSO randomAbilities;

    [SerializeField] private GameObject backButton;

    // Panels with 3 spells
    [SerializeField] private AbilityPassiveCard[] passiveCards;
    [SerializeField] private Canvas[] passiveCardsTextCanvas;

    private void OnEnable()
    {
        backButton.SetActive(false);

        if (randomAbilities.PassiveResult != null)
        {
            // Updates spell cards with random spells obtained
            for (int i = 0; i < passiveCards.Length; i++)
            {
                if (randomAbilities.PassiveResult[i] != null)
                {
                    passiveCards[i].PassiveOnCard = randomAbilities.PassiveResult[i];
                }
                else
                {
                    passiveCards[i].PassiveOnCard = null;
                }
                passiveCards[i].UpdateInformation();
            }

            // Enables a back button if there are no passives to choose
            bool deactivateCanvas = true;
            for (int i = 0; i < randomAbilities.PassiveResult.Length; i++)
            {
                if (randomAbilities.PassiveResult[i] != null)
                    deactivateCanvas = false;
            }
            if (deactivateCanvas)
            {
                backButton.SetActive(true);
            }
        }
    }
}
