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
}
