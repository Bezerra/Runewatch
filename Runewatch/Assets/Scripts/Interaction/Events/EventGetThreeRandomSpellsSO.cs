using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scritable object responsible for getting 3 random spells from all spells list.
/// </summary>
[CreateAssetMenu(menuName = "Events/Event Get Three Random Spells",
    fileName = "Event Get Three Random Spells")]
public class EventGetThreeRandomSpellsSO : EventAbstractSO
{
    [Tooltip("These weights only serve as reference for the curves bellow, affected by skill tree passives")]
    [SerializeField] private AnimationCurve defaultTierWeights;

    [Header("These weights are serialized but will ALLWAYS be set through code")]
    [SerializeField] private bool enableWeightsView;

    [EnableIf("enableWeightsView")]
    [SerializeField] private List<AnimationCurve> spellWeights;

    // Scriptable object that saves spells result
    [SerializeField] private RandomAbilitiesToChooseSO abilitiesToChose;

    [SerializeField] private SkillTreePassiveSO[] masteryOfTheArtsPassive;

    /// <summary>
    /// Finds all spells and executes GetSpell method.
    /// </summary>
    public override void Execute(AbstractEventOnInteraction invoker, PlayerInteraction interactor = null)
    {
        CharacterSaveDataController stpData =
            FindObjectOfType<CharacterSaveDataController>();

        for (int i = 0; i < spellWeights.Count; i++)
        {
            if (i > 0)
            {
                spellWeights[i].MoveKey(2, new Keyframe(3,
                    defaultTierWeights.Evaluate(3) +
                    (defaultTierWeights.Evaluate(3) * masteryOfTheArtsPassive[i - 1].Amount * 0.01f)));
                spellWeights[i].MoveKey(1, new Keyframe(2,
                    defaultTierWeights.Evaluate(2) +
                    (defaultTierWeights.Evaluate(2) * masteryOfTheArtsPassive[i - 1].Amount * 0.01f)));
                spellWeights[i].MoveKey(0, new Keyframe(1,
                    1 - (spellWeights[i].keys[2].value + spellWeights[i].keys[1].value)));
            }
            else
            {
                spellWeights[i].MoveKey(2, new Keyframe(3, defaultTierWeights.Evaluate(3)));
                spellWeights[i].MoveKey(1, new Keyframe(2, defaultTierWeights.Evaluate(2)));
                spellWeights[i].MoveKey(0, new Keyframe(1,
                    1 - (spellWeights[i].keys[2].value + spellWeights[i].keys[1].value)));
            }
        }

        // Components to get possible spells
        PlayerSpells playerSpells = FindObjectOfType<PlayerSpells>();
        IList<SpellSO> allSpellsDefault = FindObjectOfType<AllSpells>().SpellList;
        IList<SpellSO> allSpellsAvailable = new List<SpellSO>();
        IList<float> availableSpellsWeight = new List<float>();
        // All save data expertises. Neutral is added as 2 cause it doesn't have expertise
        IDictionary<ElementType, byte> expertises = new Dictionary<ElementType, byte>()
        {
            { ElementType.Umbra, stpData.SaveData.UmbraExpertise }, 
            { ElementType.Terra, stpData.SaveData.TerraExpertise },
            { ElementType.Fulgur, stpData.SaveData.FulgurExpertise }, 
            { ElementType.Ignis, stpData.SaveData.IgnisExpertise },
            { ElementType.Lux, stpData.SaveData.LuxExpertise }, 
            { ElementType.Natura, stpData.SaveData.NaturaExpertise },
            { ElementType.Aqua, stpData.SaveData.AquaExpertise }, 
            { ElementType.Neutral, 2 }
        };

        // Gets which index of master of the arts the player is currently at
        int masterOfTheArtsIndex = 0;
        for (int i = 0; i < masteryOfTheArtsPassive.Length; i++)
        {
            if (stpData.SaveData.MasteryOfTheArts == masteryOfTheArtsPassive[i].Amount)
            {
                // Adds index + 1 because weights list 0 is the default weight
                masterOfTheArtsIndex = i + 1;
                break;
            }
        }

        // Uses that index to create a list with 3 weights,
        // depending on the stpData.MasteryOfTheArts passive
        for (int i = 0; i < 3; i++)
        {
            availableSpellsWeight.Add(
                spellWeights[masterOfTheArtsIndex].Evaluate(i + 1));
        }

        // Now that we have the possible spell tiers for each element expertise that the player can have

        // Creates a new list with all available spells
        foreach (SpellSO spell in allSpellsDefault)
        {
            // Checks dictionary and compares to tier -1, because save data values go from 0 to 2
            // meaning spell tier 1 == saveData 0, tier 2 == saveData 1, tier 3 == saveData 2
            if (expertises[spell.Element] >= spell.Tier - 1)
            {
                allSpellsAvailable.Add(spell);
            }
        }

        // Removes the spells that player already has from the list of all spells
        foreach (SpellSO spell in playerSpells.CurrentSpells)
            allSpellsAvailable.Remove(spell);

        // Removes spells that are not possible to obtain (depends on the player's current spells)
        // Checks if player does not have any spell yet
        int currentSpellsSize = 0;
        foreach (SpellSO playerSpell in playerSpells.CurrentSpells)
        {
            if (playerSpell != null) currentSpellsSize++;
        }
        // For all available spells
        for (int i = 0; i < allSpellsAvailable.Count; i++)
        {
            if (currentSpellsSize < 1)
            {
                // If the player doesn't have spells and this spell is > tier 1, removes it
                if (allSpellsAvailable[i].Tier > 1)
                {
                    allSpellsAvailable.Remove(allSpellsAvailable[i]);
                    i--;
                }

                continue;
            }

            // For the current spell (on loop) for ALL player spells
            // meaning this will will run 4 times, and check all current spells
            bool removeSpell = true;
            foreach (SpellSO playerSpell in playerSpells.CurrentSpells)
            {
                if (playerSpell == null)
                {
                    continue;
                }

                // If it's the same element, it must be lower tier or same tier + 1
                if (allSpellsAvailable[i].Element == playerSpell.Element)
                {
                    if (allSpellsAvailable[i].Tier <= playerSpell.Tier ||
                        allSpellsAvailable[i].Tier == playerSpell.Tier + 1)
                    {
                        removeSpell = false;
                    }
                }
                // For all other elements, it only accepts tier 1
                else
                {
                    if (allSpellsAvailable[i].Tier == 1)
                    {
                        removeSpell = false;
                    }
                }
                
                // This loop will remove all spells that are not possible to obtain yet
            }

            if (removeSpell)
            {
                allSpellsAvailable.Remove(allSpellsAvailable[i]);
                i--;
            }
        }

        // Creates array with 3 random spells
        abilitiesToChose.SpellResult = GetSpell(allSpellsAvailable, availableSpellsWeight);
    }

    /// <summary>
    /// Gets three random spells without repetition from the list of all spells.
    /// </summary>
    /// <returns>Returns a list off 3 random spells.</returns>
    private ISpell[] GetSpell(IList<SpellSO> allSpellsAvailable, IList<float> availableSpellsWeight)
    {
        ISpell[] resultSpells = new ISpell[3];
        System.Random random = new System.Random();

        for (int i = 0; i < 3; i++)
        {
            if (allSpellsAvailable.Count == 0)
                break;

            // Adds one because the first value can be 0 (in that cause it's tier 1)
            float spellTierWithWeight = 1 + random.RandomWeight(availableSpellsWeight);

            // Creates a new list with all possible spells of that tier
            IList<SpellSO> spellsWithTier = new List<SpellSO>();
            foreach(SpellSO spell in allSpellsAvailable)
            {
                if (spell.Tier == spellTierWithWeight)
                {
                    spellsWithTier.Add(spell);
                }
            }

            // If any spell of that tier exists,
            // gets a random spell from that created list
            int spellIndex;
            if(spellsWithTier.Count > 0)
            {
                spellIndex = Random.Range(0, spellsWithTier.Count);
                resultSpells[i] = spellsWithTier[spellIndex];
            }
            else
            {
                // If that's not possible, gets a random spell from all spells
                spellIndex = Random.Range(0, allSpellsAvailable.Count);
                resultSpells[i] = allSpellsAvailable[spellIndex];
            }

            // Removes it from all spells list
            allSpellsAvailable.Remove(resultSpells[i] as SpellSO);
        }
        return resultSpells;
    }
}
