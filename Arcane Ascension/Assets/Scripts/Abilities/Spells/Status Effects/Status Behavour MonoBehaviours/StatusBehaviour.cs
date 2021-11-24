using System.Collections;
using UnityEngine;

/// <summary>
/// Class responsible for running status logic.
/// </summary>
public class StatusBehaviour : MonoBehaviour
{
    /// <summary>
    /// Parent spell of this 
    /// </summary>
    public ISpell Spell { get; set; }

    /// <summary>
    /// Time of spawn.
    /// </summary>
    public float TimeSpawned { get; set; }

    private Stats characterHit;

    /// <summary>
    /// Which character is being affected by this spell.
    /// </summary>
    public Stats CharacterHit
    {
        get => characterHit;
        set
        {
            characterHit = value;
            characterHit.EventDeath += DisableStatusGameObject;
        }
    }

    /// <summary>
    /// Which character cast the spell of this status.
    /// </summary>
    public Stats WhoCast { get; set; }

    private YieldInstruction wffu;

    private void Awake()
    {
        wffu = new WaitForFixedUpdate();
    }

    private void OnDisable()
    {
        if(characterHit != null)
        {
            characterHit.EventDeath -= DisableStatusGameObject;
            CharacterHit = null;
        }

        TimeSpawned = 0;
        WhoCast = null;
        Spell = null;
    }

    /// <summary>
    /// Method called after instantiating the spell.
    /// Must be called manually through this method instead of OnEnable or Start in order to prevent bugs.
    /// </summary>
    public void TriggerStartBehaviour()
    {
        TimeSpawned = Time.time;
        StartCoroutine(TriggerStartBehaviourCoroutine());
    }

    private IEnumerator TriggerStartBehaviourCoroutine()
    {
        yield return wffu;
        yield return wffu;
        if (Spell?.StatusBehaviour != null)
            Spell.StatusBehaviour.StartBehaviour(this);
    }

    private void Update()
    {
        if (Spell?.StatusBehaviour != null)
            Spell.StatusBehaviour.ContinuousUpdateBehaviour(this);
    }

    private void FixedUpdate()
    {
        if (CharacterHit != null)
            transform.position = CharacterHit.transform.position;
    }

    /// <summary>
    /// Disables status gameobject.
    /// </summary>
    /// <param name="emptyVariable">Variable to match death event.</param>
    public void DisableStatusGameObject(Stats emptyVariable = null)
    {
        characterHit.EventDeath -= DisableStatusGameObject;
        gameObject.SetActive(false);
    }
}
