using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptable object with player character whole information.
/// </summary>
[InlineEditor]
[CreateAssetMenu(menuName = "Character/Player Character", fileName = "Player Character")]
public class PlayerCharacterSO : CharacterSO
{
    [SerializeField] private CurrencySO currency;

    /// <summary>
    /// Currency of player.
    /// </summary>
    public CurrencySO Currency => currency;
}
