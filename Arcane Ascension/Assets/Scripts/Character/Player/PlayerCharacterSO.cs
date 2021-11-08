using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptable object with player character whole information.
/// </summary>
[InlineEditor]
[CreateAssetMenu(menuName = "Character/Player Character", fileName = "Player Character")]
public class PlayerCharacterSO : CharacterSO
{
    [SerializeField] private PlayerCurrencySO currency;

    /// <summary>
    /// Currency of player.
    /// </summary>
    public PlayerCurrencySO Currency => currency;
}
