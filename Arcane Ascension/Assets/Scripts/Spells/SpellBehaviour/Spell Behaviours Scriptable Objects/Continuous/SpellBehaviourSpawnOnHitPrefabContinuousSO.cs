using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Scriptable object responsible for creating a spell's behaviour.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/Continuous/Spell Behaviour Spawn On Hit Prefab", 
    fileName = "Spell Behaviour Spawn On Hit Prefab")]
sealed public class SpellBehaviourSpawnOnHitPrefabContinuousSO : SpellBehaviourAbstractContinuousSO
{
    [Range(0f, 2f)][SerializeField] private float hitSpawnDelay;
    [Range(0f, 0.3f)] [SerializeField] private float distanceFromWall = 0.1f;

    public override void StartBehaviour(SpellBehaviourContinuous parent)
    {
        // Left blank on purpose
    }

    public override void ContinuousUpdateBehaviour(SpellBehaviourContinuous parent)
    {
        // If the spell is in hit time (updated on parent)
        if (Time.time > parent.LastTimeSpellWallHit + hitSpawnDelay)
        {
            if (parent.CurrentSpellDistance.Similiar(parent.HitPoint.distance, 1f))
            {
                // Creates spell muzzle prefab
                // Update() will run from its monobehaviour script
                // Creates spell muzzle prefab
                GameObject spellOnHitBehaviourGameObject = SpellHitPoolCreator.Pool.InstantiateFromPool(
                    parent.Spell.Name, parent.HitPoint.point + parent.HitPoint.normal * distanceFromWall,
                    Quaternion.LookRotation(parent.HitPoint.normal, parent.transform.up));

                // Gets muzzle behaviour from it
                if (spellOnHitBehaviourGameObject.TryGetComponent<SpellOnHitBehaviourContinuous>
                    (out SpellOnHitBehaviourContinuous onHitBehaviour))
                {
                    if (onHitBehaviour.Spell != parent.Spell)
                    {
                        onHitBehaviour.Spell = parent.Spell;
                        onHitBehaviour.SpellMonoBehaviour = parent;
                    }
                }
            }
        }
    }

    public override void ContinuousFixedUpdateBehaviour(SpellBehaviourContinuous parent)
    {
        // Left blank on purpose
    }

    public override void HitTriggerBehaviour(Collider other, SpellBehaviourContinuous parent)
    {
        // Left blank on purpose
    }
}
