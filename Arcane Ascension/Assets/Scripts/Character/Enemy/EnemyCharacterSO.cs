using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptable object with enemy character whole information.
/// </summary>
[InlineEditor]
[CreateAssetMenu(menuName = "Enemy Character", fileName = "Enemy Character")]
public class EnemyCharacterSO : CharacterSO
{
    // Variable that updates depending if characterValuesSO is player or enemy
    //[PropertySpace(20)]
    //[SerializeField] private FSMState initialState;
    //[SerializeField] private FSMState nullState;
    //
    //public FSMState InitialState => initialState;
    //public FSMState NullState => nullState;
}
