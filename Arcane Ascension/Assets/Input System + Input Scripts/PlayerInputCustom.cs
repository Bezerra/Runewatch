using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using System.Collections;

/// <summary>
/// Class responsible for all player input.
/// The game is using Unity Events because PerformInteractiveRebinding 
/// (in RebindActionUI script) only works with unity events.
/// </summary>
public class PlayerInputCustom : MonoBehaviour
{
    private PlayerInput controls;
    private InputActions inputActionsAsset;

    [Header("Unity default UI input actions")]
    [SerializeField] private InputActionAsset uiInputActions;

    private void Awake()
    {
        inputActionsAsset = new InputActions();
        controls = GetComponent<PlayerInput>();
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

        controls.uiInputModule.enabled = false;

        UnsubscribeCheatConsole();
        SubscribeGameplay();
    }

    public void SwitchActionMapToUI()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        Movement = Vector3.zero;
        Camera = Vector3.zero;

        controls.uiInputModule.enabled = true;
        controls.uiInputModule.actionsAsset = uiInputActions;

        UnsubscribeCheatConsole();
        UnsubscribeGameplay();
    }

    public void SwitchActionMapToCheatConsole()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Movement = Vector3.zero;
        Camera = Vector3.zero;

        controls.uiInputModule.enabled = true;

        UnsubscribeGameplay();
        StartCoroutine(SubscribeCheatConsoleCoroutine());
    }

    private IEnumerator SubscribeCheatConsoleCoroutine()
    {
        yield return new WaitForFixedUpdate();
        SubscribeCheatConsole();
    }

    public void DisableAll()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        UnsubscribeGameplay();
        UnsubscribeCheatConsole();

        controls.uiInputModule.enabled = false;
    }

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
        if (context.started) OnClick();
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
        if (context.started) OnPreviousNextSpell(-1, false);
    }
    public void HandlePreviousSpellSelect(InputAction.CallbackContext context)
    {
        if (context.started) OnPreviousNextSpell(1, false);
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
    protected virtual void OnClick() => Click?.Invoke();
    public event Action Click;
    protected virtual void OnPauseGame() => PauseGame?.Invoke();
    public event Action PauseGame;
    protected virtual void OnCheatConsole() => CheatConsole?.Invoke();
    public event Action CheatConsole;
    protected virtual void OnInteract() => Interact?.Invoke();
    public event Action Interact;
    protected virtual void OnPreviousNextSpell(float axis, bool withDelay) => PreviousNextSpell?.Invoke(axis, withDelay);
    public event Action<float, bool> PreviousNextSpell;

    private void SubscribeGameplay()
    {
        inputActionsAsset.Gameplay.Movement.performed += HandleMovement;

        inputActionsAsset.Gameplay.Jump.started += HandleJump;

        inputActionsAsset.Gameplay.Camera.performed += HandleCamera;

        inputActionsAsset.Gameplay.CastSpell.started += HandleCastSpell;
        inputActionsAsset.Gameplay.CastSpell.canceled += HandleCastSpell;

        inputActionsAsset.Gameplay.CastBasicSpell.started += HandleCastBasicSpell;
        inputActionsAsset.Gameplay.CastBasicSpell.canceled += HandleCastBasicSpell;

        inputActionsAsset.Gameplay.SelectSpell1.started += HandleSelectFirstSpell;

        inputActionsAsset.Gameplay.SelectSpell2.started += HandleSelectSecondSpell;

        inputActionsAsset.Gameplay.SelectSpell3.started += HandleSelectThirdSpell;

        inputActionsAsset.Gameplay.SelectSpell4.started += HandleSelectForthSpell;

        inputActionsAsset.Gameplay.Run.started += HandleRun;
        inputActionsAsset.Gameplay.Run.canceled += HandleRun;

        inputActionsAsset.Gameplay.Dash.started += HandleDash;

        inputActionsAsset.Gameplay.QuickSave.started += HandleQuickSave;

        inputActionsAsset.Gameplay.QuickLoad.started += HandleQuickLoad;

        inputActionsAsset.Gameplay.Pause.started += HandlePauseGame;

        inputActionsAsset.Gameplay.CheatConsole.started += HandleCheatConsole;

        inputActionsAsset.Gameplay.Interact.started += HandleInteract;

        inputActionsAsset.Gameplay.PreviousAndNextSpellMouseScroll.performed +=
            HandlePreviousNextSpellMouseScrollSelect;

        inputActionsAsset.Gameplay.NextSpell.started += HandleNextSpellSelect;

        inputActionsAsset.Gameplay.PreviousSpell.started += HandlePreviousSpellSelect;
    }

    private void UnsubscribeGameplay()
    {
        inputActionsAsset.Gameplay.Movement.performed -= HandleMovement;

        inputActionsAsset.Gameplay.Jump.started -= HandleJump;

        inputActionsAsset.Gameplay.Camera.performed -= HandleCamera;

        inputActionsAsset.Gameplay.CastSpell.started -= HandleCastSpell;
        inputActionsAsset.Gameplay.CastSpell.canceled -= HandleCastSpell;

        inputActionsAsset.Gameplay.CastBasicSpell.started -= HandleCastBasicSpell;
        inputActionsAsset.Gameplay.CastBasicSpell.canceled -= HandleCastBasicSpell;

        inputActionsAsset.Gameplay.SelectSpell1.started -= HandleSelectFirstSpell;

        inputActionsAsset.Gameplay.SelectSpell2.started -= HandleSelectSecondSpell;

        inputActionsAsset.Gameplay.SelectSpell3.started -= HandleSelectThirdSpell;

        inputActionsAsset.Gameplay.SelectSpell4.started -= HandleSelectForthSpell;

        inputActionsAsset.Gameplay.Run.started -= HandleRun;
        inputActionsAsset.Gameplay.Run.canceled -= HandleRun;

        inputActionsAsset.Gameplay.Dash.started -= HandleDash;

        inputActionsAsset.Gameplay.QuickSave.started -= HandleQuickSave;

        inputActionsAsset.Gameplay.QuickLoad.started -= HandleQuickLoad;

        inputActionsAsset.Gameplay.Pause.started -= HandlePauseGame;

        inputActionsAsset.Gameplay.CheatConsole.started -= HandleCheatConsole;

        inputActionsAsset.Gameplay.Interact.started -= HandleInteract;

        inputActionsAsset.Gameplay.PreviousAndNextSpellMouseScroll.performed -=
            HandlePreviousNextSpellMouseScrollSelect;

        inputActionsAsset.Gameplay.NextSpell.started -= HandleNextSpellSelect;

        inputActionsAsset.Gameplay.PreviousSpell.started -= HandlePreviousSpellSelect;
    }

    private void SubscribeCheatConsole()
    {
        inputActionsAsset.CheatsConsole.CheatConsole.started += HandleCheatConsole;
    }

    private void UnsubscribeCheatConsole()
    {
        inputActionsAsset.CheatsConsole.CheatConsole.started -= HandleCheatConsole;
    }
}
