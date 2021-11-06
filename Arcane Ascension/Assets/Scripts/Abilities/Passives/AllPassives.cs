using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class with a getter for all passives scriptable object.
/// </summary>
public class AllPassives : MonoBehaviour
{
    [SerializeField] private AllPassivesSO allPassives;

    public List<PassiveSO> PassiveList =>
        allPassives.PassivesList;
}
