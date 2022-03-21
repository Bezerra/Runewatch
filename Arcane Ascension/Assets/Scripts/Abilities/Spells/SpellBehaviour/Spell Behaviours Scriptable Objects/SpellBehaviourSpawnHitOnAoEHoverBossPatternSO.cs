using UnityEngine;

/// <summary>
/// Scriptable object responsible for spawning hit of AoE hover spell boss pattern.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour " +
    "Spawn Hit Aoe Hover Spell Boss Pattern",
    fileName = "Spell Behaviour Spawn Hit Aoe Hover Spell Boss Pattern")]
public class SpellBehaviourSpawnHitOnAoEHoverBossPatternSO : SpellBehaviourAbstractSO
{
    public override void StartBehaviour(SpellBehaviourOneShot parent)
    {
        // Needed to run other behaviours
        parent.SpellStartedMoving = true;
        parent.TimeOfImpact = Time.time;
        parent.TimeSpawned = Time.time;

        parent.transform.position = parent.AreaHoverAreaHit.point + parent.AreaHoverAreaHit.normal;
        parent.PositionOnHit = parent.AreaHoverAreaHit.point + parent.AreaHoverAreaHit.normal;

        parent.SpellsInPatternsTransforms = new Transform[parent.AreaHoverVFXMultiple.Length];

        for (int i = 0; i < parent.AreaHoverVFXMultiple.Length; i++)
        {
            // Spawns hit
            GameObject onHitBehaviourGameObject = SpellHitPoolCreator.Pool.InstantiateFromPool(
                parent.Spell.Name, parent.AreaHoverVFXMultiple[i].transform.position,
                Quaternion.identity);

            if (onHitBehaviourGameObject.TryGetComponent<SpellOnHitBehaviourOneShot>(
                out SpellOnHitBehaviourOneShot onHitBehaviour))
            {
                // Sets hit Spell to this spell
                if (onHitBehaviour.Spell != parent.Spell)
                    onHitBehaviour.Spell = parent.Spell;

                parent.SpellsInPatternsTransforms[i] = onHitBehaviour.transform;
            }

            if (parent.ExtraGameObjects != null)
            {
                parent.ExtraGameObjects[i].SetActive(false);
            }
        }
    }

    public override void ContinuousUpdateBeforeSpellBehaviour(SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
    }

    public override void ContinuousUpdateBehaviour(SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
    }

    public override void ContinuousFixedUpdateBehaviour(SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
    }

    public override void HitTriggerBehaviour(Collider other, SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
    }
}
