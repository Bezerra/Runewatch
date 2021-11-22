using UnityEngine;

/// <summary>
/// Class responsible for running status logic.
/// </summary>
public abstract class StatusBehaviour : MonoBehaviour
{
    /// <summary>
    /// Parent spell of this 
    /// </summary>
    public ISpell Spell { get; set; }

    /// <summary>
    /// Time of spawn.
    /// </summary>
    public float TimeSpawned { get; set; }

    /// <summary>
    /// Which character is being affected by this spell.
    /// </summary>
    public Stats CharacterHit { get; set; }

    /// <summary>
    /// Which character cast the spell of this status.
    /// </summary>
    public Stats WhoCast { get; set; }

    /// <summary>
    /// Method called after instantiating the spell.
    /// Must be called manually through this method instead of OnEnable or Start in order to prevent bugs.
    /// </summary>
    public void TriggerStartBehaviour()
    {
        if (Spell.StatusBehaviour != null)
            Spell.StatusBehaviour.StartBehaviour(this);
    }

    private void Update()
    {
        if (Spell.StatusBehaviour != null)
            Spell.StatusBehaviour.ContinuousUpdateBehaviour(this);
    }

    /// <summary>
    /// Disables status gameobject.
    /// </summary>
    public void DisableHitSpell() =>
        gameObject.SetActive(false);
}
