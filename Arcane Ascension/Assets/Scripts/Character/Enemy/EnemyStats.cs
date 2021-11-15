using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Class responsible for handling enemy stats.
/// </summary>
public class EnemyStats : Stats
{
    public EnemyStatsSO EnemyAttributes => character.CommonValues.CharacterStats as EnemyStatsSO;

    // Creates a list of room weights 
    public IList<int> AvailableSpellsWeight { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        // Creates a list of spell weights 
        AvailableSpellsWeight = new List<int>();
    }

    protected override void Start()
    {
        base.Start();
        

        for (int i = 0; i < EnemyAttributes.AllEnemySpells.Count; i++)
        {
            AvailableSpellsWeight.Add(EnemyAttributes.AllEnemySpells[i].SpellWeight);
        }
    }
}
