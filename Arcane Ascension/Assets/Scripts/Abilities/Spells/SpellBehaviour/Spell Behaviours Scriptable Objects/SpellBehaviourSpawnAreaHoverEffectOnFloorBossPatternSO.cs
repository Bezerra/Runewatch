using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Scriptable object responsible for spawning an hover vfx on ground.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour " +
    "Spawn Area Hover Effect On Floor Boss Pattern", 
    fileName = "Spell Behaviour Spawn Area Hover Effect On Floor Boss Pattern")]
sealed public class SpellBehaviourSpawnAreaHoverEffectOnFloorBossPatternSO : SpellBehaviourAbstractSO
{
    [Range(0f, 0.3f)] [SerializeField] private float distanceFromWall = 0.05f;
    [Range(0.5f, 4f)] [SerializeField] private float ROTATIONFREQUENCY = 2f;
    [Range(0.5f, 4f)] [SerializeField] private float DISTANCEGROWTH = 2.5f;

    private Vector3 DISTANTVECTOR = new Vector3(10000, 10000, 10000);

    public override void StartBehaviour(SpellBehaviourOneShot parent)
    {
        foreach (ParticleDisable particles in parent.ParticlesDisable)
            particles.gameObject.SetActive(true);
    }

    public override void ContinuousUpdateBeforeSpellBehaviour(SpellBehaviourOneShot parent)
    {
        foreach (ParticleDisable particles in parent.ParticlesDisable)
            particles.gameObject.SetActive(false);

        // If who cast does not exist anymore, cancels effects
        if (parent.WhoCast == null)
        {
            return;
        }

        // Will be set to true in another movement behaviour start behaviour
        if (parent.ColliderTrigger != null)
            parent.ColliderTrigger.enabled = false;

        // If it's a boss
        if (parent.WhoCast.CommonAttributes.Type == CharacterType.Boss)
        {
            // If time while casting reaches the time limit set on enemy spell list,
            // shows one shot with release spell area.
            // Code only happens once.
            float timeEnemyIsStopped = Time.time - parent.AICharacter.TimeEnemyStoppedWhileAttacking;
            if (timeEnemyIsStopped > parent.AICharacter.CurrentlySelectedSpell.StoppingTime * 
                parent.AICharacter.CurrentlySelectedSpell.PercentageStoppingTimeTriggerAoESpell &&
                parent.AreaHoverVFXMultiple == null)
            {
                parent.AreaHoverVFXMultiple = new GameObject[3];
                parent.ExtraGameObjects = new GameObject[3];

                if (parent.AICharacter.CurrentTarget == null)
                    return;

                Ray playerFloorPosition = new Ray(
                    parent.AICharacter.CurrentTarget.position, Vector3.down);

                if (Physics.Raycast(playerFloorPosition, out RaycastHit floorHit, 5, Layers.WallsFloor))
                {
                    if (parent.AreaHoverVFXMultiple[0] == null)
                    {
                        for (int i = 0; i < parent.AreaHoverVFXMultiple.Length; i++)
                        {
                            parent.AreaHoverVFXMultiple[i] =
                                SpellAreaHoverPoolCreator.Pool.InstantiateFromPool(
                                parent.Spell.Name, DISTANTVECTOR,
                                Quaternion.identity);

                            if (parent.AreaHoverVFXMultiple[i].TryGetComponent(
                                out AreaTargetSizeUpdate targetSize))
                            {
                                targetSize.UpdateAreaTargetSize(
                                    parent.Spell.AreaOfEffect, parent.WhoCast);
                            }

                            // Spawns extra game objects
                            if (parent.ExtraGameObjectName != string.Empty)
                            {
                                parent.ExtraGameObjects[i] = 
                                    SpellExtraGameObjectsPool.Pool.InstantiateFromPool(
                                        parent.ExtraGameObjectName, 
                                        parent.Eyes.transform.position,
                                        Quaternion.identity);
                            }
                        }
                    }

                    parent.AreaHoverVFXMultiple[0].transform.SetPositionAndRotation(
                        floorHit.point + floorHit.normal * distanceFromWall,
                        Quaternion.LookRotation(floorHit.normal, floorHit.collider.transform.up) *
                        Quaternion.Euler(90, 0, 0));
                }

                // Default value so it doesn't spawn in the center of the initial spell
                parent.TimeInsideSpellChanneling = 5;
            }

            if (timeEnemyIsStopped > parent.AICharacter.CurrentlySelectedSpell.StoppingTime *
                parent.AICharacter.CurrentlySelectedSpell.PercentageStoppingTimeTriggerAoESpell)
            {
                parent.TimeInsideSpellChanneling += Time.deltaTime * DISTANCEGROWTH;

                // X + Z positions
                float xPos = Mathf.Cos(Time.time * ROTATIONFREQUENCY) * 
                    parent.TimeInsideSpellChanneling;
                float zPos = Mathf.Sin(Time.time * ROTATIONFREQUENCY) *
                    parent.TimeInsideSpellChanneling;

                // Circle 1 position update
                parent.AreaHoverVFXMultiple[1].transform.SetPositionAndRotation(
                    parent.AreaHoverVFXMultiple[0].transform.position + new Vector3(xPos, 0, zPos),
                    parent.AreaHoverVFXMultiple[0].transform.rotation);

                // Circle 2 position update
                parent.AreaHoverVFXMultiple[2].transform.SetPositionAndRotation(
                    parent.AreaHoverVFXMultiple[0].transform.position + new Vector3(-xPos, 0, -zPos),
                    parent.AreaHoverVFXMultiple[0].transform.rotation);

                // If this spell has extra game objects, it will move them towards
                // the created patterns
                if (parent.ExtraGameObjects != null)
                {
                    for (int i = 0; i < parent.ExtraGameObjects.Length; i++)
                    {
                        parent.ExtraGameObjects[i].transform.Translate(
                            parent.ExtraGameObjects[i].transform.Direction(
                                parent.AreaHoverVFXMultiple[i].transform) *
                                Time.deltaTime * 25f);
                    }
                }
            }
        }
    }

    public override void ContinuousUpdateBehaviour(SpellBehaviourOneShot parent)
    {
        // If the spell is already in motion, it will disable this game object
        if (parent.SpellStartedMoving == true)
        {
            for (int i = 0; i < parent.AreaHoverVFXMultiple.Length; i++)
            {
                parent.AreaHoverVFXMultiple[i].SetActive(false);
            }
        }
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
