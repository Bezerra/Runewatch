using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptable object responsible for creating burn status behaviour.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Status/Status Behaviour Burn",
    fileName = "Status Behaviour Burn")]
[InlineEditor]
public class StatusBehaviourBurnSO : StatusBehaviourAbstractSO
{
    [Range(0, 4f)][SerializeField] private float damageToDo;
    [Range(0, 2f)] [SerializeField] private float damageInterval;
    [Range(0, 10f)] [SerializeField] private float damageMaxTime;

    public override void StartBehaviour(StatusBehaviour parent)
    {
        base.StartBehaviour(parent);

        if (parent.CharacterHit != null)
        {

            parent.CharacterHit.TakeDamageOvertime(
                damageToDo, ElementType.Fire, damageInterval, damageMaxTime);
        }

        parent.CurrentlyActive = true;
    }

    public override void ContinuousUpdateBehaviour(StatusBehaviour parent)
    {
        if (Time.time - parent.TimeSpawned > damageMaxTime)
            parent.DisableStatusGameObject();
    }
}
