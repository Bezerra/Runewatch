using UnityEngine;
using System.Collections;

#pragma warning disable 0414 // variable assigned but not used.

/// <summary>
/// Class responsible for generating a spell hit cinemachine source impulse.
/// Used by every spell hit that generates impulses.
/// </summary>
public class SpellHitGenerateCinemachineImpulse : GenerateCinemachineImpulse
{
    [SerializeField] private string notes = "Uses spell DelayToDoDamage as a timer to generate.";

    private SpellOnHitBehaviourAbstract spellBehaviour;
    private IEnumerator generateImpulseCoroutine;
    private YieldInstruction wfs;

    protected override void Awake()
    {
        base.Awake();
        spellBehaviour = GetComponent<SpellOnHitBehaviourAbstract>();
        wfs = new WaitForSeconds(spellBehaviour.SpellInformation.DelayToDoDamage);
    }

    private void OnEnable()
    {
        generateImpulseCoroutine = GenerateImpulseCoroutine();
        StartCoroutine(generateImpulseCoroutine);
    }

    private IEnumerator GenerateImpulseCoroutine()
    {
        yield return wfs;

        if (spellBehaviour.Spell != null)
        {
            GenerateImpulse();
        }
    }
}
