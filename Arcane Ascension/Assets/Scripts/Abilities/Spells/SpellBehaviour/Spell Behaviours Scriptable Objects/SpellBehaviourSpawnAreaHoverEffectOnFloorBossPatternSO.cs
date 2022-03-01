using UnityEngine;

/// <summary>
/// Scriptable object responsible for spawning an hover vfx on ground.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour " +
    "Spawn Area Hover Effect On Floor Boss Pattern", 
    fileName = "Spell Behaviour Spawn Area Hover Effect On Floor Boss Pattern")]
sealed public class SpellBehaviourSpawnAreaHoverEffectOnFloorBossPatternSO : SpellBehaviourAbstractSO
{
    [Range(0f, 0.3f)] [SerializeField] private float distanceFromWall = 0.05f;

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
            if (parent.AreaHoverVFXMultiple != null && parent.AreaHoverVFXMultiple[0] != null)
            {
                for (int i = 0; i < parent.AreaHoverVFXMultiple.Length; i++)
                {
                    parent.AreaHoverVFXMultiple[i].SetActive(false);
                }
                return;
            }
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
                                targetSize.UpdateAreaTargetSize(parent.Spell.AreaOfEffect);
                            }
                        } 
                    }

                    parent.AreaHoverAreaHit = floorHit;

                    for (int i = 0; i < parent.AreaHoverVFXMultiple.Length; i++)
                    {
                        parent.AreaHoverVFXMultiple[i].transform.SetPositionAndRotation(
                            floorHit.point + floorHit.normal * distanceFromWall + new Vector3(i*7, 0, 0),
                            Quaternion.LookRotation(floorHit.normal, floorHit.collider.transform.up) *
                            Quaternion.Euler(90, 0, 0));
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
            if (parent.AreaHoverVFXMultiple != null && parent.AreaHoverVFXMultiple.Length > 0)
            {
                for (int i = 0; i < parent.AreaHoverVFXMultiple.Length; i++)
                {
                    parent.AreaHoverVFXMultiple[i].SetActive(false);
                }
                return;
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
