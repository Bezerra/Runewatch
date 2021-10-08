using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

/// <summary>
/// Class responsible for all player input.
/// The game is using Unity Events because PerformInteractiveRebinding 
/// (in RebindActionUI script) only works with unity events.
/// </summary>
public class PlayerInputCustom : MonoBehaviour
{
    private PlayerInput controls;
    private InputActions inputActionsAsset;
    private InputSystemUIInputModule uiModule;

    private void Awake()
    {
        inputActionsAsset = new InputActions();
        controls = GetComponent<PlayerInput>();
        uiModule = GetComponentInChildren<InputSystemUIInputModule>();
    }

    private void OnEnable()
    {
        inputActionsAsset.Enable();
        SwitchActionMapToGameplay();
    }

    private void OnDisable()
    {
        inputActionsAsset.Disable();
    }

    public Vector2 Movement { get; private set; }
    public Vector2 Camera { get; private set; }


    public void SwitchActionMapToGameplay()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        uiModule.enabled = true;
        controls.SwitchCurrentActionMap("Gameplay");
    }

    public void SwitchActionMapToUI()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        uiModule.enabled = true;
        controls.SwitchCurrentActionMap("Interface");
    }

    public void DisableAll()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        controls.SwitchCurrentActionMap("Nothing");
        uiModule.enabled = false;
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
        if (context.started) SaveDataController.SaveGame();
    }
    public void HandleQuickLoad(InputAction.CallbackContext context)
    {
        if (context.started) SaveDataController.LoadGame();
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
    ///////////////////////// Events /////////////////////////////////////////
    protected virtual void OnRun(bool running) => Run?.Invoke(running);
    public event Action<bool> Run;
    protected virtual void OnJump() => Jump?.Invoke();
    public event Action Jump;
    protected virtual void OnCastSpell() => CastSpell?.Invoke();
    public event Action CastSpell;
    protected virtual void OnStopCastSpell() => StopCastSpell?.Invoke();
    public event Action StopCastSpell;
    protected virtual void OnSelectSpell(byte index) => SelectSpell?.Invoke(index);
    public event Action<byte> SelectSpell;
    protected virtual void OnClick(Direction dir) => Click?.Invoke(dir);
    public event Action<Direction> Click;
    protected virtual void OnPauseGame() => PauseGame?.Invoke();
    public event Action PauseGame;
}
