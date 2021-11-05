using UnityEngine;

/// <summary>
/// Scriptable object that saves random abilities from spell scrolls, chests or passive orbs.
/// This asset keeps getting updated every time the player interacts with any of these mentioned types of loot.
/// </summary>
[CreateAssetMenu(menuName = "Create Once/Random Abilities to Choose",
    fileName = "Random Abilities to Choose")]
public class RandomAbilitiesToChooseSO : ScriptableObject
{
    /// <summary>
    /// Three spells from scrolls and chests.
    /// </summary>
    public ISpell[] SpellResult { get; set; }

    /// <summary>
    /// Spell from a dropped spell.
    /// </summary>
    public ISpell DroppedSpell { get; set; }

    // Fazer para passivas tambem
}
