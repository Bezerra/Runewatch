using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class with a getter for all run passives scriptable object.
/// </summary>
public class AllRunPassives : MonoBehaviour
{
    [SerializeField] private AllRunPassivesSO allPassives;

    public List<RunPassiveSO> PassiveList =>
        allPassives.PassivesList;
}
