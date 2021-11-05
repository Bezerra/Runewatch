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
    private IList<PassiveSO> allPassives;

    // Scriptable object that saves passives result
    [SerializeField] private RandomAbilitiesToChooseSO abilitiesToChose;

    /// <summary>
    /// Finds all spells and executes GetSpell method.
    /// </summary>
    public override void Execute(EventOnInteraction invoker)
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();

        allPassives = new List<PassiveSO>();
        List<PassiveSO> allPassivesDefault = FindObjectOfType<AllPassives>().PassiveList;
        foreach (PassiveSO passive in allPassivesDefault)
        {
            bool addSkill = false;

            // If player doesn't have the passive yet
            if (playerStats.CurrentPassives.Contains(passive) == false)
            {
                // If player has no abilities yet, it will add everything
                if (playerStats.CurrentPassives.Count == 0)
                {
                    if (passive.PassiveTier == 1)
                        allPassives.Add(passive);

                    continue;
                }

                // All passives in player
                foreach(IPassive passiveInPlayer in playerStats.CurrentPassives)
                {
                    // If the types are the same
                    if (passiveInPlayer.PassiveType == passive.PassiveType)
                    {
                        // And the tier is 1 level higher max
                        if (passive.PassiveTier == passiveInPlayer.PassiveTier + 1)
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
                        if (passive.PassiveTier == 1)
                            addSkill = true;
                    }
                }

                if (addSkill == true)
                    allPassives.Add(passive);
            }           
        }

        // Creates array with 3 random spells
        abilitiesToChose.PassiveResult = GetPassive();

        //foreach (IPassive pass in abilitiesToChose.PassiveResult)
        //{
        //
        //        Debug.Log(pass);
        //
        //    
        //}

        playerStats.CurrentPassives.Add(abilitiesToChose.PassiveResult[0]);

        foreach (IPassive pass in playerStats.CurrentPassives)
        {
    
                Debug.Log(pass);
            
        }
    }

    /// <summary>
    /// Gets three random passives without repetition from the list of all possible passives.
    /// </summary>
    /// <returns>Returns an array with random passives</returns>
    private IPassive[] GetPassive()
    {
        IPassive[] resultPassives = new IPassive[3];

        for (int i = 0; i < 3; i++)
        {
            if (allPassives.Count <= i)
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
