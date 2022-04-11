using UnityEngine;

/// <summary>
/// Class responsible for growing particles radius depending on a spell AoE.
/// </summary>
public class FireDemonImmolateRadiusGrow : MonoBehaviour
{
    [SerializeField] private ParticleSystem risingRings;
    [SerializeField] private ParticleSystem smokeParticles;
    [SerializeField] private ParticleSystem fireParticles;
    private ParticleSystem.ShapeModule smokeShape;
    private ParticleSystem.ShapeModule fireShape;
    private SpellBehaviourOneShot spell;

    private void Awake()
    {
        smokeShape = smokeParticles.shape;
        fireShape = fireParticles.shape;
        spell = GetComponent<SpellBehaviourOneShot>();
    }

    private void OnEnable()
    {
        smokeShape.radius = 0;
        fireShape.radius = 0;
        risingRings.transform.localScale = 
            new Vector3(0, risingRings.transform.localScale.y, 0);
    }

    private void Update()
    {
        float radius = spell.Spell.AreaOfEffect;
        smokeShape.radius = radius;
        fireShape.radius = radius;
        risingRings.transform.localScale =
            new Vector3(radius / risingRings.startSize, 
                risingRings.transform.localScale.y,
                radius / risingRings.startSize);
    }
}
