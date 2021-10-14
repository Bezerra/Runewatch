using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Abstract scriptable object for damage behaviours.
/// </summary>
[InlineEditor]
public abstract class DamageBehaviourAbstractSO : ScriptableObject, IDamageBehaviour
{
    public abstract void Damage(Collider other, SpellBehaviourAbstract parent);
}
