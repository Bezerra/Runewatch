using UnityEngine;

/// <summary>
/// Abstract class responsible for executing a spell behaviour.
/// </summary>
public abstract class SpellBehaviourAbstract : MonoBehaviour
{
    [SerializeField] protected SpellSO spell;

    public Transform Hand { get; private set; }
    public Transform Eyes { get; private set; }
    public IDamageable ThisIDamageable { get; private set; }

    private Stats whoCast;
    public Stats WhoCast 
    { 
        get => whoCast; 
        set 
        {
            // Updates variables if who cast is different from the last time this spell was cast
            if (whoCast != value)
            {
                whoCast = value;
                Hand = WhoCast.GetComponent<Character>().Hand;
                Eyes = WhoCast.GetComponent<Character>().Eyes;
                ThisIDamageable = WhoCast.GetComponent<IDamageable>();
            }
        }
    }

    public ISpell Spell => spell;

    /// <summary>
    /// Method called after instantiating the spell.
    /// Must be called manually through this method instead of OnEnable or Start in order to prevent bugs.
    /// </summary>
    public abstract void TriggerStartBehaviour();

    /// <summary>
    /// Immediatly disables spell gameobject.
    /// </summary>
    /// <param name="parent">Spell parent.</param>
    public void DisableSpell(SpellBehaviourAbstract parent) =>
        spell.SpellBehaviour.DisableSpell(parent);
}
