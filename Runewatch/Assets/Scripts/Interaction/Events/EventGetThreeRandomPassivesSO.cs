using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scritable object responsible for getting 3 random passives from all passives list.
/// </summary>
[CreateAssetMenu(menuName = "Events/Event Get Three Random Passives",
    fileName = "Event Get Three Random Passives")]
public class EventGetThreeRandomPassivesSO : EventAbstractSO
{
    // Components to get possible passives
    private IList<RunPassiveSO> allPassives;

    // Scriptable object that saves passives result
    [SerializeField] private RandomAbilitiesToChooseSO abilitiesToChose;

    /// <summary>
    /// Finds all spells and executes GetSpell method.
    /// </summary>
    public override void Execute(AbstractEventOnInteraction invoker, PlayerInteraction interactor = null)
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();

        allPassives = new List<RunPassiveSO>();
        List<RunPassiveSO> allPassivesDefault = FindObjectOfType<AllRunPassives>().PassiveList;
        foreach (RunPassiveSO passive in allPassivesDefault)
        {
            bool addSkill = false;

            // Ignores this loop if player already has this spell
            if (playerStats.CurrentPassives.Contains(passive))
            {
                continue;
            }

            // If player has no abilities yet, it will add everything
            if (playerStats.CurrentPassives.Count == 0)
            {
                if (passive.Tier == 1)
                    allPassives.Add(passive);

                continue;
            }

            // All passives in player
            foreach(IRunPassive passiveInPlayer in playerStats.CurrentPassives)
            {
                // If the types are the same
                if (passive.PassiveType == passiveInPlayer.PassiveType)
                {
                    // And the tier is 1 level higher max
                    if (passive.Tier == passiveInPlayer.Tier + 1)
                    {
                        addSkill = true;
                        break;
                    }
                    else
                    {
                        addSkill = false;
                    }
                }
                else
                {
                    if (passive.Tier == 1)
                        addSkill = true;
                }
            }
            if (addSkill == true)
                allPassives.Add(passive);       
        }

        // Creates array with 3 random spells
        abilitiesToChose.PassiveResult = GetPassive();
    }

    /// <summary>
    /// Gets three random passives without repetition from the list of all possible passives.
    /// </summary>
    /// <returns>Returns an array with random passives</returns>
    private IRunPassive[] GetPassive()
    {
        IRunPassive[] resultPassives = new IRunPassive[3];

        for (int i = 0; i < 3; i++)
        {
            if (allPassives.Count == 0)
            {
                resultPassives[i] = null;
                continue;
            }

            int passiveIndex = Random.Range(0, allPassives.Count);

            // Gets passive and removes it from all spells list
            resultPassives[i] = allPassives[passiveIndex];
            allPassives.Remove(allPassives[passiveIndex]);
        }

        return resultPassives;
    }
}
