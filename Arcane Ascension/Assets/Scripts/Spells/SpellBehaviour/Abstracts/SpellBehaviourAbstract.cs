using UnityEngine;

/// <summary>
/// Abstract class responsible for executing a spell behaviour.
/// </summary>
public abstract class SpellBehaviourAbstract : MonoBehaviour
{
    [SerializeField] protected SpellSO spell;

    public Transform Hand { get; private set; }
    public Transform Eyes { get; private set; }

    private Stats whoCast;
    public Stats WhoCast 
    { 
        get => whoCast; 
        set 
        { 
            whoCast = value; 
            Hand = WhoCast.GetComponent<Character>().Hand;
            Eyes = WhoCast.GetComponent<Character>().Eyes;
        }
    }

    public ISpell Spell => spell;

    /// <summary>
    /// Immediatly disables spell gameobject.
    /// </summary>
    /// <param name="parent">Spell parent.</param>
    public void DisableSpell(SpellBehaviourAbstract parent) =>
        spell.SpellBehaviour.DisableSpell(parent);
}
