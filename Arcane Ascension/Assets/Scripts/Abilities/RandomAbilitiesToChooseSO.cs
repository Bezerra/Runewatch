using UnityEngine;

/// <summary>
/// Scriptable object that saves random abilities from spell scrolls, chests or passive orbs.
/// </summary>
[CreateAssetMenu(menuName = "Create Once/Random Abilities to Choose",
    fileName = "Random Abilities to Choose")]
public class RandomAbilitiesToChooseSO : ScriptableObject
{
    public ISpell[] SpellResult { get; set; }

    // Fazer para passivas tambem
}
