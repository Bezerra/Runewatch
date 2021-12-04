using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// Class responsible for all player input.
/// The game is using Unity Events because PerformInteractiveRebinding 
/// (in RebindActionUI script) only works with unity events.
/// </summary>
public class PlayerInputCustom : MonoBehaviour, IInput
{
    private PlayerInput controls;
    private InputActions inputActionsAsset;

    private void Awake()
    {
        inputActionsAsset = new InputActions();
        controls = GetComponent<PlayerInput>();

        // TEMPORARY STUFF FOR EDITOR SCENES
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name != "LoadingScreenToMainMenu" &&
            scene.name != "LoadingScreenToNewGame" &&
            scene.name != "MainMenu" &&
            scene.name != "ProceduralGeneration")
        {
            SwitchActionMapToGameplay();
        }
    }

    private void OnEnable()
    {
        inputActionsAsset.Enable();
    }

    private void OnDisable()
    {
        inputActionsAsset.Disable();
    }

    public void ReenableInput()
    {
        inputActionsAsset.Disable();
        inputActionsAsset.Enable();
    }

    public Vector2 Movement { get; private set; }
    public Vector2 Camera { get; private set; }

    public void SwitchActionMapToGameplay()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        controls.uiInputModule.enabled = false;

        controls.SwitchCurrentActionMap("Gameplay");
    }

    public void SwitchActionMapToNone()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        controls.uiInputModule.enabled = false;

        controls.SwitchCurrentActionMap("None");
    }

    public void SwitchActionMapToUI()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        controls.uiInputModule.enabled = true;

        Movement = Vector3.zero;
        Camera = Vector3.zero;

        controls.SwitchCurrentActionMap("Interface");
    }

    public void SwitchActionMapToAbilitiesUI()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        controls.uiInputModule.enabled = true;

        Movement = Vector3.zero;
        Camera = Vector3.zero;

        controls.SwitchCurrentActionMap("AbilityChoice");
    }

    public void SwitchActionMapToCheatConsole()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Movement = Vector3.zero;
        Camera = Vector3.zero;

        controls.uiInputModule.enabled = true;

        controls.SwitchCurrentActionMap("CheatsConsole");
    }

    public void DisableAll()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        controls.uiInputModule.enabled = false;

        controls.SwitchCurrentActionMap("Nothing");
    }

    public string GetCurrentActionMap() => controls.currentActionMap.name;

    //////////////////////// Methods /////////////////////////////////////////
    public void HandleMovement(InputAction.CallbackContext context) {
        if (context.performed) Movement = context.ReadValue<Vector2>();
    }
    public void HandleRun(InputAction.CallbackContext context) {
        if (context.started) OnRun(true);
        if (context.canceled) OnRun(false);
    }
    public void HandleDash(InputAction.CallbackContext context)
    {
        if (context.started) OnDash();
    }
    public void HandleCamera(InputAction.CallbackContext context) {
        if (context.performed) Camera = context.ReadValue<Vector2>();
    }
    public void HandleJump(InputAction.CallbackContext context) {
        if (context.started) OnJump();
    }
    public void HandleCastSpell(InputAction.CallbackContext context) {
        if (context.started) OnCastSpell();
        if (context.canceled) OnStopCastSpell();
    }
    public void HandleCastBasicSpell(InputAction.CallbackContext context)
    {
        if (context.started) OnCastBasicSpell();
        if (context.canceled) OnStopBasicCastSpell();
    }
    public void HandleSelectFirstSpell(InputAction.CallbackContext context) {
        if (context.started) SelectSpell(0);
    }
    public void HandleSelectSecondSpell(InputAction.CallbackContext context) {
        if (context.started) SelectSpell(1);
    }
    public void HandleSelectThirdSpell(InputAction.CallbackContext context) {
        if (context.started) SelectSpell(2);
    }
    public void HandleSelectForthSpell(InputAction.CallbackContext context) {
        if (context.started) SelectSpell(3);
    }
    public void HandleQuickSave(InputAction.CallbackContext context)
    {
        if (context.started) RunSaveDataController.SaveGame();
    }
    public void HandleQuickLoad(InputAction.CallbackContext context)
    {
        if (context.started) RunSaveDataController.LoadGame();
    }
    public void HandleLeftClickUI(InputAction.CallbackContext context) {
        if (context.started) OnClick(Direction.Left);
    }
    public void HandleRightClickUI(InputAction.CallbackContext context) {
        if (context.started) OnClick(Direction.Right);
    }
    public void HandlePauseGame(InputAction.CallbackContext context)
    {
        if (context.started) OnPauseGame();
    }
    public void HandleCheatConsole(InputAction.CallbackContext context)
    {
        if (context.started) OnCheatConsole();
    }
    public void HandleInteract(InputAction.CallbackContext context)
    {
        if (context.started) OnInteract();
    }
    public void HandlePreviousNextSpellMouseScrollSelect(InputAction.CallbackContext context)
    {
        if (context.performed) OnPreviousNextSpell(context.ReadValue<Vector2>().y, true);
    }
    public void HandleNextSpellSelect(InputAction.CallbackContext context)
    {
        if (context.performed) OnPreviousNextSpell(-1, false);
    }
    public void HandlePreviousSpellSelect(InputAction.CallbackContext context)
    {
        if (context.performed) OnPreviousNextSpell(1, false);
    }
    ///////////////////////// Events /////////////////////////////////////////
    protected virtual void OnDash() => Dash?.Invoke();
    public event Action Dash;
    protected virtual void OnRun(bool condition) => Run?.Invoke(condition);
    public event Action<bool> Run;
    protected virtual void OnJump() => Jump?.Invoke();
    public event Action Jump;
    protected virtual void OnCastSpell() => CastSpell?.Invoke();
    public event Action CastSpell;
    protected virtual void OnCastBasicSpell() => CastBasicSpell?.Invoke();
    public event Action CastBasicSpell;
    protected virtual void OnStopCastSpell() => StopCastSpell?.Invoke();
    public event Action StopCastSpell;
    protected virtual void OnStopBasicCastSpell() => StopBasicCastSpell?.Invoke();
    public event Action StopBasicCastSpell;
    protected virtual void OnSelectSpell(byte index) => SelectSpell?.Invoke(index);
    public event Action<byte> SelectSpell;
    protected virtual void OnClick(Direction dir) => Click?.Invoke(dir);
    public event Action<Direction> Click;
    protected virtual void OnPauseGame() => PauseGame?.Invoke();
    public event Action PauseGame;
    protected virtual void OnCheatConsole() => CheatConsole?.Invoke();
    public event Action CheatConsole;
    protected virtual void OnInteract() => Interact?.Invoke();
    public event Action Interact;
    protected virtual void OnPreviousNextSpell(float axis, bool withDelay) => PreviousNextSpell?.Invoke(axis, withDelay);
    public event Action<float, bool> PreviousNextSpell;
}
