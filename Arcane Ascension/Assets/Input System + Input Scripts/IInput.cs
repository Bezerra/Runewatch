using System;
using UnityEngine;

/// <summary>
/// Interface implemented by input.
/// </summary>
public interface IInput
{
    void SwitchActionMapToGameplay();

    void SwitchActionMapToNone();

    void SwitchActionMapToUI();

    void SwitchActionMapToAbilitiesUI();

    void SwitchActionMapToCheatConsole();

    void DisableAll();

    string GetCurrentActionMap();

    Vector2 Movement { get; }

    Vector2 Camera { get; }

    bool HoldingCastSpell { get; }

    void ReenableInput();

    event Action Dash;
    event Action Run;
    event Action Jump;
    event Action CastSpell;
    event Action CastBasicSpell;
    event Action StopCastSpell;
    event Action StopBasicCastSpell;
    event Action<byte> SelectSpell;
    event Action<Direction> Click;
    event Action PauseGame;
    event Action CheatConsole;
    event Action Interact;
    event Action<float, bool> PreviousNextSpell;
    event Action<bool> HoldingToBuyEvent;
    event Action<bool> HoldingToHideEvent;
}
