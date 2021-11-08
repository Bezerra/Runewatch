using UnityEngine;
using System.Collections;

/// <summary>
/// Class responsible for generating a spell hit cinemachine source impulse.
/// Used by every gameobject that generates impulses.
/// </summary>
public class SpellHitGenerateCinemachineImpulse : GenerateCinemachineImpulse
{
    [SerializeField] private string notes = "Uses spell SPEED as a timer to generate.";

    private SpellOnHitBehaviourAbstract spellBehaviour;
    private IEnumerator generateImpulseCoroutine;
    private YieldInstruction wfs;

    protected override void Awake()
    {
        base.Awake();
        spellBehaviour = GetComponent<SpellOnHitBehaviourAbstract>();
        wfs = new WaitForSeconds(spellBehaviour.SpellInformation.Speed);
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
            cinemachineSource.GenerateImpulse(mainCam.transform.forward);
        }
    }
}
