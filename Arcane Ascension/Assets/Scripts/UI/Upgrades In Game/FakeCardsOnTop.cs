using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using ExtensionMethods;

/// <summary>
/// Class responsible for unrevealing spell and passive cards by fading out cards on top.
/// </summary>
public class FakeCardsOnTop : MonoBehaviour
{
    [SerializeField] private AbilityType abilityType;
    [SerializeField] private Button[] cardButtonsToActivate;
    [SerializeField] private Image[] images;

    // Children cards
    private AbilitySpellCard[] spellCards;
    private AbilityPassiveCard[] passiveCards;

    // Images and alpha
    private float alpha;
    private int allImagesDone;

    // Coroutine
    [Header("Reroll Variables")]
    [SerializeField] private EventAbstractSO rerollEvent;
    private IEnumerator rerollingCoroutine;
    private AbilitySpellChoice parentSpellChoiceCanvas;

    private void Awake()
    {
        parentSpellChoiceCanvas = GetComponentInParent<AbilitySpellChoice>();
    }

    private void OnEnable()
    {
        alpha = 1;
        allImagesDone = 0;

        // Children images on top alpha to 1
        for (int i = 0; i < images.Length; i++)
        {
            images[i].color = new Color(0, 0, 0, alpha * 2);
        }
        // Deactivates spells buttons
        for (int j = 0; j < cardButtonsToActivate.Length; j++)
        {
            cardButtonsToActivate[j].enabled = false;
        }

        switch(abilityType)
        {
            case AbilityType.Spell:
                spellCards = new AbilitySpellCard[cardButtonsToActivate.Length];
                for (int i = 0; i < cardButtonsToActivate.Length; i++)
                {
                    if (cardButtonsToActivate[i].gameObject.TryGetComponent(out AbilitySpellCard spellCard))
                        spellCards[i] = spellCard;
                }
                break;
            case AbilityType.Passive:
                passiveCards = new AbilityPassiveCard[cardButtonsToActivate.Length];
                for (int i = 0; i < cardButtonsToActivate.Length; i++)
                {
                    if (cardButtonsToActivate[i].gameObject.TryGetComponent(out AbilityPassiveCard passiveCard))
                        passiveCards[i] = passiveCard;
                }
                break;
        }
    }

    public void Rerolled()
    {
        // Deactivates spells buttons
        for (int j = 0; j < cardButtonsToActivate.Length; j++)
        {
            cardButtonsToActivate[j].enabled = false;
        }

        this.StartCoroutineWithReset(ref rerollingCoroutine, RerollCoroutine());
    }

    private IEnumerator RerollCoroutine()
    {
        alpha = 0;
        allImagesDone = 0;
        while (alpha < 1)
        {
            alpha += Time.unscaledDeltaTime * 2;
            for (int i = 0; i < images.Length; i++)
            {
                images[i].color = new Color(1, 1, 1, alpha);

                if (images[i].color.a <= 0)
                {
                    allImagesDone = i;
                }
            }
            yield return null;
        }

        if (rerollEvent != null)
            rerollEvent.Execute();

        if (parentSpellChoiceCanvas != null)
            parentSpellChoiceCanvas.Reroll();

        rerollingCoroutine = null;
    }

    private void Update()
    {
        if (rerollingCoroutine == null)
        {
            if (allImagesDone == images.Length - 1)
            {
                for (int i = 0; i < cardButtonsToActivate.Length; i++)
                {
                    switch (abilityType)
                    {
                        case AbilityType.Spell:
                            if (spellCards[i].SpellOnCard != null)
                                cardButtonsToActivate[i].enabled = true;
                            break;
                        case AbilityType.Passive:
                            if (passiveCards[i].PassiveOnCard != null)
                                cardButtonsToActivate[i].enabled = true;
                            break;
                    }
                }
                return;
            }

            alpha -= Time.unscaledDeltaTime * 2;

            for (int i = 0; i < images.Length; i++)
            {
                images[i].color = new Color(1, 1, 1, alpha + i);

                if (images[i].color.a <= 0)
                {
                    allImagesDone = i;
                }
            }
        }
    }

    private enum AbilityType { Spell, Passive, }
}
